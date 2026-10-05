import { Component, input, output, effect } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

interface LogPoint {
  id: number;
  tripId: number;
  measuredAtGps: string | Date;
  lat: number;
  lon: number;
  altitude: number | null;
  speed: number | null;
  mcc: number | null;
  mnc: number | null;
  cellId: number | null;
  dbm: number | null;
  ta: number | null;
  accuracy: number | null;
  bearing: number | null;
  measuredAtBts: string | Date | null;
  netType: string | null;
}

@Component({
  imports: [MatButtonModule, MatIcon],
  selector: 'app-point-detail',
  styleUrl: './point-detail.css',
  templateUrl: './point-detail.html',
})
export class PointDetail {
  pnt = input<LogPoint>();
  closeDetail = output<void>();

  logPoint: LogPoint | null = null;

  constructor() {
    effect(() => {
      const point = this.pnt();
      if (point) {
        this.logPoint = point;
      }
    });
  }
  

}
