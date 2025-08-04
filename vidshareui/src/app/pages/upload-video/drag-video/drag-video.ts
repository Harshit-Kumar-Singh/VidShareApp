import { Component } from '@angular/core';
import { MessageService} from 'primeng/api';
import { PrimeNG } from 'primeng/config';
import { FileUpload } from 'primeng/fileupload';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { BadgeModule } from 'primeng/badge';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ProgressBar } from 'primeng/progressbar';
import { ToastModule } from 'primeng/toast';

import { InputIcon } from 'primeng/inputicon';
import { IconField } from 'primeng/iconfield';
import { InputTextModule } from 'primeng/inputtext';
import { FormControl, FormGroup, FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { AuthService } from '../../../services/auth.service';
import { RippleModule } from 'primeng/ripple';
import { VideoUploadService } from '../../../services/videoupload.service';
import { DialogModule } from 'primeng/dialog';
import  {environment as env} from '../../../../environments/environment';

@Component({
  selector: 'app-drag-video',
  imports: [FileUpload, ButtonModule, BadgeModule, DialogModule,ProgressBar, ToastModule, HttpClientModule, CommonModule,InputIcon, IconField, InputTextModule, FormsModule,RippleModule],
  templateUrl: './drag-video.html',
  styleUrl: './drag-video.scss',
  providers:[MessageService,AuthService,VideoUploadService]
})
export class DragVideo {
  files :File[] = [];
  uploadedFiles  : File[]= [];
  showWarning: boolean = false;

  
  uploadProgress = 0;
  connection!: signalR.HubConnection;
  uploadId = crypto.randomUUID(); // generate unique ID per upload


  ngOnInit() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${env.webApiHost}/uploadHub`,{
        accessTokenFactory: () => localStorage.getItem('authToken') || '', // 👈 or your authService.getToken()
        withCredentials: false // 👈 only true if you're using cookies; false if using Bearer token
      })
      .build();

    this.connection.on('ReceiveProgress', (percent: number) => {
      this.uploadProgress = percent;
      console.log(this.uploadProgress);
    });

    this.connection.start().then(() => {
      this.connection.invoke('JoinGroup', this.uploadId);
    });
  }




    totalSize : number = 0;




    constructor(private config: PrimeNG, 
      private messageService: MessageService,
      private http:HttpClient,
      private authService:AuthService,
      private videoUploadService : VideoUploadService
      ) {}

    choose(event : any, callback : any) {
      console.log(event,callback);
        callback();
    }

    onRemoveTemplatingFile(event : any, file : any, removeFileCallback : any, index : any) {
        
      this.uploadProgress = 0;
//this.files = [];

        console.log(event);
    }

    title = '';
    removeFiles(callback:any){
      this.files = [];
      this.uploadedFiles = [];
      this.uploadProgress = 0;
      this.title = ''
      this.downloadUrls.reset();
      this.showWarning = false;
      callback();
    }

    onClearTemplatingUpload(clear : any) {
       // clear();
        this.totalSize = 0;
        this.uploadProgress = 0;
    }

    onTemplatedUpload() {
        
    }

    downloadUrls = new FormGroup({
      _rawVideoUrl : new FormControl(''),
      _480VideoUrl : new FormControl(''),
      _720VideoUrl : new FormControl(''),
      _1O80VideoUrl : new FormControl('')
    });
  
    onSelectedFiles(event : any) {
        console.log(event);
        this.files = [];
          this.uploadedFiles = [];
          this.uploadProgress = 0;
          
        const file = event.currentFiles[0];
        this.files.push(file);
    }
    uploadEvent(){
      if(!this.title){
        this.showWarning = true;
        return;
      }
       const formData = new FormData();
        formData.append('mediaFile', this.files[0]);
        formData.append('uploadId', this.uploadId); // pass to backend
        formData.append('title',this.title);
        
        this.videoUploadService.uploadVideo(formData).subscribe({
        next: (res : any) => {
          
          this.uploadedFiles.push(this.files[0]);
          this.files = [];
          console.log('Uploaded!');
          if(res.success){
            this.downloadUrls.get('_rawVideoUrl')?.setValue(res.result.downloadUrlRaw);
            console.log(this.downloadUrls.get('_rawVideoUrl')?.value)
            this.keyid = res.result.keyId;
            this.updateDownloadUrls()
          }
          
        },
        error: (err) => console.error(err)
        });
    }

    get getrawUrl(){
      return this.downloadUrls.get('_rawVideoUrl')?.value;
    }

    get get480pUrl(){
      return this.downloadUrls.get('_480VideoUrl')?.value;
    }

    // uploadEvent(callback : any) {
    //   console.log(callback);
    //     callback();
    // }

    copyToClipboard(text: string) {
      navigator.clipboard.writeText(text).then(() => {
        // Optional: Show a toast or message
        this.messageService.add({ severity: 'success', summary: 'Copied', detail: 'Download Link Copied'});
        console.log('Copied:', text);
      });

    }
    keyid : string = '';
    timeOut:any;
    updateDownloadUrls(){
      let flag =  false;
      this.timeOut = setInterval(()=>{
          this.videoUploadService.getDownloadUrls(this.keyid).subscribe({
            next:(res:any)=>{
              if(res.success){
                flag = true;
                this.downloadUrls.get('_480VideoUrl')?.setValue(res.result.downloadUrl480);
              }
            },
            error:(er)=>{

            }
          })
          if(flag){
            clearInterval(this.timeOut)
          }
        
      },5000)
    }

    formatSize(bytes : any) {
        const k = 1024;
        const dm = 3;
        const sizes = this.config.translation.fileSizeTypes;
        if (bytes === 0 && sizes) {
            return `0 ${sizes[0]}`;
        }

        const i = Math.floor(Math.log(bytes) / Math.log(k));
        const formattedSize = parseFloat((bytes / Math.pow(k, i)).toFixed(dm));

        return `${formattedSize} ${sizes ? sizes[i] : '0'}`;
    } 

}
