import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import {RouterModule} from '@angular/router';
import {NouisliderModule} from 'ng2-nouislider';
import {FlexLayoutModule} from '@angular/flex-layout';
import { PaymentFormComponent } from './components/payment-form/payment-form.component';
import { PersonalDetailsComponent } from './components/personal-details/personal-details.component';
import {
  MatButtonModule,
  MatChipsModule,
  MatFormFieldModule,
  MatInputModule, MatProgressBarModule,
  MatProgressSpinnerModule,
  MatSliderModule,
  MatStepperModule
} from '@angular/material';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {GiftAidApiService} from './services/gift-aid.api.service';
import {HttpClientModule} from '@angular/common/http';
import {GiftAidService} from './services/gift-aid.service';
import {GiftAidApiMockService} from './services/gift-aid.api.mock.service';

const MATERIAL_IMPORTS = [
  MatStepperModule,
  MatFormFieldModule,
  MatInputModule,
  MatSliderModule,
  MatButtonModule,
  MatChipsModule,
  MatProgressSpinnerModule,
  MatProgressBarModule
]

@NgModule({
  declarations: [MainComponent, PaymentFormComponent, PersonalDetailsComponent],
  imports: [
    CommonModule,
    NouisliderModule,
    FlexLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forChild([
      {
        path: '',
        component: MainComponent
      }
    ]),
    ...MATERIAL_IMPORTS
  ],
  providers: [
    GiftAidApiService,
    GiftAidApiMockService,
    GiftAidService
  ]
})
export class GiftAidModule { }
