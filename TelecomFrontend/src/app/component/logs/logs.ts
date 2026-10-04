import { Component, inject, output, signal } from '@angular/core';
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
export class Logs {
  private http = inject(HttpClient);

  buttons = signal<LogButton[]>([]);
  openUpload = output<void>();

  ngOnInit(): void {
    this.loadLogsFromBackend();
  }

  reloadLogs(): void {
    this.loadLogsFromBackend();
  }

  private loadLogsFromBackend(): void {
    this.http.get<any[]>(`http://localhost:5065/api/logs`).subscribe({
      next: (logs) => {
        if (!logs || logs.length === 0) {
          console.info("No logs!")
          this.buttons.set([]);
          return;
        }
        const newButtons = logs.map((log) => ({
          text: log.tripName,
          action: () => this.handleButtonClick(log.id)
        }));

        this.buttons.set(newButtons);
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
