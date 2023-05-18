import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewAccommodationComponent } from './view-accommodation/view-accommodation.component';
import {MatCardModule} from "@angular/material/card";
import { CreateReservationComponent } from './create-reservation/create-reservation.component';
import {MatDialogModule} from "@angular/material/dialog";
import {ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatButtonModule} from "@angular/material/button";


@NgModule({
  declarations: [
    ViewAccommodationComponent,
    CreateReservationComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDatepickerModule,
    MatButtonModule,
  ]
})
export class ViewAccommodationModule { }
