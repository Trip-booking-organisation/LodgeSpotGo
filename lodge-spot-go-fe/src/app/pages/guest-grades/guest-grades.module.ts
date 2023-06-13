import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GuestGradesViewComponent } from './guest-grades-view/guest-grades-view.component';
import { AccommodationGradeCardComponent } from './guest-grades-view/accommodation-grade-card/accommodation-grade-card.component';
import { EditAccommodationGradeComponent } from './guest-grades-view/edit-accommodation-grade/edit-accommodation-grade.component';
import {MatDialogModule} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    GuestGradesViewComponent,
    AccommodationGradeCardComponent,
    EditAccommodationGradeComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ]
})
export class GuestGradesModule { }
