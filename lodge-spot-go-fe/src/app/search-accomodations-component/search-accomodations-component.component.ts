import { Component, Input, OnInit } from '@angular/core';
import {AccomodationResult, MyDate} from '../core/model/AccomodationResult';
import { FormControl } from '@angular/forms';
import { accomodationsAutoComplete } from '../core/data-access/cityAndCountryData';
import { Observable, map, startWith } from 'rxjs';

import { Address } from '../core/model/Address';
import {AccommodationService} from "../common/services/accommodationService";


@Component({
  selector: 'app-search-accomodations-component',
  templateUrl: './search-accomodations-component.component.html',
  styleUrls: ['./search-accomodations-component.component.scss']
})
export class SearchAccomodationsComponentComponent implements OnInit {

  @Input() searchResults: AccomodationResult[] = [];

  //locationControl = new FormControl();
  numberOfGuests: number | undefined;
  startDate = new Date()
  endDate = new Date()
  //accomodationAddresses = accomodationsAutoComplete;
  //filteredLocation!: Observable<Address[]>;

  isLoading!: boolean;
 // cityCountry: any;
  country: any;
  city: any

  constructor(private accomodationService: AccommodationService){}

  ngOnInit(): void {


  }





  // parseCityCountry (){
  //   var parsed = this.cityCountry.parse(",")
  //   this.city = parsed[0]
  //   this.country = parsed[1]
  // }
searchAccomodations(){
    var sd:MyDate ={
      date : this.startDate.toDateString()
    }
  var ed:MyDate ={
    date : this.endDate.toDateString()
  }

    var accomodationRequest : AccomodationResult={
      city: this.city,
      country: this.country,
      numberOfGuests: this.numberOfGuests,
      endDate: this.endDate,
      startDate: this.startDate

  }
  console.log(accomodationRequest)
  this.accomodationService.searchAccomodation(accomodationRequest)
    .subscribe({
      next: (data: AccomodationResult[]) => {
        this.searchResults = data
        this.isLoading = false
      },
      error: (_: any) => {
        //this.toast.error("Search error occurs!", "Search")
        this.isLoading = false
      }
    })}}
