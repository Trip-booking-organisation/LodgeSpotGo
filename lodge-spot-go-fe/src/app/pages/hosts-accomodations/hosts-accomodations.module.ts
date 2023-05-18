import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HostsAccomodationsComponent } from './hosts-accomodations/hosts-accomodations.component';
import {MatInputModule} from "@angular/material/input";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatLegacyButtonModule} from "@angular/material/legacy-button";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    HostsAccomodationsComponent
  ],
  imports: [
    CommonModule,
    MatInputModule,
    MatDatepickerModule,
    MatLegacyButtonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class HostsAccomodationsModule { }
