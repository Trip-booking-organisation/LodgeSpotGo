import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {User} from "../../../core/keycloak/user";
import {Router} from "@angular/router";
import {AuthService} from "../../../core/keycloak/auth.service";
import {ReservationService} from "../../../common/services/reservation.service";
import {IReservation} from "../../../common/model/reservations";
import {AccommodationService} from "../../../common/services/accommodationService";
import {IReservationAccommodation} from "../../../common/model/reservedAccommodation";
import {MatDialog} from "@angular/material/dialog";
import {CancelReservationComponent} from "../cancel-reservation/cancel-reservation.component";
import {DataService} from "../../../common/services/data.service";
import {ReservationStatus} from "../../../common/model/reservationStatus";


@Component({
  selector: 'app-reservation-guest',
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.scss']
})
export class ReservationComponent implements OnInit{
  user$: Observable<User | null> = this.authService.getUserObservable();
  reservations! : IReservation[];
  reservedAccommodations: IReservationAccommodation[]=[];

  constructor(private router: Router,
              private authService:AuthService,
              private accommodationClient: AccommodationService,
              private reservationClient : ReservationService,
              private dialog : MatDialog,
              private dataService: DataService) {
    this.authService.getUserObservable().subscribe(
      value => {
        console.log(value)
        this.getReservationsByGuest(value?.id!);
      })
    this.dataService.getData().subscribe(data => {
      // @ts-ignore
      let res = this.reservedAccommodations.filter(e => {
        //@ts-ignore
        if(e.reservation.id != data){
          return true
        }
      });
      this.reservedAccommodations =  [...res];
    })
  }

  ngOnInit(): void {
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
              if(this.reservations.length === resAccomm.length){
                this.assignReservedAccommodations(resAccomm)
              }
            }
          })
        })
      }
    })

  }
  convertEnumToString(status : ReservationStatus):string{
    if(status === ReservationStatus.Confirmed)
      return 'Confirmed'
    if(status === ReservationStatus.Waiting)
      return 'Confirmed'
    else return "Refused"

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
}
