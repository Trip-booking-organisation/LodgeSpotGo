import {Component, Inject, OnInit} from '@angular/core';
import {DataService} from "../../../common/services/data.service";
import {Flight} from "../../../common/model/flight";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {ReservationService} from "../../../common/services/reservation.service";
import {flightsAutoComplete} from "../../../search-accomodations-component/data-access/cityAndCountryData";
import {map, Observable, startWith} from "rxjs";
import {Address} from "../../../core/model/Address";
import {FormControl} from "@angular/forms";
import {AddressFlight} from "../../../common/model/AddressFlight";
import {FlightsComponent} from "./flights/flights.component";
import {IReservation} from "../../../common/model/reservations";
import {AccommodationService} from "../../../common/services/accommodationService";
import {IAccommodationDto} from "../../../common/model/accommodation-dto";
import {AuthService} from "../../../core/keycloak/auth.service";
import {SignInRequest} from "../../../common/model/jets-set-go/sign-in-request";
import {JetSetGoService} from "../../../common/services/jet-set-go.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-view-flights',
  templateUrl: './view-flights.component.html',
  styleUrls: ['./view-flights.component.scss']
})
export class ViewFlightsComponent implements OnInit{
  flightsAddresses = flightsAutoComplete;
  filteredFlightsTo!: Observable<Address[]>;
  filteredFlightsFrom!: Observable<Address[]>;
  flightControlFrom = new FormControl();
  flightControlTo = new FormControl();
  flights : Flight[]
  reservation : IReservation
  accommodation : IAccommodationDto
  token : string | undefined
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<ViewFlightsComponent>,
              private reservationClient: ReservationService,
              private dataService : DataService,
              private dialog: MatDialog,
              private authService: AuthService,
              private jetSetGo : JetSetGoService,
              private accommodationClient: AccommodationService) {
    console.log(this.data.reservation)
    this.reservation = this.data.reservation
    this.flights = this.data.flights
    console.log(this.data.flights)
  }
  ngOnInit(): void {
    let user = this.authService.getUser();
    let request  : SignInRequest={
      email: user.email,
      password : 'a'
    }
    this.jetSetGo.signInUser(request).subscribe({
      next : response => {
        this.token = response.token
        console.log('token',this.token)
      }
    })
    this.accommodationClient.getAccommodationById(this.reservation.accommodationId).subscribe({
      next: response => {
        this.accommodation = response.accommodation
        console.log(this.accommodation)
      }
    })
    this.mapFilterFrom()
    this.mapFilterTo()
  }

  private mapFilterTo() {
    this.filteredFlightsTo = this.flightControlTo.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filterFlights(value))
      )
  }

  private mapFilterFrom() {
    this.filteredFlightsFrom = this.flightControlFrom.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filterFlights(value))
      )
  }
  private _filterFlights(value: string): any[] {
    const filterValue = value.toLowerCase();
    return this.flightsAddresses.filter(flight => {
      const city = flight.city.toLowerCase();
      const country = flight.country.toLowerCase();
      return city.includes(filterValue) || country.includes(filterValue);
    });
  }

  onSubmit() {
    this.dialogRef.close()
    console.log(this.accommodation)
    const locationFrom = this.flightControlFrom.value.split(",")
    const cityFrom = locationFrom[0]
    const countryFrom = locationFrom[1]
    const locationTo = this.flightControlTo.value.split(",")
    const cityTo = locationTo[0]
    const countryTo = locationTo[1]
    let filterFromFlights = this.flights.filter(x =>{
      const arrival = new Date(x.arrivalDate)
      const from = new Date(this.reservation.dateRange.from)
      arrival.setHours(0)
      arrival.setMinutes(0)
      from.setHours(0)
      from.setMinutes(0)
      console.log(arrival)
      console.log(from)
      console.log(arrival.getTime() === from.getTime())
      return arrival.getTime() === from.getTime() &&
      x.departureAddress.city == cityFrom &&
      x.departureAddress.country == countryFrom &&
      x.arrivalAddress.country == this.accommodation.address.country &&
      x.arrivalAddress.city == this.accommodation.address.city &&
      x.seats.filter(s => s.available).length >= this.reservation.numberOfGuest
    }
 );
    let filterFromTo = this.flights.filter(x =>{
      const to = new Date(this.reservation.dateRange.to)
      const departure = new Date(x.departureDate)
      departure.setHours(0)
      departure.setMinutes(0)
      to.setHours(0)
      to.setMinutes(0)
      return departure.getTime() === to.getTime() &&
        x.arrivalAddress.city == cityTo &&
        x.arrivalAddress.country == countryTo&&
        x.departureAddress.country == this.accommodation.address.country &&
        x.departureAddress.city == this.accommodation.address.city&&
        x.seats.filter(s => s.available).length >= this.reservation.numberOfGuest

    }
      );
    this.dialog.open(FlightsComponent,{
      width: '2000px',
      height:'700pxx',
      data: { reservation: this.reservation,
              flightsFrom: filterFromFlights,
              flightsTo: filterFromTo,
              token :this.token}});
  }

  onCancel() {
      this.dialogRef.close()
  }
}
