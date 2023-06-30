import {Component, Input} from '@angular/core';
import {NotificationResponse} from "../../../shered/model/NotificationResponse";

@Component({
  selector: 'app-notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.scss']
})
export class NotificationCardComponent {
  @Input() notification: NotificationResponse;
  bindInput(){
    if(this.notification.type === "CreatedReservation"){
      return "Created reservation"
    }
    if(this.notification.type === "CanceledReservation"){
      return "Canceled reservation"
    }
    if(this.notification.type === 'CreatedAccommodationGrade'){
      return "Accommodation graded"
    }
    if(this.notification.type === 'CreatedHostGrade'){
      return "You are graded"
    }
    if(this.notification.type === 'LoseOutstandingHostStatus'){
      return "Lost status"
    }
    if(this.notification.type === 'BecomeOutstandingHost'){
      return "Earned status"
    }
    return this.notification.type
  }
  chooseIcon() {
    if(this.notification.type === "CreatedReservation"){
      return "assets/images/booking.png"
    }
    if(this.notification.type === "CanceledReservation"){
      return "assets/images/cancelled.png"
    }
    if(this.notification.type === 'CreatedAccommodationGrade') {
      return "assets/images/best.png"
    }
    if(this.notification.type === 'CreatedHostGrade'){
      return "assets/images/host-grade.png"
    }
    if(this.notification.type === 'LoseOutstandingHostStatus'){
      return "assets/images/out-lose.png"
    }
    if(this.notification.type === 'BecomeOutstandingHost'){
      return "assets/images/out-get.png"
    }
    if(this.notification.type === 'Reservation Refused'){
      return "assets/images/refused.png"
    }
    if(this.notification.type === 'Reservation Confirmed'){
      return "assets/images/verified.png"
    }
    return "assets/images/battery.png"
  }
}
