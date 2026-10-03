import { Component, AfterViewInit, inject, input, effect, output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as L from 'leaflet';

@Component({
  selector: 'app-map',
  standalone: true,
  templateUrl: './map.html',
  styleUrl: './map.css'
})
export class Map implements AfterViewInit {
  tripId = input<number>();
  point = output<any>();

  constructor() {
    effect(() => {
      const id = this.tripId();
      if (id) {
        this.loadTripPointsFromBackend(id);
      }
    });
  }


  private map: L.Map | undefined;
  private http = inject(HttpClient);

  private tripLayer = L.layerGroup();

  private defaultLat = 50.0755;
  private defaultLon = 14.4378

  
  private initMap(): void {

    const defaultIcon = L.icon({
      iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
    });

    // Set the default marker icon
    L.Marker.prototype.options.icon = defaultIcon;

    const mapContainer = document.getElementById('map');
    if (!mapContainer) {
      console.error('Map container not found! Make sure <div id="map"></div> exists in the DOM.');
      return;
    }

    // Create map centered at a default location
    this.map = L.map('map', {zoomControl: false}).setView([this.defaultLat, this.defaultLon], 10);

    L.control.zoom({
      position: 'bottomright'
    }).addTo(this.map);

    // Add OpenStreetMap tiles
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    }).addTo(this.map);

    this.tripLayer.addTo(this.map);
  }

  public loadTripPointsFromBackend(tripId: number): void {
    this.http.get<any[]>(`http://localhost:5065/api/logs/${tripId}/points`).subscribe({
      next: (points) => {
        this.tripLayer.clearLayers();

        if (!points || points.length === 0) {
          console.info("No point!")
          return;
        }


        const pathCoordinates: L.LatLngExpression[] = [];

        points.forEach((point) => {
          pathCoordinates.push([point.lat, point.lon]);

          L.circleMarker([point.lat, point.lon], {
            radius: 6,
            color: '#0f1c54',
            fillColor: '#2d2878',
            fillOpacity: 0.9,
            weight: 2
          }).addTo(this.tripLayer)
          .bindPopup(`<b>Cell ID:</b> ${point.cellId ?? 'Neznámé'}<br><b>Rychlost:</b> ${point.speed ?? 0} m/s`)
          .on('click', () => {
            this.point.emit(point);
          });
        });

        L.polyline(pathCoordinates, { color: '#9b38d4', weight: 3, interactive: false, opacity: 0.8}).addTo(this.tripLayer);

        this.map!.fitBounds(L.latLngBounds(pathCoordinates));
      },
      error: (err) => {
        console.error('Error uploading points from backend:', err);
      }
    });
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.initMap();
    });
  }
  
}