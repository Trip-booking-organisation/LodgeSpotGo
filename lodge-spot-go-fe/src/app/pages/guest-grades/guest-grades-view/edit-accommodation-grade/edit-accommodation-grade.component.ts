import {Component, EventEmitter, Inject, OnInit, Output} from '@angular/core';
import {User} from "../../../../core/keycloak/user";
import {FormControl} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AuthService} from "../../../../core/keycloak/auth.service";
import {GradeAccommodationService} from "../../../../common/services/grade-accommodation.service";
import {GradeAccommodationRequest} from "../../../../common/model/GradeAccommodationRequest";
import {AccommodationGrade} from "../../../../common/model/accommodation-grade";
import {UpdateAccommodationGradeRequest} from "../../../../common/model/update-accommodation-grade-request";
import {DataService} from "../../../../common/services/data.service";

@Component({
  selector: 'app-edit-accommodation-grade',
  templateUrl: './edit-accommodation-grade.component.html',
  styleUrls: ['./edit-accommodation-grade.component.scss']
})
export class EditAccommodationGradeComponent implements OnInit {
  accommodationGrade!: AccommodationGrade
  text!: string
  gradeFormControl = new FormControl(0);
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<EditAccommodationGradeComponent>,
              private auth: AuthService,
              private gradeAccommodationClient: GradeAccommodationService,
              private dataService : DataService) {
    this.accommodationGrade = this.data.accommodationGrade;
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
        }
      })
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
