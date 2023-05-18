import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ReservationService} from "../../../../common/services/reservation.service";

@Component({
  selector: 'app-deleted-reservations-dialog',
  templateUrl: './deleted-reservations-dialog.component.html',
  styleUrls: ['./deleted-reservations-dialog.component.scss']
})
export class DeletedReservationsDialogComponent implements OnInit {
  guestId!: string;
  count: number;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<DeletedReservationsDialogComponent>,
              private reservationClient: ReservationService) {
    this.guestId = data.guestId;
  }

  ngOnInit(): void {
    this.reservationClient.getDeletedReservationsCount(this.guestId).subscribe({
      next: (count) => {
        this.count = count.count;
      }
    })
  }

}
