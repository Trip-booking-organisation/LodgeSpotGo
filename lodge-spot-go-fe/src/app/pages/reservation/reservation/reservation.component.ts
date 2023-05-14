import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {User} from "../../../core/keycloak/user";
import {Router} from "@angular/router";
import {AuthService} from "../../../core/keycloak/auth.service";
import {ReservationService} from "../../../common/services/reservation.service";

@Component({
  selector: 'app-reservation',
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.scss']
})
export class ReservationComponent implements OnInit{
  user$: Observable<User | null> = this.authService.getUserObservable();

  constructor(private router: Router,
              private authService:AuthService,
              private reservationClient : ReservationService) {
    this.authService.getUserObservable().subscribe(
      value => {
        console.log(value)
        this.getReservationsByGuest(value?.id!);
      })
  }

  ngOnInit(): void {
    console.log("aaaaa")

  }

  private getReservationsByGuest(id : string) {
    this.reservationClient.getByGuestId(id).subscribe({
      next: response => {
        console.log(response)
      }
    })
  }
}
