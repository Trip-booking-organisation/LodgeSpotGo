import {Component, OnInit} from '@angular/core';
import {Accommodation} from "../../../common/model/accommodation";
import {AccommodationCurrentService} from "../../../common/services/accommodation-current.service";
import {IAccommodationDto} from "../../../common/model/accommodation-dto";
import {SpecialPrice} from "../../../common/model/specialPrice";
import {toIsoString} from "../../../common/utility/date.converter";
import {Observable} from "rxjs";
import {
  GradeAccommodationDialogComponent
} from "../../grade-accommodation/grade-accommodation-dialog/grade-accommodation-dialog.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-view-accommodation',
  templateUrl: './view-accommodation.component.html',
  styleUrls: ['./view-accommodation.component.scss']
})
export class ViewAccommodationComponent implements OnInit{
  accommodation: IAccommodationDto;
  accommodation$: Observable<IAccommodationDto>;

  constructor(private currentService:AccommodationCurrentService,
              private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.accommodation = this.currentService.accommodation
    this.accommodation$ = this.currentService.accommodation$
  }

  rateHost() {
    this.currentService.accommodation = this.accommodation
    this.dialog.open(GradeAccommodationDialogComponent, {
      width: '400px',
      height:'300px',
      data: { accommodation: this.accommodation, text : 'host' }
    });
  }

}
