import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReservationComponent } from './reservation-guest/reservation.component';
import { CancelReservationComponent } from './cancel-reservation/cancel-reservation.component';
import {MatDialog, MatDialogModule} from "@angular/material/dialog";
import { ReservationHostComponent } from './reservation-host/reservation-host.component';
import { DeletedReservationsDialogComponent } from './reservation-host/deleted-reservations-dialog/deleted-reservations-dialog.component';



@NgModule({
  declarations: [
    ReservationComponent,
    CancelReservationComponent,
    ReservationHostComponent,
    DeletedReservationsDialogComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule
  ]
})
export class ReservationModule { }
