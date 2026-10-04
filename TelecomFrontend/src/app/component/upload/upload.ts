import { Component, inject, output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-upload',
  styleUrl: './upload.css',
  templateUrl: './upload.html',
  standalone: true,
  imports: [MatButtonModule, MatDividerModule, MatIconModule],
})
export class Upload {
  private http = inject(HttpClient);

  uploadCompleted = output<void>();

  fileName1 = '';
  fileName2 = '';

 // DO HERE ERROR CASE IF REALLY DIFFERENT FILES
  onFileSelected(event: Event): void {
    const formData = new FormData();
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length == 1) {
      const file1 = input.files[0];
      this.fileName1 = file1.name;
      console.log('One file missing!');
    } else if (input.files && input.files.length == 2) {
      var file1 = input.files[0];
      var file2 = input.files[1];
      this.fileName1 = file1.name;
      this.fileName2 = file2.name;

      const csvFile = file1.name.toLowerCase().endsWith('.csv') ? file1 : file2;
      const gpxFile = file1.name.toLowerCase().endsWith('.gpx') ? file1 : file2;
      
      formData.append('CSVFile', csvFile);
      formData.append('GPXFile', gpxFile);

      this.http.post<any[]>(`http://localhost:5065/api/logs/upload`, formData).subscribe({
        next: response => {
          console.log('The trip was uploaded successfully:', response);
          this.uploadCompleted.emit();
        },
        error: error => {
          console.error('Error uloading:', error);
        }
      });
    }
  }
}
