import {Component, Input, OnInit} from '@angular/core';
import {UserManagementService} from "../../../common/services/user-management.service";
import {HostGrade} from "../../../common/model/host-grade";
import {KeycloakUser} from "../../../common/model/keycloakUser";
import {Observable} from "rxjs";
import {
  EditAccommodationGradeComponent
} from "../../guest-grades/guest-grades-view/edit-accommodation-grade/edit-accommodation-grade.component";
import {MatDialog} from "@angular/material/dialog";
import {DeleteHostGradeRequest} from "../../../common/model/DeleteHostGradeRequest";

@Component({
  selector: 'app-grade-host-card',
  templateUrl: './grade-host-card.component.html',
  styleUrls: ['./grade-host-card.component.scss']
})
export class GradeHostCardComponent implements OnInit{
  constructor(private userService:UserManagementService,
              private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.guest$ = this.userService.getUser(this.hostGrade.guestId)
    this.userService.getUser(this.hostGrade.guestId).subscribe({
      next: res =>{
        this.guest = res
        console.log(this.hostGrade)

      }
    })
  }
  @Input() hostGrade : HostGrade
  guest:KeycloakUser
  guest$:Observable<KeycloakUser>
  @Input()canEdit: boolean;

  editGrade() {
    this.dialog.open(EditAccommodationGradeComponent, {
      width: '400px',
      height:'300px',
      data: { hostGrade: this.hostGrade, text : 'host' }
    });
  }

  deleteGrade() {
    let request:DeleteHostGradeRequest={
      gradeId:this.hostGrade.id,
      guestId:this.hostGrade.guestId
    }
    this.userService.deleteHostGrade(request).subscribe({
      next: res=>{

      }
    })
  }
}
