import {IGetGiftAid, IGetGiftAidDto, ISendDonationDto, SendDonationModel} from './gift-aid.models';

export interface ISendDonationRequest extends SendDonationModel {}

export interface ISendDonationResponse {
  donation: ISendDonationDto;
}

export interface IGetGiftAidRequest extends IGetGiftAid {
}

export interface IGetGiftAidResponse {
  giftAid: IGetGiftAidDto;
}
