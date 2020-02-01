import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Ticket, Price } from "./price";
import { retry, catchError } from 'rxjs/operators';
import { error } from "util";

@Injectable({
    providedIn: 'root',
  })
export class PriceService {
  
    registerUrl: string =  'http://localhost:52295/api/';
  
    constructor(private http: HttpClient) { }
  

    checkIn(ticket:Ticket){
      return this.http.put<any>(this.registerUrl+"Tickets/CheckIn",ticket,{ 'headers': { 'Content-type': 'application/json' }} ).pipe(
        map(res => {
        alert("Checked in!");
        }), 
        
      );
    }

    showPrices(ticket:string,user:string) : Observable<number> {
        return this.http.get<number>(this.registerUrl+"Price/GetOnePrice?ticket="+ticket+"&user="+user)
          .pipe(
            //catchError(this.handleError<Hero[]>('getHeroes', []))
          );
      }

      showPrice(ticket:string,email:string) : Observable<number> {
        return this.http.get<number>(this.registerUrl+"Price/GetPrice?ticket="+ticket+"&email="+email)
          .pipe(
            //catchError(this.handleError<Hero[]>('getHeroes', []))
          );
      }

      buyOneHour(ticket:Ticket){
        return this.http.post<any>(this.registerUrl+"Tickets/Buy?Type="+ "One-hour" +"&UserName="+ ticket.UserName, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).pipe(
          map(res => {
          }),
          //catchError(this.handleError<any>('login'))
        );
      }

      buyTicket(ticketType:string){
       
        return this.http.post<any>(this.registerUrl+"Tickets/Buy?Type="+ ticketType +"&UserName="+ localStorage.email, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).pipe(
          map(res => {
          }),
          //catchError(this.handleError<any>('login'))
        );
      }
      buyTickett(ticket:Ticket){
       
        return this.http.post<any>(this.registerUrl+"Tickets/Buy?Type="+ ticket.TypeOfTicket +"&UserName="+ localStorage.email, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).pipe(
          map(res => {
          }),
          //catchError(this.handleError<any>('login'))
        );
      }

      showTickets(): Observable<Ticket[]> {
        //user.IDtypeOfUser = 1;
        
        return this.http.get<Ticket[]>(this.registerUrl+"Tickets/Tickets")
        .pipe(
          //catchError(this.handleError<Hero[]>('getHeroes', []))
        );
      }

      showTicketsValidUser(email : string): Observable<Ticket[]> {
        //user.IDtypeOfUser = 1;
        
        return this.http.get<Ticket[]>(this.registerUrl+"Tickets/Tickets?email=" + email)
        .pipe(
          //catchError(this.handleError<Hero[]>('getHeroes', []))
        );
      }
     
  }
  