import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {AuthService} from "../../../core/keycloak/auth.service";
import {ReservationService} from "../../../common/services/reservation.service";
import {IReservation} from "../../../common/model/reservations";
import {AccommodationService} from "../../../common/services/accommodationService";
import {IReservationAccommodation} from "../../../common/model/reservedAccommodation";
import {MatDialog} from "@angular/material/dialog";
import {CancelReservationComponent} from "../cancel-reservation/cancel-reservation.component";
import {DataService} from "../../../common/services/data.service";


@Component({
  selector: 'app-reservation-guest',
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.scss']
})
export class ReservationComponent implements OnInit{
  reservations! : IReservation[];
  reservedAccommodations: IReservationAccommodation[]=[];

  constructor(private router: Router,
              private authService:AuthService,
              private accommodationClient: AccommodationService,
              private reservationClient : ReservationService,
              private dialog : MatDialog,
              private dataService: DataService) {
  }

  ngOnInit(): void {
    this.dataService.getData().subscribe(data => {

      let res = this.reservedAccommodations.filter(e => {
        return e.reservation.id != data;
      });
      this.reservedAccommodations =  [...res];
    })
    this.getReservationsByGuest(this.authService.getUser()!.id!);
  }

  private getReservationsByGuest(id : string) {
    this.reservationClient.getByGuestId(id).subscribe({
      next: response => {
        this.reservations = response.reservations
        console.log(this.reservations)
        console.log(" res : ",this.reservations[0].status)
        const resAccomm: IReservationAccommodation[] = []
        this.reservations.forEach(r => {
          this.accommodationClient.getAccommodationById(r.accommodationId!).subscribe({
            next: response => {
              const reservedAccommodation : IReservationAccommodation={
                reservation: r,
                accommodation: response.accommodation,
                disabled: this.IsDisabled(r)
              }
              resAccomm.push(reservedAccommodation!)
              this.calculatePrice(reservedAccommodation)
              if(this.reservations.length === resAccomm.length){
                this.assignReservedAccommodations(resAccomm)
              }
            }
          })
        })
      }
    })

  }
  private calculatePrice(reservedAccommodation: IReservationAccommodation) {
    reservedAccommodation.pricePerPersonOneNight = 0
    reservedAccommodation.priceInTotalOneNight = 0
    reservedAccommodation.priceInTotalInTotal = 0
    reservedAccommodation.pricePerPersonInTotal = 0
    reservedAccommodation.accommodation.specialPrices.forEach( a => {
      let start= new Date(reservedAccommodation.reservation.dateRange.from)
      let end =  new Date( reservedAccommodation.reservation.dateRange.to)
      console.log("rez", start)
      console.log("cena", a.dateRange.from)
      console.log("rez", end)
      console.log("cena", a.dateRange.to)

      console.log(new Date(a.dateRange.from).getTime() <= new Date(start).getTime())
      console.log(new Date(end).getTime() <= new Date(a.dateRange.to).getTime())
      if(new Date(a.dateRange.from).getTime() <= new Date(start).getTime() &&  new Date(end).getTime() <= new Date(a.dateRange.to).getTime())
      {
        console.log("aaaa")
        reservedAccommodation.pricePerPersonOneNight = a.price
        reservedAccommodation.priceInTotalOneNight = (a.price * reservedAccommodation.reservation.numberOfGuest)
        let differenceMs = new Date(end).getTime() - new Date(start).getTime();
        let daysDiff = Math.floor(differenceMs / (1000 * 60 * 60 * 24))
        console.log(daysDiff)
        let priceTotal = (reservedAccommodation.reservation.numberOfGuest * a.price) * daysDiff

        reservedAccommodation.priceInTotalInTotal = priceTotal;
        reservedAccommodation.pricePerPersonInTotal = (daysDiff * a.price)
        console.log(reservedAccommodation.pricePerPersonInTotal)
      }
    })
  }
  assignReservedAccommodations(resAccomm: IReservationAccommodation[]){
    this.reservedAccommodations = [...resAccomm]
    console.log(this.reservedAccommodations)
    console.log(this.reservedAccommodations[0].reservation?.status)
  }

  onCancel(reservation: IReservation | undefined) {
    this.dialog.open(CancelReservationComponent, {
      width: '400px',
      height:'220px',
      data: { reservation: reservation }
    });
  }

  private IsDisabled(r: IReservation) {
    if(r.status != "Confirmed")
      return true;
    return this.calculateDiff(r.dateRange?.from!) <= 1;

  }
  calculateDiff(dateSent:Date){
    let currentDate = new Date();
    dateSent = new Date(dateSent);
    console.log(dateSent)
    return Math.floor((
      Date.UTC(dateSent.getFullYear(),
      dateSent.getMonth(),
        dateSent.getDate()) - Date.UTC(currentDate.getFullYear(),
        currentDate.getMonth(),
        currentDate.getDate()) ) /(1000 * 60 * 60 * 24)
    );
  }

  onDelete(reservation: IReservation ) {
    console.log(reservation)
    this.reservationClient.deleteReservation(reservation.id!).subscribe({
      next: _ =>{
        const filter = this.reservedAccommodations.filter(r => r.reservation?.id !== reservation.id)
        this.reservedAccommodations = [...filter]
      }
    })
  }
}
