import {Component, OnInit} from '@angular/core';
import {GradeAccommodationService} from "../../../common/services/grade-accommodation.service";
import {AuthService} from "../../../core/keycloak/auth.service";
import {Observable, switchMap, take, tap} from "rxjs";
import {AccommodationGrade} from "../../../common/model/accommodation-grade";
import {MatDialog} from "@angular/material/dialog";
import {HostGrades} from "../../../common/model/host-grade";
import {UserManagementService} from "../../../common/services/user-management.service";

@Component({
  selector: 'app-guest-grades-view',
  templateUrl: './guest-grades-view.component.html',
  styleUrls: ['./guest-grades-view.component.scss']
})
export class GuestGradesViewComponent implements OnInit{
  grades:AccommodationGrade[] =[];
  hostGrades$: Observable<HostGrades>
  constructor(private gradeClient : GradeAccommodationService,
              private authService : AuthService,
              private userService:UserManagementService,
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
    this.hostGrades$ = this.userService.getGuestsGrades({
      guestId: user.id
    })
    console.log(user.id)

  }

  onDelete($event: AccommodationGrade) {
    this.grades = this.grades.filter(x => x.id != $event.id);
  }
}
