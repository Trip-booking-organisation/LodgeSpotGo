import {Component, OnInit} from '@angular/core';
import {AccommodationResponse} from "../../../shered/model/accommodationResponse";
import {AccommodationService} from "../../../common/services/accommodationService";

@Component({
  selector: 'app-hosts-accomodations',
  templateUrl: './hosts-accomodations.component.html',
  styleUrls: ['./hosts-accomodations.component.scss']
})
export class HostsAccomodationsComponent  implements OnInit{

  yourAccomodations: AccommodationResponse[] = []

  constructor( private readonly accomodationService:AccommodationService) {
  }
  ngOnInit(): void {
    this.accomodationService.getAllAccommodations().subscribe({
      next:res =>{
        this.yourAccomodations = res.accommodations;
        console.log(res.accommodations)
      }
    })

  }

}
