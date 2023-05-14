import { Component } from '@angular/core';
import {AccomodationResult} from "../../model/AccomodationResult";
import {AccommodationResponse} from "../../../shered/model/accommodationResponse";
import {Address} from "../../../common/model/addres";
import {SpecialPrice} from "../../../shered/model/specialPrice";
import {DateRange} from "../../../shered/model/dateRange";

@Component({
  selector: 'app-reserve-accomodation-card',
  templateUrl: './reserve-accomodation-card.component.html',
  styleUrls: ['./reserve-accomodation-card.component.scss']
})
export class ReserveAccomodationCardComponent {
    from:Date = new Date()
    to:Date = new Date()
  price = "450$"


    myDateRange :DateRange= {
      from: new Date('2023-06-01'),
      to: new Date('2023-06-07')
    }

  acc: AccommodationResponse = {
    id: "123",
    name: "Cozy Cottage",
    address: {
      street: "123 Main St",
      city: "Anytown",
      country:"Serbia"
    },
    max_guests: 6,
    min_guests: 2,
    amenities: ["Free Wi-Fi", "Swimming Pool", "Pet-friendly"],
    photos: ["https://example.com/photo1.jpg", "https://example.com/photo2.jpg"],
    specialPrices: [
      {
        dateRange:this.myDateRange,
        price: 150
      },
      {
        dateRange:this.myDateRange,
        price: 750
      }
    ]
  };

  create() {

  }
}
