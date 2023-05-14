import { Component } from '@angular/core';
import {Accommodation} from "../model/accommodation";
import {Address} from "../model/addres";
import {AccommodationService} from "../../../services/accommodationService";
import {animate} from "@angular/animations";


@Component({
  selector: 'app-accommodation-create',
  templateUrl: './accommodation-create.component.html',
  styleUrls: ['./accommodation-create.component.scss']
})
export class AccommodationCreateComponent {

  wifi= false;
  kitchen= false;
  airCondition= false;
  freeParking= false;
  fridge = false;

  name ="";
  country = "";
  city= "";
  street = "";
  max = 0;
  min = 0;

  constructor(private readonly accomodationService:AccommodationService) {

  }

  assambleAmenitetis():string[]{
    var amenitetis:string[] = []
    if(this.freeParking){
      amenitetis.push("free parking")
    }
    if(this.wifi){
      amenitetis.push("wifi included")
    }
    if(this.kitchen){
      amenitetis.push("kitchen included")
    }
    if(this.airCondition){
      amenitetis.push("air condition included")
    }
    if(this.fridge){
      amenitetis.push("fridge included")
    }
    return amenitetis;
  }

  create() {
    var addres : Address ={
      country : this.country,
      city:this.city,
      street:this.street
    }
    console.log(addres)
    var acc: Accommodation = {
      name: this.name,
      address: addres,
      max_guests: this.max,
      min_guests: this.min,
      amenities: this.assambleAmenitetis()
    }
    this.accomodationService.createAccommodation(acc).subscribe({
      next:res =>{
        console.log(res)
      }
    })
  }
}
