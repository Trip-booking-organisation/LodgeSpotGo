import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {IReservation} from "../../../common/model/reservations";
import {User} from "../../../core/keycloak/user";
import {AuthService} from "../../../core/keycloak/auth.service";
import {FormControl} from "@angular/forms";
import {GradeAccommodationService} from "../../../common/services/grade-accommodation.service";
import {GradeAccommodationRequest} from "../../../common/model/GradeAccommodationRequest";

@Component({
  selector: 'app-grade-accommodation-dialog',
  templateUrl: './grade-accommodation-dialog.component.html',
  styleUrls: ['./grade-accommodation-dialog.component.scss']
})
export class GradeAccommodationDialogComponent implements OnInit{
  accommodationId! : string
  text!: string
  user : User | null;
  gradeFormControl  = new FormControl(0);
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<GradeAccommodationDialogComponent>,
              private auth:AuthService,
              private gradeAccommodationClient : GradeAccommodationService) {
    this.accommodationId = this.data.accommodation.id;
    this.text = this.data.text
  }
  ngOnInit(): void {
    this.user = this.auth.getUser()
    console.log(this.user);
  }

  onConfirm() {
    console.log(this.gradeFormControl.value)
    if(this.text === 'accommodation')
    {
      let request : GradeAccommodationRequest={
        accommodationId: this.accommodationId,
        guestId: this.user.id,
        number: this.gradeFormControl.value
      };
      this.gradeAccommodationClient.gradeAccommodation(request).subscribe({
        next: _=>{

        }
      })
    }


  }

  onCancel() {
    this.dialogRef.close();
  }
}
