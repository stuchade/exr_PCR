import { Component } from '@angular/core';
import { Map } from '../map/map';
import { Logs } from '../logs/logs';
import { Upload } from '../upload/upload';
import { PointDetail } from '../point-detail/point-detail';

@Component({
  imports: [Map, Logs, Upload, PointDetail],
  selector: 'app-dashboard',
  standalone: true,
  styleUrl: './dashboard.css',
  templateUrl: './dashboard.html',
})
export class Dashboard {
  isUploadOpen: boolean = false;
  selectedPoint: any = null;
  selectedId?: number;
}
