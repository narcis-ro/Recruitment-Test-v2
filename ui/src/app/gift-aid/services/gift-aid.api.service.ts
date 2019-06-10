import {Injectable} from '@angular/core';
import {IGetGiftAidRequest, IGetGiftAidResponse, ISendDonationRequest, ISendDonationResponse} from './models/request-response';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {IGetGiftAidDto, ISendDonationDto} from './models/gift-aid.models';
import {map} from 'rxjs/operators';

const endpoints = {
  SendDonations: 'api/donations',
  GiftAid: 'api/giftaid'
};

@Injectable()
export class GiftAidApiService {

  apiUrl: string = 'test.com';

  constructor(private http: HttpClient) {
  }

  sendDonation(request: ISendDonationRequest): Observable<ISendDonationResponse> {
    return this.http.post<ISendDonationDto>(`${this.apiUrl}/${endpoints.SendDonations}`, request).pipe(map(r => ({donation: r})));
  }

  getGiftAid(request: IGetGiftAidRequest): Observable<IGetGiftAidResponse> {
    return this.http.post<IGetGiftAidDto>(`${this.apiUrl}/${endpoints.SendDonations}`, request).pipe(map(r => ({giftAid: r})));
  }
}
