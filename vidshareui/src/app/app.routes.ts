import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Landing } from './pages/landing/landing';
import { Register } from './pages/auth/register/register';
import { UploadVideo } from './pages/upload-video/upload-video';

export const routes: Routes = [

    {
        path : 'login',component : Login
    },
    {
        path : 'register', component : Register
    },
    {
        path : '', component : Landing
    },
    {
        path : 'upload', component : UploadVideo
    }
];
