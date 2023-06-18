import {Component, EventEmitter, Inject, OnInit, Output} from '@angular/core';
import {FormControl} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AuthService} from "../../../../core/keycloak/auth.service";
import {GradeAccommodationService} from "../../../../common/services/grade-accommodation.service";
import {AccommodationGrade} from "../../../../common/model/accommodation-grade";
import {UpdateAccommodationGradeRequest} from "../../../../common/model/update-accommodation-grade-request";
import {DataService} from "../../../../common/services/data.service";
import {HostGrade} from "../../../../common/model/host-grade";
import {UserManagementService} from "../../../../common/services/user-management.service";
import {UpdateHostGradeRequest} from "../../../../common/model/UpdateHostGradeRequest";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-edit-accommodation-grade',
  templateUrl: './edit-accommodation-grade.component.html',
  styleUrls: ['./edit-accommodation-grade.component.scss']
})
export class EditAccommodationGradeComponent implements OnInit {
  accommodationGrade!: AccommodationGrade
  hostGrade! : HostGrade
  text!: string
  gradeFormControl = new FormControl(0);
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<EditAccommodationGradeComponent>,
              private auth: AuthService,
              private gradeAccommodationClient: GradeAccommodationService,
              private dataService : DataService,
              private userService:UserManagementService,
              private toastrService: ToastrService) {
    this.accommodationGrade = this.data.accommodationGrade;
    this.hostGrade = this.data.hostGrade;
    this.text = this.data.text
  }

  ngOnInit(): void {
  }

  onConfirm() {
    console.log(this.gradeFormControl.value)
    if (this.text === 'accommodation') {
      let request: UpdateAccommodationGradeRequest = {
        id:this.accommodationGrade.id,
        number: this.gradeFormControl.value
      };
      this.gradeAccommodationClient.updateAccommodationGrade(request).subscribe({
        next: _ => {
          this.toastrService.success("Successfully edited grade")
          this.dialogRef.close(request);
        },
        error: _ => {
          this.toastrService.error("Max grade is 5!", "Fail")
        }
      })
    }
    if (this.text === 'host') {
      let request: UpdateHostGradeRequest = {
        id:this.hostGrade.id,
        number:this.gradeFormControl.value,
        guestId:this.hostGrade.guestId,
        hostId:this.hostGrade.hostId
      };
      this.userService.updateHostGrade(request).subscribe({
        next: _ =>{
          this.toastrService.success("Successfully edited grade")
          this.dialogRef.close(request);
        },
        error: _ => {
        this.toastrService.error("Max grade is 5!", "Fail")
      }
      })
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
