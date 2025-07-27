import { Component } from '@angular/core';
import { MessageService} from 'primeng/api';
import { PrimeNG } from 'primeng/config';
import { FileUpload } from 'primeng/fileupload';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { BadgeModule } from 'primeng/badge';
import { HttpClientModule } from '@angular/common/http';
import { ProgressBar } from 'primeng/progressbar';
import { ToastModule } from 'primeng/toast';

import { InputIcon } from 'primeng/inputicon';
import { IconField } from 'primeng/iconfield';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-drag-video',
  imports: [FileUpload, ButtonModule, BadgeModule, ProgressBar, ToastModule, HttpClientModule, CommonModule,InputIcon, IconField, InputTextModule, FormsModule],
  templateUrl: './drag-video.html',
  styleUrl: './drag-video.scss',
  providers:[MessageService]
})
export class DragVideo {


  files = [];

    totalSize : number = 0;

    totalSizePercent : number = 0;


    constructor(private config: PrimeNG, private messageService: MessageService) {}

    choose(event : any, callback : any) {
        callback();
    }

    onRemoveTemplatingFile(event : any, file : any, removeFileCallback : any, index : any) {
        removeFileCallback(event, index);
        this.totalSize -= parseInt(this.formatSize(file.size));
        this.totalSizePercent = this.totalSize / 10;
    }

    onClearTemplatingUpload(clear : any) {
        clear();
        this.totalSize = 0;
        this.totalSizePercent = 0;
    }

    onTemplatedUpload() {
        console.log('Template upload triggered')
        this.messageService.add({ severity: 'info', summary: 'Success', detail: 'File Uploaded', life: 3000 });
    }

    onSelectedFiles(event : any) {
        console.log(this.files,event);
        this.files = event.currentFiles;
        this.files.forEach((file : File) => {
           this.totalSize += parseInt(this.formatSize(file.size));
        });
        this.totalSizePercent = this.totalSize / 10;
    }

    uploadEvent(callback : any) {
        callback();
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
