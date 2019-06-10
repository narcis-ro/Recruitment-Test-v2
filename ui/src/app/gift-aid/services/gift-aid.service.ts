import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {IGetGiftAidDto, ISendDonationDto, SendDonationModel} from './models/gift-aid.models';
import {map} from 'rxjs/operators';
import {GiftAidApiMockService} from './gift-aid.api.mock.service';

@Injectable()
export class GiftAidService {

  constructor(private api: GiftAidApiMockService) {
  }

  public sendDonation(request: SendDonationModel): Observable<ISendDonationDto> {
    return this.api.sendDonation(request).pipe(map(r => r.donation));
  }

  public getGiftAid(amount: number): Observable<IGetGiftAidDto> {
    return this.api.getGiftAid({amount}).pipe(map(r => r.giftAid));
  }
}
