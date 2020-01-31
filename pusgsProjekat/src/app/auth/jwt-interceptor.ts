import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor() { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        console.log("Intercepted");
        console.log("Token : ", localStorage.jwt);

        // add authorization header with jwt token if available
        let jwt = localStorage.jwt;
        if (jwt) {

            console.log(request)
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${jwt}`
                }
            });
            console.log(request)
        }

        return next.handle(request);
    }
}