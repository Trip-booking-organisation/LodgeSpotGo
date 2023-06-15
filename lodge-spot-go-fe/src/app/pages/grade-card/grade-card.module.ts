import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GradeCardComponent } from './grade-card/grade-card.component';



@NgModule({
  declarations: [
    GradeCardComponent
  ],
  exports: [
    GradeCardComponent
  ],
  imports: [
    CommonModule
  ]
})
export class GradeCardModule { }
