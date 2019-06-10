import {InjectionToken} from '@angular/core';
import {IGiftAidApiService} from '../services/api/gift-aid.api.interface';

export const GIFT_AID_API_SERVICE = new InjectionToken<IGiftAidApiService>('GIFT_AID_API_SERVICE');

