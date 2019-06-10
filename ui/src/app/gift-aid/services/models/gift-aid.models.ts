export class SendDonationModel {
  firstName: string;
  lastName: string;
  amount: number;
  postCode: number;
  constructor(content: SendDonationModel) {}
}

export interface ISendDonationDto {
  donationId: number;
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
