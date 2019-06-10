import {AfterViewInit, ChangeDetectorRef, Component, Inject, OnDestroy} from '@angular/core';
import {CURRENCY_CODE} from '../../core/tokens/currency.token';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {GiftAidService} from '../services/gift-aid.service';
import {debounceTime, finalize, map, shareReplay, startWith, takeUntil, throttleTime} from 'rxjs/operators';
import {Observable, Subject} from 'rxjs';
import {concatMap} from 'rxjs/internal/operators/concatMap';
import {tap} from 'rxjs/internal/operators/tap';
import {ISendDonationDto} from '../services/models/gift-aid.models';

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
  sent: boolean;

  donationResponse: ISendDonationDto;

  taxInfo$: Observable<TaxInfoModel>;
  _destroy$ = new Subject();

  donationForm: FormGroup;

  constructor(@Inject(CURRENCY_CODE) public currency_name: string, private giftAidService: GiftAidService) {
    this.initForm();
  }

  send(): void {
    if (!this.donationForm.valid) {
      alert('ERROR');
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

  private initForm() {
    this.donationForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      amount: new FormControl(this.default_amount, [Validators.required, Validators.min(this.min_amount), Validators.max(this.max_amount)]),
      postCode: new FormControl('', Validators.required)
    });
  }

  ngAfterViewInit(): void {
    this.taxInfo$ = this.donationForm.get('amount').valueChanges.pipe(
      startWith(this.amount),
      debounceTime(500),
      tap(() => this.isCalculating = true),
      concatMap(amount => this.giftAidService.getGiftAid(amount)),
      map(response => new TaxInfoModel({amount: response.donationAmount, giftAid: response.giftAidAmount})),
      tap(() => this.isCalculating = false),
      takeUntil(this._destroy$),
      shareReplay()
    );
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

}
