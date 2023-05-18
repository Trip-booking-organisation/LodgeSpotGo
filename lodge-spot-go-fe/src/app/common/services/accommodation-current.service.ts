import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {Accommodation} from "../model/accommodation";
import {IAccommodationDto} from "../model/accommodation-dto";

@Injectable({
  providedIn: 'root'
})
export class AccommodationCurrentService {

  private _accommodation: IAccommodationDto
  private _accommodation$: BehaviorSubject<IAccommodationDto> = new BehaviorSubject<IAccommodationDto>(null)
  constructor() { }

  get accommodation(): IAccommodationDto {
    return this._accommodation;
  }
  get accommodation$(): Observable<IAccommodationDto> {
    return this._accommodation$ as Observable<IAccommodationDto>;
  }
  set accommodation(value: IAccommodationDto) {
    this._accommodation$.next(value);
    this._accommodation = value;
  }
}
