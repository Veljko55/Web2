import { Component, OnInit } from '@angular/core';
import { MapService } from './map/mapService';
import { Station } from './map/model/station';

@Component({
  selector: 'app-admin-station',
  templateUrl: './admin-station.component.html',
  styleUrls: ['./admin-station.component.css']
})
export class AdminStationComponent implements OnInit {

 

  constructor(public mapService: MapService) {
   

   }

  ngOnInit() {
  }

  
}
