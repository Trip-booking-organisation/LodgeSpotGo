import { Component, Input, OnInit } from '@angular/core';
import { AccomodationResult } from '../core/model/AccomodationResult';
import { FormControl } from '@angular/forms';
import { accomodationsAutoComplete } from '../core/data-access/cityAndCountryData';
import { Observable, map, startWith } from 'rxjs';

import { Address } from '../core/model/Address';


@Component({
  selector: 'app-search-accomodations-component',
  templateUrl: './search-accomodations-component.component.html',
  styleUrls: ['./search-accomodations-component.component.scss']
})
export class SearchAccomodationsComponentComponent implements OnInit {

  @Input() searchResults: AccomodationResult[] = [];
  locationControl = new FormControl();
  numberOfGuestsControl = new FormControl()
  startDate = new Date()
  endDate = new Date()
  accomodationAddresses = accomodationsAutoComplete;
  filteredLocation!: Observable<Address[]>;
  filteredGuests!: Observable<number[]>;
  isLoading!: boolean;

  constructor(){}

  ngOnInit(): void {
    this.mapFilterLocation()
    this.mapFilterNumberOfGuests()
  }
  private mapFilterLocation() {
    this.filteredLocation = this.locationControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterAccomodations(value))
    )
  }
  private mapFilterNumberOfGuests() {
    this.filteredGuests = this.numberOfGuestsControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterAccomodations(value))
    )
  }

  private _filterAccomodations(value: string): any[] {
    const filterValue = value.toLowerCase();
    return this.accomodationAddresses.filter(accomodation => {
      const street = accomodation.street.toLowerCase();
      const city = accomodation.city.toLowerCase();
      const country = accomodation.country.toLowerCase();

      return street.includes(filterValue) || city.includes(filterValue) || country.includes(filterValue);
    });
  }
searchAccomodations(){}

}
