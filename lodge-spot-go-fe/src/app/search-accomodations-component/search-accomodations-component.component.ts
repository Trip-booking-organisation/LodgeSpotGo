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


@Component({
  selector: 'app-search-accomodations-component',
  templateUrl: './search-accomodations-component.component.html',
  styleUrls: ['./search-accomodations-component.component.scss']
})
export class SearchAccomodationsComponentComponent implements OnInit {

  searchResults: IAccommodationDto[] = [];
  flightsAddresses = flightsAutoComplete;
  filteredLocations!: Observable<Address[]>;
  location = new FormControl()
  numberOfGuests = new FormControl();
  from = new FormControl()
  to =new FormControl()
  isLoading!: boolean;
  country: any;
  city: any

  constructor(private accomodationService: AccommodationService,
              private searchService: SearchAndFilterService){}

  ngOnInit(): void {
    this.mapFilterTo()
  }
  private mapFilterTo() {
    this.filteredLocations = this.location.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filterFlights(value))
      )
  }
  private _filterFlights(value: string): any[] {
    const filterValue = value.toLowerCase();
    return this.flightsAddresses.filter(flight => {
      const city = flight.city.toLowerCase();
      const country = flight.country.toLowerCase();
      return city.includes(filterValue) || country.includes(filterValue);
    });
  }

  searchAccommodations() {
    const numberOfGuests = this.numberOfGuests.value
    const startDate = Math.floor(new Date(this.from.value).getTime() / 1000)
    const endDate = Math.floor(new Date(this.to.value).getTime() / 1000)
    const array = this.location.value.split(",")
    const city = array[0]
    const country = array[1]
    this.searchService.searchAccommodations(numberOfGuests,startDate,endDate,city,country).subscribe({
      next: response => {
        const accommodations = response.accommodations
        this.searchResults = [...accommodations]
        console.log(this.searchResults)
      }
    })
  }
}
