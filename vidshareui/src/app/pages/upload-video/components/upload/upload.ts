import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { FileUploadModule } from 'primeng/fileupload';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';


@Component({
  selector: 'app-upload',
  imports: [ButtonModule,CommonModule,FormsModule, FileUploadModule,
    ProgressBarModule,
    InputTextModule,
    ],
  templateUrl: './upload.html',
  styleUrl: './upload.scss'
})
export class Upload {
  uploading = false;
  progress = 0;
  file: File | null = null;

  videoTitle = '';
  videoDescription = '';

  onSelectFile(event: any) {
    const file = event.files?.[0];
    if (file) {
      this.file = file;
      this.startUpload();
    }
  }

  startUpload() {
    this.uploading = true;
    this.progress = 0;

    // Fake upload progress simulation
    const interval = setInterval(() => {
      this.progress += 10;
      if (this.progress >= 100) {
        clearInterval(interval);
        this.uploading = false;
      }
    }, 300);
  }

  cancelUpload() {
    this.uploading = false;
    this.progress = 0;
    this.file = null;
  }
  onUpload(){
    
  }
}
