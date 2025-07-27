import { Component } from '@angular/core';
import { Upload } from "./components/upload/upload";
import { AppTopbar } from '../../components/app.topbar';
import { DragVideo } from "./drag-video/drag-video";

@Component({
  selector: 'app-upload-video',
  imports: [Upload, AppTopbar, DragVideo],
  templateUrl: './upload-video.html',
  styleUrl: './upload-video.scss'
})
export class UploadVideo {


}
