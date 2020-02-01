export class Ticket {
   IdTicket:number;
   From:Date;
   CheckIn:Date;
   Type:number;
   UserName:string;
   To : Date;
   IsChecked : boolean;
   TypeOfTicket:number;
  }
  

  export class Price {
  IDPrice:number;
  IDtypeOfTicket:number;
  Value:number;
  }
  
