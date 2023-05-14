import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReserveAccomodationCardComponent } from './reserve-accomodation-card.component';

describe('ReserveAccomodationCardComponent', () => {
  let component: ReserveAccomodationCardComponent;
  let fixture: ComponentFixture<ReserveAccomodationCardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReserveAccomodationCardComponent]
    });
    fixture = TestBed.createComponent(ReserveAccomodationCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
