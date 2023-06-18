import {Component, Inject, OnInit} from '@angular/core';
import {Flight} from "../../../../common/model/flight";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {ReservationService} from "../../../../common/services/reservation.service";
import {DataService} from "../../../../common/services/data.service";
import {AccommodationService} from "../../../../common/services/accommodationService";
import {IReservation} from "../../../../common/model/reservations";
import {JetSetGoService} from "../../../../common/services/jet-set-go.service";
import {SignInRequest} from "../../../../common/model/jets-set-go/sign-in-request";
import {AuthService} from "../../../../core/keycloak/auth.service";

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.scss']
})
export class FlightsComponent implements OnInit{
  flightsTo : Flight[]
  flightsFrom : Flight[]
  reservation : IReservation
  token : string | undefined
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<FlightsComponent>,
              private reservationClient: ReservationService,
              private dataService : DataService,
              private dialog: MatDialog,
              private authService:AuthService,
              private accommodationClient: AccommodationService,
              private jetSetGo : JetSetGoService) {
    this.flightsFrom = this.data.flightsFrom
    this.flightsTo = this.data.flightsTo
    this.reservation = this.data.reservation
    this.token = this.data.token
    console.log(this.token)
  }
  ngOnInit(): void {

  }

}
