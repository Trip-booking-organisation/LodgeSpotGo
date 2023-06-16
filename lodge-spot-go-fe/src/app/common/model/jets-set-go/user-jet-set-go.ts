export class UserJetSetGo{
  id:string ='';
  role:string='';
  firstLogIn?:boolean=false;
  given_name?:string='';
  family_name?:string='';
  email?:string=''

  constructor(id: string, role: string, firstLogIn: boolean,firstName:string,family_name:string,email:string) {
    this.id = id;
    this.role = role;
    this.firstLogIn = firstLogIn;
    this.given_name=firstName;
    this.family_name = family_name;
    this.email = email;
  }
}
