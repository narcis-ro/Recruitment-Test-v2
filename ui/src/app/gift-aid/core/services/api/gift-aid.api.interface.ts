import {IGetGiftAidRequest, IGetGiftAidResponse, ISendDonationRequest, ISendDonationResponse} from '../models/request-response';
import {Observable} from 'rxjs';

export interface IGiftAidApiService {
    sendDonation(request: ISendDonationRequest): Observable<ISendDonationResponse>;

    getGiftAid(request: IGetGiftAidRequest): Observable<IGetGiftAidResponse>;
}
