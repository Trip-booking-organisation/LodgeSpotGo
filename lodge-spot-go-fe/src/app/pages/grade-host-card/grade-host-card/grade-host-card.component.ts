import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UserManagementService} from "../../../common/services/user-management.service";
import {HostGrade} from "../../../common/model/host-grade";
import {KeycloakUser} from "../../../common/model/keycloakUser";
import {Observable} from "rxjs";
import {
  EditAccommodationGradeComponent
} from "../../guest-grades/guest-grades-view/edit-accommodation-grade/edit-accommodation-grade.component";
import {MatDialog} from "@angular/material/dialog";
import {DeleteHostGradeRequest} from "../../../common/model/DeleteHostGradeRequest";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-grade-host-card',
  templateUrl: './grade-host-card.component.html',
  styleUrls: ['./grade-host-card.component.scss']
})
export class GradeHostCardComponent implements OnInit{
  @Output() editAccommodationGrade: EventEmitter<any> = new EventEmitter();
  @Output() deleteAccommodation: EventEmitter<string> = new EventEmitter<string>()
  constructor(private userService:UserManagementService,
              private dialog: MatDialog, private toasterService: ToastrService) {
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
    const ref = this.dialog.open(EditAccommodationGradeComponent, {
      width: '400px',
      height:'300px',
      data: { hostGrade: this.hostGrade, text : 'host' }
    });
    ref.afterClosed().subscribe(value => {
      this.editAccommodationGrade.emit(value)
    })
  }

  deleteGrade() {
    let request:DeleteHostGradeRequest={
      gradeId:this.hostGrade.id,
      guestId:this.hostGrade.guestId
    }
    this.userService.deleteHostGrade(request).subscribe({
      next: _ =>{
        this.toasterService.success("Successfully deleted host grade", "Success")
        this.deleteAccommodation.emit(this.hostGrade.id)
      },
      error: _ => {
        this.toasterService.error("You cannot delete this grade", "Fail")
      }
    })
  }
}
