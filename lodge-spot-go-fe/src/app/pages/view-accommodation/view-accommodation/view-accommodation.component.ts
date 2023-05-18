import {Component, OnInit} from '@angular/core';
import {Accommodation} from "../../../common/model/accommodation";
import {AccommodationCurrentService} from "../../../common/services/accommodation-current.service";
import {IAccommodationDto} from "../../../common/model/accommodation-dto";
import {SpecialPrice} from "../../../common/model/specialPrice";
import {toIsoString} from "../../../common/utility/date.converter";
import {Observable} from "rxjs";

@Component({
  selector: 'app-view-accommodation',
  templateUrl: './view-accommodation.component.html',
  styleUrls: ['./view-accommodation.component.scss']
})
export class ViewAccommodationComponent implements OnInit{
  accommodation: IAccommodationDto;
  accommodation$: Observable<IAccommodationDto>;

  constructor(private currentService:AccommodationCurrentService) {
  }

  ngOnInit(): void {
    this.accommodation = this.currentService.accommodation
    this.accommodation$ = this.currentService.accommodation$
  }
}
