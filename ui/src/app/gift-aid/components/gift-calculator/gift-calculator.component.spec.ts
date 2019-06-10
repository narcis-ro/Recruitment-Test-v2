import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftCalculatorComponent } from './gift-calculator.component';

describe('GiftCalculatorComponent', () => {
  let component: GiftCalculatorComponent;
  let fixture: ComponentFixture<GiftCalculatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftCalculatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
