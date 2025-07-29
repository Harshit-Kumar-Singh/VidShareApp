import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { catchError, EMPTY, Observable, throwError } from "rxjs";
import { HttpResponse } from "@microsoft/signalr";
import { Router } from "@angular/router";

@Injectable()
export class JwtInterceptor implements HttpInterceptor{
    constructor(private authService:AuthService,private router:Router){

    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authService.getToken();
        let clonedReq = req;
        clonedReq =  clonedReq
        if (token) {
            console.log(token)
            clonedReq = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }
        return next.handle(clonedReq).pipe(
            catchError((err:HttpErrorResponse)=>{
                console.log(err);
                if(err.status === 401){
                    this.authService.logout();
                    window.location.href = '/login'
                }
                return throwError(() => err); 
            })
        );
    }
}