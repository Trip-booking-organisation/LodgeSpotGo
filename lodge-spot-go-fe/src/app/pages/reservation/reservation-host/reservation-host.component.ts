import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {User} from "../../../core/keycloak/user";
import {IReservation} from "../../../common/model/reservations";
import {IReservationAccommodation} from "../../../common/model/reservedAccommodation";
import {Router} from "@angular/router";
import {AuthService} from "../../../core/keycloak/auth.service";
import {AccommodationService} from "../../../common/services/accommodationService";
import {ReservationService} from "../../../common/services/reservation.service";
import {MatDialog} from "@angular/material/dialog";
import {DataService} from "../../../common/services/data.service";
import {ReservationStatus} from "../../../common/model/reservationStatus";
import {CancelReservationComponent} from "../cancel-reservation/cancel-reservation.component";
import {Accommodation} from "../../../common/model/accommodation";
import {IAccommodationDto} from "../../../common/model/accommodation-dto";
import {UpdateReservationStatusRequest} from "../../../common/model/UpdateReservationStatusRequest";
import {DeletedReservationsDialogComponent} from "./deleted-reservations-dialog/deleted-reservations-dialog.component";

@Component({
  selector: 'app-reservation-guest-host',
  templateUrl: './reservation-host.component.html',
  styleUrls: ['./reservation-host.component.scss']
})
export class ReservationHostComponent implements OnInit{
  user$: Observable<User | null> = this.authService.getUserObservable();
  accommodations! : IAccommodationDto[];
  reservedAccommodations: IReservationAccommodation[]=[];
  reservations! : IReservation[];
  reservationsCount! : IReservation[];
  count! : number

  constructor(private router: Router,
              private authService:AuthService,
              private accommodationClient: AccommodationService,
              private reservationClient : ReservationService,
              private dialog : MatDialog,
              private dataService: DataService) {
    this.dataService.getData().subscribe(data => {
      // @ts-ignore
      let res = this.reservedAccommodations.filter(e => {
        if(e.reservation?.id != data){
          return true
        }
      });
      this.reservedAccommodations =  [...res];
    })
  }

  ngOnInit(): void {
    this.getReservationsByGuest(this.authService.getUser()?.id!);
  }

  private getReservationsByGuest(id : string) {
    this.accommodationClient.getAccommodationByHost(id).subscribe({
      next: response => {
        this.accommodations = response.accommodations
        const resAccomm: IReservationAccommodation[] = []
        this.accommodations.forEach(a => {
          this.reservationClient.getByAccommodationId(a.id!).subscribe({
            next: response => {
              if(response.reservations.length >0){
                this.reservations = response.reservations
                this.reservationsCount = [...this.reservations]
                this.reservations.forEach(r => {
                  const reservedAccommodation : IReservationAccommodation={
                    reservation: r,
                    accommodation: a,
                    deleted:  response.count
                  }
                  resAccomm.push(reservedAccommodation!)
                  this.calculatePrice(reservedAccommodation)
                })
                if(this.reservations.length === resAccomm.length){
                  this.assignReservedAccommodations(resAccomm)
                }
              }
            }
          })
        })
      }
    })

  }

  assignReservedAccommodations(resAccomm: IReservationAccommodation[]){
    this.reservedAccommodations = [...resAccomm]
  }
  onConfirm(reservation: IReservation | undefined) {
    const updateReservationStatus :  UpdateReservationStatusRequest ={
      status : 'Confirmed',
      id: reservation?.id
    }
    this.reservationClient.updateReservationStatus(updateReservationStatus).subscribe({
      next: response => {
        console.log(response)
        const filtered = this.reservedAccommodations.filter(r => r.reservation?.id != reservation?.id)
        this.reservedAccommodations = [...filtered]
      }
    })
  }

  private IsDisabled(r: IReservation) {
    if(r.status != "Confirmed")
      return true;
    if(this.calculateDiff(r.dateRange?.from!) <=1)
      return true
    return false;
  }
  calculateDiff(dateSent:Date){
    let currentDate = new Date();
    dateSent = new Date(dateSent);
    console.log(dateSent)
    return Math.floor(( Date.UTC(dateSent.getFullYear(), dateSent.getMonth(), dateSent.getDate()) - Date.UTC(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()) ) /(1000 * 60 * 60 * 24));
  }

  onRefuse(reservation: IReservation | undefined) {
    const updateReservationStatus :  UpdateReservationStatusRequest ={
      status : 'Refused',
      id: reservation?.id
    }
    this.reservationClient.updateReservationStatus(updateReservationStatus).subscribe({
      next: response => {
        console.log(response)
        const filtered = this.reservedAccommodations.filter(r => r.reservation?.id != reservation?.id)
        this.reservedAccommodations = [...filtered]
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
      if(new Date(a.dateRange.from).getTime() <= new Date(start).getTime() &&  new Date(end).getTime() <= new Date(a.dateRange.to).getTime())
      {
        reservedAccommodation.pricePerPersonOneNight = a.price
        reservedAccommodation.priceInTotalOneNight = (a.price * reservedAccommodation.reservation.numberOfGuest)
        let differenceMs = new Date(end).getTime() - new Date(start).getTime();
        let daysDiff = Math.floor(differenceMs / (1000 * 60 * 60 * 24))
        let priceTotal = (reservedAccommodation.reservation.numberOfGuest * a.price) * daysDiff
        reservedAccommodation.priceInTotalInTotal = priceTotal;
        reservedAccommodation.pricePerPersonInTotal = (daysDiff * a.price)
      }
    })
  }
  onSeeDeleted(guestId: string) {
    this.dialog.open(DeletedReservationsDialogComponent, {
      width: '400px',
      height:'220px',
      data: { guestId: guestId }
    });
  }
}
