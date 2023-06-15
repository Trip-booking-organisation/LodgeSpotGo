import {Grader} from "./grader";

export interface AccommodationGradeResponse{
  id : string,
  accommodationId : string,
  number : number
  guest: Grader,
  averageGrade: number,
  date: Date
}
