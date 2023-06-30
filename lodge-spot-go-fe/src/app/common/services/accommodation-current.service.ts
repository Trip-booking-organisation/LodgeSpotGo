import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {IAccommodationDto} from "../model/accommodation-dto";

@Injectable({
  providedIn: 'root'
})
export class AccommodationCurrentService {

  private _accommodation: IAccommodationDto
  private _accommodation$: BehaviorSubject<IAccommodationDto> = new BehaviorSubject<IAccommodationDto>(null)
  constructor() { }

  get accommodation(): IAccommodationDto {
    this.retrieveAccommodation();
    return this._accommodation;
  }

  private retrieveAccommodation() {
    if (!this._accommodation) {
      this._accommodation = JSON.parse(sessionStorage.getItem('accommodation'))
      this._accommodation$.next(this._accommodation)
    }
  }

  get accommodation$(): Observable<IAccommodationDto> {
    this.retrieveAccommodation();
    return this._accommodation$ as Observable<IAccommodationDto>;
  }
  set accommodation(value: IAccommodationDto) {
    sessionStorage.setItem('accommodation', JSON.stringify(value))
    this._accommodation$.next(value);
    this._accommodation = value;
  }
}
