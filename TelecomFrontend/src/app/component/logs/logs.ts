import { Component, inject, OnInit, output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';

interface LogButton {
  text: string;
  action: () => void;
}

@Component({
  selector: 'app-logs',
  styleUrl: './logs.css',
  templateUrl: './logs.html',
  standalone: true,
  imports: [MatButtonModule, MatDividerModule, MatIconModule],
})
export class Logs implements OnInit {
  private http = inject(HttpClient);
  buttons: LogButton[] = [];

  ngOnInit(): void {
      this.loadLogsFromBackend();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const selectedFile = input.files[0];
      console.log('Selected file:', selectedFile.name);
    }
  }

  private showLogs(logs: any[]): void {
    
  }

  private loadLogsFromBackend(): void {
    this.buttons = [];
    this.http.get<any[]>(`http://localhost:5065/api/logs`).subscribe({
      next: (logs) => {
        if (!logs || logs.length === 0) {
          console.info("No logs!")
        }
        logs.forEach((log) => {
          this.buttons.push({
            text: log.tripName,
            action: () => this.handleButtonClick(log.id)
          });
        });
        
      },
      error: (err) => {
        console.error('Error uploading logs from backend:', err);
      }
    })
  }

  tripSelected = output<number>();

  handleButtonClick(tripId: number): void {
    console.log(`Kliknuto na tlačítko: ${tripId}`);
    this.tripSelected.emit(tripId);
  }
}
