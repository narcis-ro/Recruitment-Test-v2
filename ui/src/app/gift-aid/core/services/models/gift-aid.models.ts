export class SendDonationModel {
  firstName: string;
  lastName: string;
  amount: number;
  postCode: number;
  constructor(content: SendDonationModel) {
      Object.assign(this, content);
  }
}

export interface ISendDonationDto {
  donationId: number;
  donationReference: number;
  giftAidReference: string;
  giftAidAmount: number;
  donationAmount: number;
}

export interface IGetGiftAid {
  amount: number;
}

export interface IGetGiftAidDto {
  giftAidAmount: number;
  donationAmount: number;
}
