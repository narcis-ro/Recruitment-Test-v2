import {IGetGiftAid, IGetGiftAidDto, ISendDonationDto, SendDonationModel} from './gift-aid.models';

// tslint:disable-next-line:no-empty-interface
export interface ISendDonationRequest extends SendDonationModel {}

export interface ISendDonationResponse {
  donation: ISendDonationDto;
}

// tslint:disable-next-line:no-empty-interface
export interface IGetGiftAidRequest extends IGetGiftAid {
}

export interface IGetGiftAidResponse {
  giftAid: IGetGiftAidDto;
}
