import { Component, Input, OnInit } from '@angular/core';
import {AccomodationResult} from '../core/model/AccomodationResult';
import { FormControl } from '@angular/forms';
import { Observable, map, startWith } from 'rxjs';

import { Address } from '../core/model/Address';
import {AccommodationService} from "../common/services/accommodationService";
import {SearchAndFilterService} from "../common/services/search-and-filter.service";
import {flightsAutoComplete} from "./data-access/cityAndCountryData";
import {Accommodation} from "../common/model/accommodation";
import {IAccommodationDto} from "../common/model/accommodation-dto";
import {RecommendationService} from "../common/services/recommendation.service";
import {AuthService} from "../core/keycloak/auth.service";
import {User} from "../core/keycloak/user";
import {GetRecommenedRequest} from "../core/model/GetRecommenedRequest";


@Component({
  selector: 'app-search-accomodations-component',
  templateUrl: './search-accomodations-component.component.html',
  styleUrls: ['./search-accomodations-component.component.scss']
})
export class SearchAccomodationsComponentComponent implements OnInit {

  searchResults: IAccommodationDto[] = [];
 recommendationResults: IAccommodationDto[] = [];
  flightsAddresses = flightsAutoComplete;
  filteredLocations!: Observable<Address[]>;
  location = new FormControl()
  numberOfGuests = new FormControl();
  from = new FormControl()
  to =new FormControl()
  isLoading: boolean;
  country: any;
  city: any
  isRecommended = true
  user : User | null;
  constructor(private accommodationService: AccommodationService,
              private recommendationService: RecommendationService,
              private auth : AuthService,
              private searchService: SearchAndFilterService){}

  ngOnInit(): void {
    // this.accommodationService.getAllAccommodations().subscribe({
    //   next: response =>{
    //     console.log(response.accommodations)
    //     this.recommendationResults = response.accommodations
    //   }
    // })
   this.getRecommendedAccommodations();
    this.mapFilterTo()
  }

  private getRecommendedAccommodations() {
    this.user = this.auth.getUser()
    let request:GetRecommenedRequest={
      Name:this.user.email
    }
    this.recommendationService.getRecommendedAccommodations(request).subscribe({
      next: response => {
        console.log(response)
        this.recommendationResults = response.accommodations

      }
    })
  }

  private mapFilterTo() {
    this.filteredLocations = this.location.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      )
  }
  private _filter(value: string): any[] {
    const filterValue = value.toLowerCase();
    return this.flightsAddresses.filter(flight => {
      const city = flight.city.toLowerCase();
      const country = flight.country.toLowerCase();
      return city.includes(filterValue) || country.includes(filterValue);
    });
  }

  searchAccommodations() {
    this.isRecommended = false;
    const numberOfGuests = this.numberOfGuests.value
    const startDate = Math.floor(new Date(this.from.value).getTime() / 1000)
    const endDate = Math.floor(new Date(this.to.value).getTime() / 1000)
    const array = this.location.value.split(",")
    const city = array[0]
    const country = array[1]
    this.searchService.searchAccommodations(numberOfGuests,startDate,endDate,city,country).subscribe({
      next: response => {
        if(response.accommodations.length===0)
          this.isRecommended = true
        const accommodations = response.accommodations
        this.searchResults = [...accommodations]
        console.log(this.searchResults)
      }
    })
  }

  onFilter($event: IAccommodationDto[]) {
    this.searchResults = $event
  }
}
