import {Component, Input, OnInit} from '@angular/core';
import {Flight} from "../../../../common/model/flight";
import {Seat} from "../../../../common/model/Seat";
import {JetSetGoService} from "../../../../common/services/jet-set-go.service";
import {CreateTicketsRequest} from "../../../../common/model/jets-set-go/create-ticket-request";
import {CreateTicketInfoRequest} from "../../../../common/model/jets-set-go/create-ticket-info-request";
import {AuthService} from "../../../../core/keycloak/auth.service";
import {User} from "../../../../core/keycloak/user";
import {UserJetSetGo} from "../../../../common/model/jets-set-go/user-jet-set-go";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-flights-card',
  templateUrl: './flights-card.component.html',
  styleUrls: ['./flights-card.component.scss']
})
export class FlightsCardComponent implements OnInit{
@Input() flight : Flight;
@Input() numberOfSeats : number
@Input() token : string
  price : number = 0
  bookSeats: Seat[]  = []
  seatCounter =0
  user : User
  userJetSetGo : UserJetSetGo
  constructor(private jetSetGo : JetSetGoService,
              private toast : ToastrService,
              private  authService : AuthService) {
  }
  ngOnInit(): void {
    this.parseToken();
    this.user = this.authService.getUser();
  this.seatCounter = this.numberOfSeats
  let seats = this.flight.seats.filter(x => x.available)
  seats.forEach(x => {
    if(this.seatCounter === 0)
      return;
     this.price += x.price
      this.bookSeats.push(x)
  })
    console.log(this.bookSeats)
  }

  private parseToken() {
    let user: string = atob(this.token.split('.')[1]);
    let userObject = JSON.parse(user)
    this.userJetSetGo = new UserJetSetGo(userObject.sub, userObject.role, userObject.firstLogIn, userObject.given_name,
      userObject.family_name, userObject.email);
    console.log('user', this.userJetSetGo)
  }

  onBook() {
  console.log('token',this.token)
  let newTickets : CreateTicketInfoRequest[]=[]
  this.bookSeats.forEach(x => {
    let seat : CreateTicketInfoRequest={
      seatNumber : x.seatNumber,
      contactDetails : this.user.email
    }
    newTickets.push(seat)
  })

  let tickets : CreateTicketsRequest ={
    flightId: this.flight.id,
    passengerId :this.userJetSetGo.id,
    newTickets : newTickets,
    token: this.token
  }
  console.log(tickets)
    this.jetSetGo.bookSeats(tickets).subscribe({
      next : response => {
        this.toast.success("You have successfully booked seats!");
      }
    })
  }
}
