import { Component } from '@angular/core';
import {ReservationCreate} from "../../../common/model/reservation.create";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MatDialogRef} from "@angular/material/dialog";
import {AccommodationCurrentService} from "../../../common/services/accommodation-current.service";
import {IAccommodationDto} from "../../../common/model/accommodation-dto";
import {User} from "../../../core/keycloak/user";
import {AuthService} from "../../../core/keycloak/auth.service";
import {toIsoString} from "../../../common/utility/date.converter";
import {ReservationService} from "../../../common/services/reservation.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.scss']
})
export class CreateReservationComponent {
  reservationForm: FormGroup;
  accommodation: IAccommodationDto;
  user : User | null;

  constructor(
    private dialogRef: MatDialogRef<CreateReservationComponent>,
    private formBuilder: FormBuilder, private accommodationCurrentService:AccommodationCurrentService,
    private auth:AuthService,private reservationService:ReservationService
    ,private toast: ToastrService
  ) {
    this.reservationForm = this.formBuilder.group({
      from: ['', Validators.required],
      to: ['', Validators.required],
      numberOfGuests: ['', [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.user = this.auth.getUser()
    this.accommodation = this.accommodationCurrentService.accommodation
  }

  submitReservation(): void {
    if (this.reservationForm.valid) {
      if(this.accommodation.maxGuests < this.reservationForm.value.numberOfGuests){
        this.toast.error(`Max guest limit is ${this.accommodation.maxGuests}`,"Error")
        return
      }
      if(this.accommodation.minGuests > this.reservationForm.value.numberOfGuests){
        this.toast.error(`Min guest limit is ${this.accommodation.minGuests}`,"Error")
        return;
      }
      const reservation: ReservationCreate = {
        accommodationId: this.accommodation.id,
        dateRange: {
          from: toIsoString(this.reservationForm.value.from),
          to: toIsoString(this.reservationForm.value.to),
        },
        status: 'Waiting',
        numberOfGuests: this.reservationForm.value.numberOfGuests,
        guestId: this.user.id,
        guestEmail: this.user.email
      };
      console.log(reservation)
      this.reservationService.createReservation(reservation).subscribe({
        next: value => {
          console.log(value)
          this.toast.success("You are successfully created reservation","Success")
          window.location.reload()
        },
        error: err => {
          console.log(err)
          this.toast.error("You cannot create reservation for this accommodation","Failure")
        }
      });
      this.dialogRef.close();
    }
  }

  closeModal(): void {
    this.dialogRef.close();
  }
}
