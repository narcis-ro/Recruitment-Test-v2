import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MainComponent} from './main/main.component';
import {RouterModule} from '@angular/router';
import {FlexLayoutModule} from '@angular/flex-layout';
import {
    MatButtonModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatSliderModule, MatSnackBarModule,
    MatStepperModule
} from '@angular/material';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {GiftAidService} from './core/services/gift-aid.service';
import {GiftAidApiMockService} from './core/services/api/gift-aid.api.mock.service';
import {CURRENCY_CODE} from './core/tokens/currency.token';
import {GIFT_AID_API_SERVICE} from './core/tokens/api.token';
import {environment} from '../../environments/environment';
import {GiftAidApiService} from './core/services/api/gift-aid.api.service';

const MATERIAL_IMPORTS = [
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatSliderModule,
    MatButtonModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatSnackBarModule
];

@NgModule({
    declarations: [MainComponent],
    imports: [
        CommonModule,
        FlexLayoutModule,
        FormsModule,
        ReactiveFormsModule.withConfig({warnOnNgModelWithFormControl: 'never'}),
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
        GiftAidService,
        {
            provide: GIFT_AID_API_SERVICE,
            useFactory: (http) => environment.production ? new GiftAidApiService(http) : new GiftAidApiMockService(),
            deps: [HttpClient]
        },
        {
            provide: CURRENCY_CODE,
            useValue: 'GBP'
        }
    ]
})
export class GiftAidModule {
}
