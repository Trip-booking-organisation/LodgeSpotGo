import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {IReservation} from "../../../common/model/reservations";
import {User} from "../../../core/keycloak/user";
import {AuthService} from "../../../core/keycloak/auth.service";
import {FormControl} from "@angular/forms";
import {GradeAccommodationService} from "../../../common/services/grade-accommodation.service";
import {GradeAccommodationRequest, GradeHostRequst} from "../../../common/model/GradeAccommodationRequest";
import {GradeHostService} from "../../../common/services/grade-host.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-grade-accommodation-dialog',
  templateUrl: './grade-accommodation-dialog.component.html',
  styleUrls: ['./grade-accommodation-dialog.component.scss']
})
export class GradeAccommodationDialogComponent implements OnInit{
  accommodationId! : string
  hostId! : string
  text!: string
  user : User | null;
  gradeFormControl  = new FormControl(0);
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<GradeAccommodationDialogComponent>,
              private auth:AuthService,
              private gradeAccommodationClient : GradeAccommodationService,
              private toastr: ToastrService,
              private gradeHostClient:GradeHostService) {
    this.hostId = this.data.accommodation.hostId;
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
        number: this.gradeFormControl.value,
        guestEmail: this.user.email
      };
      this.gradeAccommodationClient.gradeAccommodation(request).subscribe({
        next: _ =>{
          this.toastr.success("You have successfully rated this accommodation.","Thank you for your support.")
        },error: _ =>{
          this.toastr.error("You cannot rate this accommodation!","Failed to rate")
        }
      })
    }
    if(this.text === 'host'){
      let request : GradeHostRequst ={
        accomodationId: this.accommodationId,
        guestId: this.user.id,
        number: this.gradeFormControl.value,
        hostId:this.hostId,
        guestEmail: this.user.email
      }
      this.gradeHostClient.gradeAccommodation(request).subscribe({
        next:_ =>{
          this.toastr.success("U have successfully rated this host.","Thank you for your support.")
        },
        error: _ =>{
        this.toastr.error("You cannot rate this host!","Failed to rate")
      }
      })
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
