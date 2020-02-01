import { Component, OnInit } from '@angular/core';
import { PriceService } from './priceService';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { Ticket } from './price';
import { ProfileService } from '../profile-view/profileService';
import { RegistrateUser } from '../register/registrationUser';

@Component({
  selector: 'app-price-list',
  templateUrl: './price-list.component.html',
  styleUrls: ['./price-list.component.css']
})
export class PriceListComponent implements OnInit {
public isPriceDataLoaded: boolean;
public isOneHour: boolean;

messageUnregistredSucced: string;
message1:string;
message:string;
selectedTicket;
selectedUser;
email;
isLogged:boolean;
prices:number;
oneHour:number;
tickets : Ticket[];
t:Ticket;

user:RegistrateUser;
  dataTicket:Array<Object> = [
    {name: "One-hour"},
    {name: "Day"},
    {name: "Mounth"},
    {name: "Year"}
];

dataUser:Array<Object> = [
  {name: "Student"},
  {name: "Pensioner"},
  {name: "Regular"}
];


dat:Date;
ticket:Ticket={IdTicket:1, From:this.dat,CheckIn:this.dat ,Type:0,UserName:"guest", To:this.dat, IsChecked : false, TypeOfTicket:1};

role;

emailForm = this.fb.group({
  email: ['', Validators.email],
});

  constructor(public priceService: PriceService,private fb: FormBuilder, public profileService : ProfileService) {
    this.isPriceDataLoaded=false;
    this.isOneHour=false;
    if(localStorage.email!=undefined){
    this.isLogged=true;
    this.getUser();
    this.showTickets();
    }else{
      this.isLogged=false;
    }
    this.message="";
    this.message1="";



  }

  ngOnInit() {
  }

  showTickets():void{
   this.priceService.showTickets().subscribe(tickets=>{
    this.tickets = tickets;
   }) 
  }

  

  showPrices(logged):void{
    if(!logged){
      if(this.selectedTicket==undefined || this.selectedUser==undefined){
        this.message1 = "You did not choose all critetiums";
             }else{
               this.message1="";
      this.priceService.showPrices(this.selectedTicket.name,this.selectedUser.name).subscribe(prices=>{
      this.prices=prices;
      this.isPriceDataLoaded=true;

      //prebaceno iz buy ticket ako nema rolu i nema buy 
      this.priceOfOneHourTicket();
      this.isOneHour=true;
    });
             }
  }else{
    if(this.selectedTicket==undefined){
      this.message1 = "You did not choose critetium";
           }else{
             this.message1="";
      this.priceService.showPrice(this.selectedTicket.name,localStorage.email).subscribe(prices=>{
      this.prices=prices;
      this.isPriceDataLoaded=true;
    });
  }
}
  }
  
  checkIn(id){
    this.t = new Ticket();
    this.t.IdTicket=id;
    this.priceService.checkIn(this.t).subscribe((date)=>{
      this.showTickets();
    });
    
  }

  getUser(){
    this.profileService.showProfile(localStorage.email).subscribe(regUser=>{
      this.user=regUser;
    });
  }

  priceOfOneHourTicket(){
    this.priceService.showPrices("One-hour","Regular").subscribe(prices=>{
      this.oneHour=prices;
      this.message = "You can buy only One-hour ticket because you are not a member. Price is "+this.oneHour+"$.";
    });
  }

  

  buyTicket(){

    this.role = localStorage.role;

    /*if(this.role==null){
      this.priceOfOneHourTicket();
      this.isOneHour=true;
    }
    else{
      
      if(this.user.State == 1){
      //this.ticket.Type=this.selectedTicket.name;
      this.priceService.buyTicket(this.selectedTicket.name).subscribe((data) => {
        console.log(data);
        this.showTickets();
      });
      }else{
        alert("Kontroler nije verifikovao Vas profil.");
      }
     
    }*/
    if(this.role==null){
      this.priceOfOneHourTicket();
      this.isOneHour=true;
    }
    else{

      if(this.user.State==1){
      this.ticket.TypeOfTicket=this.selectedTicket.name;
      this.priceService.buyTickett(this.ticket).subscribe((data) => {
        console.log(data);
        alert("Uspesno ste kupili kartu.");
        this.showTickets();
      });
      }
      else if(this.user.State==2)
      {
        alert("Kontroler je odbio vas zahtev za verifikaciju.");
      }
      else if(this.user.State==3)
      {
        alert("Kontroler je odbio vas zahtev za verifikaciju jer niste prilozili sliku.");
      }
      else{
        alert("Kontroler jos nije verifikovao Vas profil.");
      }
    }

  }

  buyOneHour(){
    //alert(this.email);
    this.message="";
    this.ticket.UserName=this.email;
    this.priceService.buyOneHour(this.ticket).subscribe((data) => {
      console.log(data);
      this.messageUnregistredSucced = "U buy ticket succesfuly"
    });
  }

lala(){
  alert(this.prices);
}

}
