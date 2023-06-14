import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GradeHostCardComponent } from './grade-host-card.component';

describe('GradeHostCardComponent', () => {
  let component: GradeHostCardComponent;
  let fixture: ComponentFixture<GradeHostCardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GradeHostCardComponent]
    });
    fixture = TestBed.createComponent(GradeHostCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
