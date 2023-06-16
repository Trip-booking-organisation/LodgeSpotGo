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
    return "Notification"
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
    return "assets/images/accommodation.png"
  }
}
