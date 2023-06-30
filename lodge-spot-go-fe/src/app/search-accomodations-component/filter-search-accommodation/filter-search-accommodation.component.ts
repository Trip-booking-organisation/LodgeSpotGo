import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {amenities} from "../data-access/amenities";
import {FormControl} from "@angular/forms";
import {ReservationFilterRequest} from "../../common/model/reservation-filter-request";
import {FilterAccommodations} from "../../common/model/filter-accommodations";
import {IAccommodationDto} from "../../common/model/accommodation-dto";
import {SearchAndFilterService} from "../../common/services/search-and-filter.service";


@Component({
  selector: 'app-filter-search-accommodation',
  templateUrl: './filter-search-accommodation.component.html',
  styleUrls: ['./filter-search-accommodation.component.scss']
})
export class FilterSearchAccommodationComponent implements OnInit{
  @Input() accommodations : IAccommodationDto[] = [];
  amenities = amenities;
  outstandingHost = new FormControl<boolean>(false);
  minGrade = new FormControl<number>(0);
  maxGrade = new FormControl<number>(0);
  amenitiesControl = new FormControl([]);
  filteredAccommodations : IAccommodationDto[] = [];
  @Output() filter = new EventEmitter<IAccommodationDto[]>;
  constructor(private filterClient : SearchAndFilterService) {
  }
  ngOnInit(): void {
    console.log(this.accommodations)
  }
  filterAccommodations() {
    console.log(this.outstandingHost.value)
    console.log('min grade', this.minGrade.value)
    console.log('max grade', this.maxGrade.value)
    let array  = this.amenitiesControl.value
    console.log(array)
    let filter : FilterAccommodations={
      accommodations :this.accommodations,
      maxGrade : this.maxGrade.value,
      minGrade: this.minGrade.value,
      outstandingHost : this.outstandingHost.value,
      amenities: array
    }
    let request : ReservationFilterRequest={
      filter : filter
    }
    this.filterClient.filterSearchedResults(filter).subscribe({
      next: response=>{
        this.filteredAccommodations = response.accommodations;
        console.log(this.filteredAccommodations)
        this.filter.emit(this.filteredAccommodations)
      }
    })
  }
}
