import {Inject, Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {IGetGiftAidDto, ISendDonationDto, SendDonationModel} from './models/gift-aid.models';
import {map} from 'rxjs/operators';
import {GIFT_AID_API_SERVICE} from '../tokens/api.token';
import {IGiftAidApiService} from './api/gift-aid.api.interface';

@Injectable()
export class GiftAidService {

    constructor(@Inject(GIFT_AID_API_SERVICE) private api: IGiftAidApiService) {
    }

    public sendDonation(request: SendDonationModel): Observable<ISendDonationDto> {
        return this.api.sendDonation(request).pipe(map(r => r.donation));
    }

    public getGiftAid(amount: number): Observable<IGetGiftAidDto> {
        return this.api.getGiftAid({amount}).pipe(map(r => r.giftAid));
    }
}
