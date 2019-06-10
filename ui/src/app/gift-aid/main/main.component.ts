import {AfterViewInit, Component, Inject, OnDestroy} from '@angular/core';
import {CURRENCY_CODE} from '../core/tokens/currency.token';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {GiftAidService} from '../core/services/gift-aid.service';
import {debounceTime, distinctUntilChanged, finalize, map, shareReplay, startWith, switchMap, takeUntil} from 'rxjs/operators';
import {Observable, Subject} from 'rxjs';
import {concatMap} from 'rxjs/internal/operators/concatMap';
import {tap} from 'rxjs/internal/operators/tap';
import {ISendDonationDto} from '../core/services/models/gift-aid.models';
import {MatSnackBar} from '@angular/material';
import {filter} from 'rxjs/internal/operators/filter';

export class TaxInfoModel {
    amount: number;
    giftAid: number;
    taxRate: number;
    totalAmount: number;

    constructor(content: Partial<TaxInfoModel>) {
        Object.assign(this, content);
        if (content.giftAid) {
            this.totalAmount = this.amount + this.giftAid;
            this.taxRate = (this.giftAid / this.totalAmount) * 100;
        }
    }
}

export enum DonationFormFieldsEnum {
    firstName = 'firstName',
    lastName = 'lastName',
    amount = 'amount',
    postCode = 'postCode'
}

@Component({
    selector: 'app-main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnDestroy, AfterViewInit {

    amount: number = 50;
    default_amount = 50;
    min_amount: number = 2;
    max_amount: number = 100000;

    max_slider: number = 1000;

    isSending: boolean = false;
    isCalculating: boolean = false;
    isCalculatingSlow: boolean = false;
    sent: boolean = false;

    donationResponse: ISendDonationDto;

    taxInfo$: Observable<TaxInfoModel>;
    _destroy$ = new Subject();

    donationForm: FormGroup;
    formFields = DonationFormFieldsEnum;

    constructor(@Inject(CURRENCY_CODE) public currency_name: string,
                private giftAidService: GiftAidService,
                private snackBar: MatSnackBar) {
        this.initForm();
    }

    send(): void {
        if (!this.donationForm.valid) {
            this.snackBar.open('Donation form is invalid, please check the input values');
            return;
        }

        this.isSending = true;
        this.giftAidService.sendDonation(this.donationForm.getRawValue()).pipe(
            tap(r => {
                this.sent = true;
                this.donationResponse = r;
            }),
            finalize(() => this.isSending = false),
            takeUntil(this._destroy$)
        ).subscribe();
    }

    reset(): void {
        this.donationForm.reset(undefined, {emitEvent: false});
        this.amount = this.default_amount;
        this.sent = false;
    }

    ngAfterViewInit(): void {
        this.taxInfo$ = this.donationForm.get(DonationFormFieldsEnum.amount).valueChanges.pipe(
            startWith(this.amount),
            distinctUntilChanged(),
            filter(s => this.donationForm.get(this.formFields.amount).valid),
            debounceTime(200),
            // Note: We do not want to show a loading progress bar if the request completes very fast
            // Note: If this finishes after the server response isCalculating will be false.
            tap(() => {
                this.isCalculating = true;
                this.isCalculatingSlow = false;
                setTimeout(() => {
                    if (this.isCalculating) {
                        this.isCalculatingSlow = true;
                    }
                }, 200);
            }),
            switchMap(amount => this.giftAidService.getGiftAid(amount)),
            map(response => new TaxInfoModel({amount: response.donationAmount, giftAid: response.giftAidAmount})),
            tap(() => {
                this.isCalculating = false;
                this.isCalculatingSlow = false;
            }),
            takeUntil(this._destroy$),
            shareReplay()
        );
    }

    checkControlValidity(controlName: DonationFormFieldsEnum): boolean {
        const control = this.donationForm.get(controlName);
        return control.dirty && control.invalid || control.touched && control.invalid;
    }

    ngOnDestroy(): void {
        this._destroy$.next();
        this._destroy$.complete();
    }

    private initForm() {
        this.donationForm = new FormGroup({
            [this.formFields.firstName]: new FormControl('', [Validators.required, Validators.pattern('[a-zA-Z]+')]),
            [this.formFields.lastName]: new FormControl('', [Validators.required, Validators.pattern('[a-zA-Z]+')]),
            [this.formFields.amount]: new FormControl(
                this.default_amount, [Validators.required, Validators.min(this.min_amount), Validators.max(this.max_amount)]
            ),
            [this.formFields.postCode]: new FormControl('', [Validators.required, Validators.pattern('[0-9]{1,}')])
        });
    }

}
