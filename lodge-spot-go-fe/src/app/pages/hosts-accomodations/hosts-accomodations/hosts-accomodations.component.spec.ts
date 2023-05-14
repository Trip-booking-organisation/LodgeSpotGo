import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostsAccomodationsComponent } from './hosts-accomodations.component';

describe('HostsAccomodationsComponent', () => {
  let component: HostsAccomodationsComponent;
  let fixture: ComponentFixture<HostsAccomodationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HostsAccomodationsComponent]
    });
    fixture = TestBed.createComponent(HostsAccomodationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
