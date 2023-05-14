import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {ReservationService} from "../../../common/services/reservation.service";
import {IReservation} from "../../../common/model/reservations";
import {DataService} from "../../../common/services/data.service";

@Component({
  selector: 'app-cancel-reservation-guest',
  templateUrl: './cancel-reservation.component.html',
  styleUrls: ['./cancel-reservation.component.scss']
})
export class CancelReservationComponent {
  reservation! : IReservation
constructor(@Inject(MAT_DIALOG_DATA) public data: any,
            private dialogRef: MatDialogRef<CancelReservationComponent>,
            private reservationClient: ReservationService,
            private dataService : DataService) {
  this.reservation = this.data.reservation
}

  onYes() {
    this.dialogRef.close()
    this.reservationClient.cancelReservation(this.reservation.id!).subscribe({
      next: response =>{
        this.dataService.sendData(this.reservation.id)
      }
    })
  }

  onNo() {
    this.dialogRef.close()
  }
}
