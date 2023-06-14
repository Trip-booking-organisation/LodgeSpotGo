import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GradeHostCardComponent } from './grade-host-card/grade-host-card.component';



@NgModule({
  declarations: [
    GradeHostCardComponent
  ],
  imports: [
    CommonModule
  ],
  exports:[
    GradeHostCardComponent
  ]
})
export class GradeHostCardModule { }
