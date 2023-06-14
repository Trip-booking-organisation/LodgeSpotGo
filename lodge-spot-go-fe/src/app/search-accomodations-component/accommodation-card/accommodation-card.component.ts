import {Component, Input, OnInit} from '@angular/core';
import {Accommodation} from "../../common/model/accommodation";
import {AccomodationResult} from "../../core/model/AccomodationResult";
import {IAccommodationDto} from "../../common/model/accommodation-dto";
import {AuthService} from "../../core/keycloak/auth.service";
import {User} from "../../core/keycloak/user";
import {toIsoString} from "../../common/utility/date.converter";
import {ReservationCreate} from "../../common/model/reservation.create";
import {ReservationService} from "../../common/services/reservation.service";
import {ToastrService} from "ngx-toastr";
import {AccommodationCurrentService} from "../../common/services/accommodation-current.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {
  CreateReservationComponent
} from "../../pages/view-accommodation/create-reservation/create-reservation.component";
import {
  GradeAccommodationDialogComponent
} from "../../pages/grade-accommodation/grade-accommodation-dialog/grade-accommodation-dialog.component";
import {CancelReservationComponent} from "../../pages/reservation/cancel-reservation/cancel-reservation.component";

@Component({
  selector: 'app-accommodation-card',
  templateUrl: './accommodation-card.component.html',
  styleUrls: ['./accommodation-card.component.scss']
})
export class AccommodationCardComponent implements OnInit{
  @Input() accommodation! : IAccommodationDto
  @Input() from :Date = new Date()
  @Input() to :Date = new Date()
  user : User | null;
  @Input() numberOfGuests : number = 0;
  pricePerPersonOneNight  = 0;
  pricePerPersonInTotal = 0
  priceInTotalOneNight = 0
  priceInTotalInTotal = 0
  constructor(private authService: AuthService, private currentService:AccommodationCurrentService,
              private router:Router, private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.user = this.authService.getUser()
    this.findCurrentPrice()
    console.log(this.accommodation.id)
  }

  onBook() {
    this.currentService.accommodation = this.accommodation
    const dialogRef = this.dialog.open(CreateReservationComponent, {
      width: '400px',
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log('Modal closed');
    });
  }

  viewAccommodation() {
    this.currentService.accommodation = this.accommodation
    this.router.navigate(['accommodation-view']).then(_ => this.currentService.accommodation = this.accommodation)
  }

  private findCurrentPrice() {
    this.accommodation.specialPrices.forEach( a => {
      if(new Date(a.dateRange.from).getTime() <= this.from.getTime() && this.from.getTime() <= new Date(a.dateRange.to).getTime())
      {
        this.pricePerPersonOneNight = a.price
        this.priceInTotalOneNight = (a.price * this.numberOfGuests)
        let start= this.from
        let end = this.to
        let differenceMs = end.getTime() - start.getTime();
        let daysDiff = Math.floor(differenceMs / (1000 * 60 * 60 * 24))
        let priceTotal = (this.numberOfGuests * a.price) * daysDiff
        this.priceInTotalInTotal = priceTotal;
        this.pricePerPersonInTotal = (daysDiff * a.price)
      }
    })
  }

  onGrade() {
    this.currentService.accommodation = this.accommodation
    this.dialog.open(GradeAccommodationDialogComponent, {
      width: '400px',
      height:'300px',
      data: { accommodation: this.accommodation, text : 'accommodation' }
    });
  }
}
