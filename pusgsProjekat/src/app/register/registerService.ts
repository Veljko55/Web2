import { HttpClient } from "@angular/common/http";
import { RegistrateUser } from "./registrationUser";
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ImageMoj } from "./slika";


@Injectable({
    providedIn: 'root',
  })
export class RegisterService {
    isLoggedIn = false;
  
    registerUrl: string =  'http://localhost:52295/api/Account/Register';
    registerUrl1: string =  'http://localhost:52295/api/Account/PostImage';

    constructor(private http: HttpClient,private route:Router) { }
  
    registrate(user: RegistrateUser): Observable<any> {
      //user.IDtypeOfUser = 1;
      return this.http.post<any>(this.registerUrl, user, { 'headers': { 'Content-type': 'application/json' } }).pipe(
        map(res => {
        this.route.navigate(['/login']);
        }),
  
        catchError(this.handleError<any>('login'))
      );
    }
  
 

    posaljiSliku(slika: ImageMoj): Observable<any> {
      //user.IDtypeOfUser = 1;
      alert("uso");
      //alert(fd);
      
      
      return this.http.post<any>(this.registerUrl1,slika, { 'headers': { 'Content-type': 'application/json' } }).pipe(
        map(res => {
        alert("Uspesno");
        }),
  
        catchError(this.handleError<any>('login'))
      );
    }

    private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {
        if(error.error.Message!=undefined){
        }else{
        }
        return of(error.error.Message);
      };
    }
  }
  