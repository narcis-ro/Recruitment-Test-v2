import {Injectable} from '@angular/core';
import {IGetGiftAidRequest, IGetGiftAidResponse, ISendDonationRequest, ISendDonationResponse} from '../models/request-response';
import {Observable} from 'rxjs';
import {delay} from 'rxjs/operators';
import {of} from 'rxjs/internal/observable/of';
import {IGiftAidApiService} from './gift-aid.api.interface';

@Injectable()
export class GiftAidApiMockService implements IGiftAidApiService {

    taxRate: number = 25;

    constructor() {
    }

    sendDonation(request: ISendDonationRequest): Observable<ISendDonationResponse> {
        return of(
            {
                donation: {
                    donationAmount: request.amount,
                    donationId: Math.random(),
                    donationReference: Math.random(),
                    giftAidAmount: request.amount * (this.taxRate / (100 - this.taxRate)),
                    giftAidReference: Math.random().toString()
                }
            }
        ).pipe(delay(Math.random() * 5000));
    }

    getGiftAid(request: IGetGiftAidRequest): Observable<IGetGiftAidResponse> {
        return of({
            giftAid: {
                donationAmount: request.amount,
                giftAidAmount: request.amount * (this.taxRate / (100 - this.taxRate)),
            }
        }).pipe(delay(Math.random() * 5000));
    }
}
