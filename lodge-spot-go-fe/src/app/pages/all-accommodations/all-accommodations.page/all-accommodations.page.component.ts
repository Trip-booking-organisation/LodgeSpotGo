import {Component, OnInit} from '@angular/core';
import {AccommodationService} from "../../../common/services/accommodationService";
import {Accommodation} from "../../../common/model/accommodation";
import {map, Observable} from "rxjs";
import {AccommodationResponse} from "../../../shered/model/accommodationResponse";

@Component({
  selector: 'app-all-accommodations.page',
  templateUrl: './all-accommodations.page.component.html',
  styleUrls: ['./all-accommodations.page.component.scss']
})
export class AllAccommodationsPageComponent implements OnInit {
  accommodations$ : Observable<any>
  accommodations : Accommodation[] = []
  constructor(private accommodationService:AccommodationService) {
  }
  ngOnInit(): void {
    this.accommodations$ = this.accommodationService.getAllAccommodations().pipe(
      map(response => response.accommodations)
    );
  }

}
