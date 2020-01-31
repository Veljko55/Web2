import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from './user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLoggedIn = false;

  loginUrl: string = 'http://localhost:52295/oauth/token';

  constructor(private http: HttpClient,private route:Router) { }

  login(user: User): Observable<any> {
    return this.http.post<any>(this.loginUrl, `username=`+ user.username +`&password=`+ user.password + `&grant_type=password`, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).pipe(
      map(res => {
        console.log(res.access_token);

        let jwt = res.access_token; //ceo token

        let jwtData = jwt.split('.')[1] //sredisnji-ljubicasti deo
        let decodedJwtJsonData = window.atob(jwtData) //dekodujemo ga
        let decodedJwtData = JSON.parse(decodedJwtJsonData) //parsiranje u json

        let role = decodedJwtData.role //izvlacimo sta je(admin,..)

        console.log('jwtData: ' + jwtData) 
        console.log('decodedJwtJsonData: ' + decodedJwtJsonData)
        console.log('decodedJwtData: ' + decodedJwtData)
        console.log('Role ' + role)

        localStorage.setItem('jwt', jwt)//u localstorage google chroma
        localStorage.setItem('role', role);//u localstorage google chroma
        localStorage.setItem('email', user.username)//u localstorage google chroma
        this.isLoggedIn=  true;
        location.reload();
        
      }),

      catchError(this.handleError<any>('login'))
    );
  }

  logout(): void {
    this.isLoggedIn=  false;
    localStorage.removeItem('jwt');//u localstorage google chroma brisemo
    localStorage.removeItem('role');// -||-
    localStorage.removeItem('email');// -||-
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}
