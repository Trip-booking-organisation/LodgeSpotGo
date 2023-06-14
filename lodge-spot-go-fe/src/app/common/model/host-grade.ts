import {Grader} from "./grader";

export interface HostGrade{
  id : string,
  guestId : string,
  number : number
  hostId: Grader,
  date: Date
}

export interface HostGrades{
  hostGrades:HostGrade[]
}
