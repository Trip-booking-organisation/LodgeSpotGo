import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewFlightsComponent } from './view-flights/view-flights.component';
import { FlightsCardComponent } from './view-flights/flights-card/flights-card.component';
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {MatOptionModule} from "@angular/material/core";
import { FlightsComponent } from './view-flights/flights/flights.component';



@NgModule({
  declarations: [
    ViewFlightsComponent,
    FlightsCardComponent,
    FlightsComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatOptionModule,
  ],
  providers: [
    { provide: MAT_DIALOG_DATA, useValue: {} },
    { provide: MatDialogRef, useValue: {} }
  ]
})
export class FlightsModule { }
