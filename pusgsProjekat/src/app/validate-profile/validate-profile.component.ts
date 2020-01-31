import { Component, OnInit } from '@angular/core';
import { RegistrateUser } from '../register/registrationUser';
import { ValidateService } from './validateService';
import { areAllEquivalent } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-validate-profile',
  templateUrl: './validate-profile.component.html',
  styleUrls: ['./validate-profile.component.css']
})


export class ValidateProfileComponent implements OnInit {
  users:RegistrateUser[];
  areUsersThere:boolean;
  user:RegistrateUser;
  ok;
  constructor(public validateService: ValidateService) { 
      this.getUsers();
  }

  ngOnInit() {
  }

  
  
  getUsers():void{
    this.validateService.getUsers().subscribe(Users=>{
      this.users = Users;
      this.users.forEach(obj =>{obj.Picture = "data:image/png;base64,"+obj.Picture;} );
      
      if(this.users[1]!=null){
        this.areUsersThere = true;
      }else
        this.areUsersThere=false;
    });


  }
 
  validateProfile(item){
    this.user = new RegistrateUser();
    this.user.VerificationStatus="Valid";
    this.user.Email=item;
    this.validateService.valiadte(this.user, "Accepted").subscribe(ok=>{this.ok=ok;this.getUsers();});
  }

  rejectProfile(item){
    this.user = new RegistrateUser();
    this.user.VerificationStatus="Invalid";
    this.user.Email=item;
    this.validateService.valiadte(this.user, "Rejected").subscribe(ok=>{this.ok=ok;this.getUsers();});
  }
  //na ok klik ces opet da zoves getUsers da makne tog validiranog

}
