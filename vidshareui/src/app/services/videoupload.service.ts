import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { environment as env } from "../../environments/environment";
import { Observable } from "rxjs";
import { ServiceResult } from "../models/serviceResult.model";
@Injectable({
    providedIn:'root'
})
export class VideoUploadService {
    private host : string = env.webApiHost;
    constructor(private http:HttpClient){

    }
    uploadVideo(formData: any): Observable<ServiceResult<any>> {
        return this.http.post<ServiceResult<any>>(`${this.host}/upload-video`, formData);
    }
    getDownloadUrls(keyId : string) : Observable<ServiceResult<any>>{
        return this.http.get<ServiceResult<any>>(`${this.host}/get-download-urls/${keyId}`)
    }
}