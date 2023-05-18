import {Component, OnInit} from '@angular/core';
import {AccommodationResponse} from "../../../shered/model/accommodationResponse";
import {AccommodationService} from "../../../common/services/accommodationService";
import {AuthService} from "../../../core/keycloak/auth.service";
import {map, Observable, of} from "rxjs";
import {Address} from "../../../shered/model/addres";
import {SpecialPrice} from "../../../shered/model/specialPrice";
import {FormControl, Validators} from "@angular/forms";
import {PriceData} from "../../../common/services/accommodation.price.model";
import {toIsoString} from "../../../common/utility/date.converter";
import {DateRange} from "../../../shered/model/dateRange";
import {AccommodationCurrentService} from "../../../common/services/accommodation-current.service";
import { Router } from '@angular/router';

export interface AccommodationPrice {
  id?: string;
  name?: string;
  address?:Address;
  max_guests?:number;
  min_guests?:number;
  amenities?:string[];
  photos?:string[];
  specialPrices?:SpecialPrice[]
  isPriceActive: boolean;
}
@Component({
  selector: 'app-hosts-accomodations',
  templateUrl: './hosts-accomodations.component.html',
  styleUrls: ['./hosts-accomodations.component.scss']
})
export class HostsAccomodationsComponent  implements OnInit{

  transformedList$: Observable<AccommodationPrice[]>
  startDate: FormControl<Date> = new FormControl<Date>(null,Validators.required);
  finishDate: FormControl<Date> = new FormControl<Date>(null,Validators.required);
  price: FormControl<number> = new FormControl<number>(0,Validators.required);
  constructor( private readonly accommodationService:AccommodationService,
               private readonly authService:AuthService,
               private currentService:AccommodationCurrentService,
               private router:Router) {
  }
  ngOnInit(): void {
    this.transformedList$ = this.accommodationService
      .getAccommodationByHost(this.authService.getUser().id).pipe(
      map((responses: {accommodations: AccommodationResponse[]}) => {
        console.log(responses)
          return responses.accommodations.map((response: AccommodationResponse) => ({
            ...response,
            isPriceActive: false
          }))
        }
      )
    );
  }

  addPrice(accommodation: AccommodationPrice) {
    accommodation.isPriceActive = !accommodation.isPriceActive;

  }

  submitPrice(accommodation: AccommodationPrice) {
    console.log(this.startDate.value)
    console.log(this.finishDate.value)
    console.log(this.price.value)
    const priceData: PriceData = {
      price: {
        dateRange: {
          from: toIsoString(this.startDate.value),
          to: toIsoString(this.finishDate.value),
        },
        price: this.price.value as number,
      },
      accommodationId: accommodation.id
    };
    this.accommodationService.updatePrice(priceData).subscribe({
      next: (response: AccommodationPrice) => {
        console.log(response)
      },
      error: err => {
        console.log(err.message)
      }
    })
    const special:SpecialPrice = {
      dateRange: {
        from: this.startDate.value,
        to: this.finishDate.value,
      },
      price:this.price.value as number
    }
    accommodation.specialPrices = [...accommodation.specialPrices,special]
    accommodation.isPriceActive = !accommodation.isPriceActive
  }

  viewAccommodation(accommodation: AccommodationPrice) {
    const acc = {...accommodation,hostId: this.authService.getUser().id}
    this.currentService.accommodation = acc
    this.router.navigate(['accommodation-view'])
      .then(_ => this.currentService.accommodation = acc)

  }
}
