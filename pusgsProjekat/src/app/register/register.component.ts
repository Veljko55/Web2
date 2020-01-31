import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from './registerService';
import { ImageMoj } from './slika';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  fd:FormData;
  mySrc;
  canUpload:boolean;
  message:string;

  registerForm = this.fb.group({
    Email: ['', Validators.required],
    Password: ['', Validators.required],
    ConfirmPassword: ['', Validators.required],
    Name: ['', Validators.required],
    LastName: ['', Validators.required],
    BirthdayDate:['',Validators.required],
    Address: ['', Validators.required],
    Picture: [''],
    PassengerType:[],
    State:[''],

  });

  constructor(public registerService: RegisterService, private fb: FormBuilder) {
    this.canUpload=false;
    this.message="";
  }
  
  hit(){
    this.canUpload=true;
  }  
  unHit(){
    this.canUpload=false;
  }
 register(){
    //this.router.navigate(["/home"]);

    if(this.registerForm.controls['Password'].value == this.registerForm.controls['ConfirmPassword'].value){
      if(this.registerForm.controls['PassengerType'].value!=null){
        this.message="";
        if(this.registerForm.controls['Picture'].value != ""){    
          this.registerForm.controls['Picture'].setValue(this.base64textString);
        }else{
          this.registerForm.controls['Picture'].setValue("nema slike");
        }
        this.registerForm.controls['State'].setValue(0);
        //this.registerForm.controls['BirthdayDate'].setValue(this.registerForm.controls['BirthdayDate'].value);

        this.registerService.registrate(this.registerForm.value).subscribe((data) => {
          this.message = data;
          
        });
        //this.router.navigate(["/login"]);
      }else{
        this.message="Please tell us what you are..";

      }
    }else{
      this.message="Passwords does not match.";
    }
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
            alert(btoa(binaryString));
            this.mySrc="data:image/png;base64," + this.base64textString;
    }


}
