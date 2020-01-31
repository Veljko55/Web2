import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";


@Injectable({
    providedIn: 'root',
  })
export class ValidateTicketService {
  
    registerUrl: string =  'http://localhost:52295/api/Tickets/';
    constructor(private http: HttpClient,private route:Router) { }
  
    getInfo(id:number): Observable<string>{
        return this.http.get<string>(this.registerUrl+"CheckValidation?id="+id)
        .pipe(
          //catchError(this.handleError<Hero[]>('getHeroes', []))
        );}

    private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {
        alert("Greska");
        return of(result as T);
      };
    }
  }