import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewHostComponent } from './view-host/view-host.component';
import {GradeHostCardModule} from "../grade-host-card/grade-host-card.module";


@NgModule({
  declarations: [
    ViewHostComponent,

  ],
  imports: [
    CommonModule,
    GradeHostCardModule,
  ],
  exports:[
  ]
})
export class ViewHostModule { }
