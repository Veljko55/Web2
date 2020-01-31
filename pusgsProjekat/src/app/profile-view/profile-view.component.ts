import { Component, OnInit } from '@angular/core';
import { PriceService } from '../price-list/priceService';
import { ProfileService } from './profileService';
import { RegistrateUser } from '../register/registrationUser';
import { Validators, FormBuilder } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrls: ['./profile-view.component.css']
})
export class ProfileViewComponent implements OnInit {
  regUser:RegistrateUser;

  ok:string;
  mySrc;
  public imagePath;
  isRegular:boolean;

  message:string;

  updateUserForm = this.fb.group({
    Email: ['', Validators.email],
    LastName: ['', Validators.required],
    Address: ['', Validators.required],
    Name: ['', Validators.required],
  });


  

  constructor(public profileService: ProfileService,private fb: FormBuilder) {
    this.showProfile();
   }

  ngOnInit() {
  }
/////////////////////////////////////////////////
  showProfile():void{
    this.profileService.showProfile(localStorage.email).subscribe(regUser=>{
       this.regUser=regUser;
       this.regUser.ConfirmPassword = localStorage.email;
       this.updateUserForm.controls['Name'].setValue(this.regUser.Name);
       this.updateUserForm.controls['LastName'].setValue(this.regUser.LastName);
       this.updateUserForm.controls['Address'].setValue(this.regUser.Address);
       this.updateUserForm.controls['Email'].setValue(this.regUser.Email);
       this.mySrc ="data:image/png;base64," + this.regUser.Picture;
       console.log(this.regUser.PassengerType); //ove sam promenio
       if(this.regUser.PassengerType === 3){
       this.isRegular=false;
        } else
       this.isRegular=true;

    });
  }

  update(){
    this.regUser.Name =this.updateUserForm.controls['Name'].value;
    this.regUser.LastName =this.updateUserForm.controls['LastName'].value;
    this.regUser.Address =this.updateUserForm.controls['Address'].value;
    this.regUser.Email =this.updateUserForm.controls['Email'].value;
    this.regUser.Picture = this.base64textString;
    this.profileService.update(this.regUser).subscribe(ok=>{
      this.message=ok;
      if(this.message=="ok")
      this.showProfile();
   });

  }

  private base64textString:string="";
  
  handleFileSelect(evt){
      var files = evt.target.files;
      var file = files[0];
    
    if (files && file) {
        var reader = new FileReader();

        reader.onload =this._handleReaderLoaded.bind(this);

        reader.readAsBinaryString(file);
    }
  }
  
  _handleReaderLoaded(readerEvt) {
     var binaryString = readerEvt.target.result;
            this.base64textString= btoa(binaryString);
            this.mySrc="data:image/png;base64," + this.base64textString;
    }




}
