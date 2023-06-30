import {Component, OnInit} from '@angular/core';
import {GradeAccommodationService} from "../../../common/services/grade-accommodation.service";
import {AuthService} from "../../../core/keycloak/auth.service";
import {BehaviorSubject, Observable, switchMap, take, tap} from "rxjs";
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
  hostGrades$: HostGrades;
  constructor(private gradeClient : GradeAccommodationService,
              private authService : AuthService,
              private userService:UserManagementService,
              private dialog: MatDialog) {
  }
  ngOnInit(): void {
    let user = this.authService.getUserAsSignal()
    this.gradeClient.getGradesByGuest(user().id).subscribe({
      next: value => {
        this.grades = value.grades
        console.log('grade')
        console.log(this.grades)
      }
    })
    this.userService.getGuestsGrades({
      guestId: user().id
    }).subscribe({
      next: value => {
        this.hostGrades$ = value
      }
    })
  }

  onDelete($event: AccommodationGrade) {
    this.grades = this.grades.filter(x => x.id != $event.id);
  }

  onEdit($event: any) {
    this.grades = this.grades.map(x => {
      if (x.id === $event.id) {
        return { ...x, number: $event.number };
      } else {
        return x;
      }
    });
  }

  onDeleteGrade($event: string) {
    this.hostGrades$.hostGrades = this.hostGrades$.hostGrades.filter(x => x.id != $event);
  }

  onEditGrade($event: any) {
    this.hostGrades$.hostGrades = this.hostGrades$.hostGrades.map(x => {
      if (x.id === $event.id) {
        return { ...x, number: $event.number };
      } else {
        return x;
      }
    });
  }
}
