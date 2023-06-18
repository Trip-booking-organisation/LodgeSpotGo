import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {IAccommodationDto} from "../../../../common/model/accommodation-dto";
import {User} from "../../../../core/keycloak/user";
import {AuthService} from "../../../../core/keycloak/auth.service";
import {AccommodationCurrentService} from "../../../../common/services/accommodation-current.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {CreateReservationComponent} from "../../../view-accommodation/create-reservation/create-reservation.component";
import {
  GradeAccommodationDialogComponent
} from "../../../grade-accommodation/grade-accommodation-dialog/grade-accommodation-dialog.component";
import {AccommodationGrade} from "../../../../common/model/accommodation-grade";
import {GradeAccommodationService} from "../../../../common/services/grade-accommodation.service";
import {EditAccommodationGradeComponent} from "../edit-accommodation-grade/edit-accommodation-grade.component";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-accommodation-grade-card',
  templateUrl: './accommodation-grade-card.component.html',
  styleUrls: ['./accommodation-grade-card.component.scss']
})
export class AccommodationGradeCardComponent implements OnInit{
  @Input() accommodationGrade! : AccommodationGrade
  user : User | null;
  @Output() gradeEmit = new  EventEmitter<AccommodationGrade>;
  @Output() gradeUpdateEmit = new  EventEmitter<any>;
  constructor(private authService: AuthService,
              private currentService:AccommodationCurrentService,
              private router:Router,
              private gradeClient : GradeAccommodationService,
              private dialog: MatDialog,
              private toastrService: ToastrService) {
  }
  ngOnInit(): void {
    this.user = this.authService.getUser()

  }

  onDeleteGrade() {
    this.gradeClient.deleteGrade(this.accommodationGrade.id).subscribe({
      next: _=>{
        this.gradeEmit.emit(this.accommodationGrade)
        this.toastrService.success("Successfully deleted")
      },
      error: _ => {
        this.toastrService.error("Error when deleting")
      }
    })
  }

  onEditGrade() {
    const ref = this.dialog.open(EditAccommodationGradeComponent, {
      width: '30vw',
      height:'30vh',
      data: { accommodationGrade: this.accommodationGrade, text : 'accommodation' }
    });
    ref.afterClosed().subscribe(value => {
      this.gradeUpdateEmit.emit(value)
    })
  }
}
