import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { FloatingConfigurator } from '../../../components/floating-configurator/floating-configurator';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../../services/auth.service';


@Component({
  selector: 'app-login',
  imports: [RouterModule,ButtonModule, CheckboxModule, InputTextModule, PasswordModule, FormsModule, RouterModule, RippleModule, FloatingConfigurator],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  
    userName: string = '';
    password: string = '';
    checked: boolean = false;

   // authService:AuthService = Inject(AuthService);

    constructor(public router: Router,private authService:AuthService){
      
    }


    signIn(){
      this.authService.login({userName : this.userName,password : this.password}).subscribe({
        next:(v)=>{
          if(v && v.success && v.result){
            this.authService.storeToken(v.result)
            this.router.navigate(['/upload']);
          }
        },
        error:(e:any)=>{
          console.log(e)
        },
        complete:() =>{
          
        },
      })
      
    }
}
