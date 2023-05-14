import { Component } from '@angular/core';
import {Accommodation} from "../../../shered/model/accommodation";
import {Address} from "../../../shered/model/addres";
import {AccommodationService} from "../../../services/accommodationService";
import {animate} from "@angular/animations";
import { Buffer } from 'buffer';
import * as pako from 'pako';


@Component({
  selector: 'app-accommodation-create',
  templateUrl: './accommodation-create.component.html',
  styleUrls: ['./accommodation-create.component.scss']
})
export class AccommodationCreateComponent {

  base64String: string = '';
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
    var phts: string[]= []
    phts.push(this.base64String)
    console.log(phts)
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
      amenities: this.assambleAmenitetis(),
      photos:phts
    }
    console.log(acc)
    this.accomodationService.createAccommodation(acc).subscribe({
      next:res =>{
        console.log(res)
      }
    })
  }

  onFileSelected(event: any) {
    const fileInput: HTMLInputElement = event.target as HTMLInputElement;
    const files: FileList | null = fileInput.files;

    if (files && files.length > 0) {
      const file: File = files[0];
      const reader = new FileReader();

      reader.onloadend = () => {
        this.base64String = reader.result!.toString().split(',')[1];
      };

      reader.readAsDataURL(file);
      const compressedData = pako.deflate(this.base64String);
      console.log(compressedData)
    }
  }
}
