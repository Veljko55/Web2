import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { RegistrateUser } from "../register/registrationUser";


@Injectable({
    providedIn: 'root',
  })
export class ValidateService {
  
    registerUrl: string =  'http://localhost:52295/api/Account/';
    constructor(private http: HttpClient,private route:Router) { }
  
    getUsers(): Observable<RegistrateUser[]> {
      //user.IDtypeOfUser = 1;
      
      return this.http.get<RegistrateUser[]>(this.registerUrl+"GetUsers")
      .pipe(
        //catchError(this.handleError<Hero[]>('getHeroes', []))
      );
    }
  
 
    valiadte(user:RegistrateUser, validateOperation:string ){
        if(validateOperation == "Rejected")
        {
          user.State = 2;
        }
        else if(validateOperation == "Accepted")
        {
          user.State = 1;
        }

        return this.http.put<string>(this.registerUrl+"Validate",user,{ 'headers': { 'Content-type': 'application/json' }} ).pipe(
            map(res => {
            alert("Successfully validate!");
            }),
            catchError(this.handleError<any>('login'))
          );
    }



    private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {
        alert("Greska");
        return of(result as T);
      };
    }
  }
  