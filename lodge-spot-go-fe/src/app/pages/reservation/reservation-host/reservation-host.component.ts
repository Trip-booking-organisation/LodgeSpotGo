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
    this.accommodationClient.getAccommodationByHost(id).subscribe({
      next: response => {
        this.accommodations = response.accommodations
        console.log("accom: ",this.accommodations)
        const resAccomm: IReservationAccommodation[] = []
        this.accommodations.forEach(a => {
          this.reservationClient.getByAccommodationId(a.id!).subscribe({
            next: response => {
              console.log("res: ",response.reservations)
              if(response.reservations.length >0){
                this.reservations = response.reservations
                this.reservationsCount = [...this.reservations]
                this.reservations.forEach(r => {
                  const reservedAccommodation : IReservationAccommodation={
                    reservation: r,
                    accommodation: a,
                    deleted:  this.reservationsCount.filter(x=>x.guestId === r.guestId && x.deleted === true).length
                  }
                  resAccomm.push(reservedAccommodation!)
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
    console.log(this.reservedAccommodations)
    console.log(this.reservedAccommodations[0].reservation?.status)
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

  private getDeletedReservationsCount(guestId: string | undefined):number {
    let count :number = 0;
    this.reservationClient.getDeletedReservationsCount(guestId!).subscribe({
      next : response => {
      count = response.count
        return count
      }
    })
    return count
  }
}
