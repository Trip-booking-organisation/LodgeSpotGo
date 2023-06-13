import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GradeAccommodationDialogComponent } from './grade-accommodation-dialog/grade-accommodation-dialog.component';
import {MatDialogModule} from "@angular/material/dialog";
import {ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatCardModule} from "@angular/material/card";



@NgModule({
  declarations: [
    GradeAccommodationDialogComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatInputModule,
    MatCardModule
  ]
})
export class GradeAccommodationModule { }
