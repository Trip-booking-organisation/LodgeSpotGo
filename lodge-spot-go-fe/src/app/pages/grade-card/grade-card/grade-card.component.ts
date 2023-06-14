import {Component, Input, OnInit} from '@angular/core';
import {AccommodationGradeResponse} from "../../../common/model/accommodation-grade-response";
import {GradeAccommodationService} from "../../../common/services/grade-accommodation.service";

@Component({
  selector: 'app-grade-card',
  templateUrl: './grade-card.component.html',
  styleUrls: ['./grade-card.component.scss']
})
export class GradeCardComponent implements OnInit{
@Input() accommodationGrade : AccommodationGradeResponse
  constructor() {
  }
  ngOnInit(): void {

  }
}
