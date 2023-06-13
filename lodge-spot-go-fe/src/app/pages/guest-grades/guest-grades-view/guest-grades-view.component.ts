import {Component, OnInit} from '@angular/core';
import {GradeAccommodationService} from "../../../common/services/grade-accommodation.service";
import {AuthService} from "../../../core/keycloak/auth.service";
import {Observable, switchMap, take, tap} from "rxjs";
import {AccommodationGrade} from "../../../common/model/accommodation-grade";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-guest-grades-view',
  templateUrl: './guest-grades-view.component.html',
  styleUrls: ['./guest-grades-view.component.scss']
})
export class GuestGradesViewComponent implements OnInit{
  grades:AccommodationGrade[] =[];
  constructor(private gradeClient : GradeAccommodationService,
              private authService : AuthService,
              private dialog: MatDialog) {
  }
  ngOnInit(): void {
    let user = this.authService.getUser()
    this.gradeClient.getGradesByGuest(user.id).subscribe({
      next: value => {
        this.grades = value.grades
        console.log(this.grades)
      }
    })
  }

  onDelete($event: AccommodationGrade) {
    this.grades = this.grades.filter(x => x.id != $event.id);
  }
}
