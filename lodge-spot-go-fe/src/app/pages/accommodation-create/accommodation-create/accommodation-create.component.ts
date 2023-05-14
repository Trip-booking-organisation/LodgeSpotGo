import { Component } from '@angular/core';
import {Address} from "../../../shered/model/addres";
import {AccommodationService} from "../../../common/services/accommodationService";
import {Accommodation} from "../../../common/model/accommodation";
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";
import {AuthService} from "../../../core/keycloak/auth.service";


@Component({
  selector: 'app-accommodation-create',
  templateUrl: './accommodation-create.component.html',
  styleUrls: ['./accommodation-create.component.scss']
})
export class AccommodationCreateComponent {

  base64String: string = '';
  photos: string[] = [];
  wifi= false;
  kitchen= false;
  airCondition= false;
  freeParking= false;
  fridge = false;
  isAutomatic = false;

  name ="";
  country = "";
  city= "";
  street = "";
  max = 0;
  min = 0;
  userId: string = "";

  constructor(private readonly accommodationService:AccommodationService, private toastService:ToastrService,
              private router:Router,private authService:AuthService) {
    this.authService.getUserObservable().subscribe(
      value => {
        console.log(value)
        this.userId = value?.id!
      })
  }

  assambleAmenitetis():string[]{
    let amenitetis:string[] = []
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
    let phtsBase: string[] = []
    phtsBase.push(this.base64String)
    console.log(phtsBase)
    let address : Address ={
      country : this.country,
      city:this.city,
      street:this.street
    }
    console.log(address)
    let accommodation: Accommodation = {
      name: this.name,
      address: address,
      max_guests: this.max,
      min_guests: this.min,
      amenities: this.assambleAmenitetis(),
      photos: this.photos,
      hostId: this.userId,
      automaticConfirmation: this.isAutomatic
    }
    if(accommodation.max_guests! < accommodation.min_guests!){
      this.toastService.error("Wrong range max and min guests!")
      return
    }
    if(accommodation.name === '' || accommodation.address?.city === ''
      || accommodation.name === ''  || !this.assambleAmenitetis()){
      this.toastService.error("Please valid fulfill form!")
      return;
    }
    console.log(accommodation)
    this.accommodationService.createAccommodation(accommodation).subscribe({
      next:res =>{
        console.log(res)
        this.toastService.success("You are successfully created accommodation","Success")
        this.router.navigate([''])
      },
      error: err => {
        console.log(err)
        this.toastService.error(err.toString(),"Error")
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
        this.photos.push(this.base64String)
      };
      reader.readAsDataURL(file);
    }
  }

  deletePhoto(i: number) {
    const filter =this.photos.filter((photo, index) => i !== index)
    this.photos = [...filter]
  }
}
