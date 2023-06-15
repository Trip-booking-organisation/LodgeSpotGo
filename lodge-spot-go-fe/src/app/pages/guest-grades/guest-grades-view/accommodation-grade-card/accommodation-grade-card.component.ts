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

@Component({
  selector: 'app-accommodation-grade-card',
  templateUrl: './accommodation-grade-card.component.html',
  styleUrls: ['./accommodation-grade-card.component.scss']
})
export class AccommodationGradeCardComponent implements OnInit{
  @Input() accommodationGrade! : AccommodationGrade
  user : User | null;
  @Output() gradeEmit = new  EventEmitter<AccommodationGrade>;
  constructor(private authService: AuthService,
              private currentService:AccommodationCurrentService,
              private router:Router,
              private gradeClient : GradeAccommodationService,
              private dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.user = this.authService.getUser()

  }

  onDeleteGrade() {
    this.gradeClient.deleteGrade(this.accommodationGrade.id).subscribe({
      next: _=>{
        this.gradeEmit.emit(this.accommodationGrade)
      }
    })
  }

  onEditGrade() {
    this.dialog.open(EditAccommodationGradeComponent, {
      width: '400px',
      height:'300px',
      data: { accommodationGrade: this.accommodationGrade, text : 'accommodation' }
    });
  }
}
