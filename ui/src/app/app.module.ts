import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import {GiftAidModule} from './gift-aid/gift-aid.module';
import {Router, RouterModule} from '@angular/router';
import {CURRENCY_CODE} from './core/tokens/currency.token';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    GiftAidModule,
    RouterModule.forRoot([
      {
        path: '',
        loadChildren: './gift-aid/gift-aid.module#GiftAidModule'
      }
    ]),
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: CURRENCY_CODE,
      useValue: 'GBP'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
