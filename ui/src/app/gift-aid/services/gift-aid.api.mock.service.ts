import {Injectable} from '@angular/core';
import {IGetGiftAidRequest, IGetGiftAidResponse, ISendDonationRequest, ISendDonationResponse} from './models/request-response';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {IGetGiftAidDto, ISendDonationDto} from './models/gift-aid.models';
import {delay, map} from 'rxjs/operators';
import {of} from 'rxjs/internal/observable/of';

const endpoints = {
  SendDonations: 'api/donations',
  GiftAid: 'api/giftaid'
};

@Injectable()
export class GiftAidApiMockService {

  apiUrl: string = 'test.com';
  taxRate: number = 25;

  constructor(private http: HttpClient) {
  }

  sendDonation(request: ISendDonationRequest): Observable<ISendDonationResponse> {
    return of(
      {
        donation: {
          donationAmount: request.amount,
          donationId: Math.random(),
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
