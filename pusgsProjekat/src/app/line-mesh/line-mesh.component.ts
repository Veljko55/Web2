import { Component, OnInit, Input, NgZone } from '@angular/core';
import { MarkerInfo } from '../admin-station/map/model/marker-info.model';
import { GeoLocation } from '../admin-station/map/model/geolocation';
import { Polyline } from '../admin-station/map/model/polyline';
import { Line } from '../models/Line';
import { ScheduleAdminService } from '../services/schedule-admin.service';
import { Station } from '../admin-station/map/model/station';
import { MapService } from '../admin-station/map/mapService';
import { stringify } from '@angular/core/src/util';


@Component({
  selector: 'app-line-mesh',
  templateUrl: './line-mesh.component.html',
  styleUrls: ['./line-mesh.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}'] //postavljamo sirinu i visinu mape
})
export class LineMeshComponent implements OnInit {

  markerInfo: MarkerInfo;
  markersInfo:Array<MarkerInfo>;

  markersInfos:Map<string,Array<MarkerInfo>>;

  public polylines1:Array<Polyline>;
  public polylines: Map<string,Polyline>;
  public zoom: number;
  linesAll: Line[];
  public polyline:Polyline;
  linijeBul:Map<string,boolean>
  stations:Station[];

  public lines = [];

  getRandomColor() {
    var color = Math.floor(0x1000000 * Math.random()).toString(16);
    return '#' + ('000000' + color).slice(-6);
  }

lala(a){
  this.polylines1 = new Array<Polyline>();
  this.markersInfo = new Array<MarkerInfo>();


  if(this.linijeBul[a]){
    this.linijeBul[a]=false;
    this.polylines[a] = null;
    this.markersInfos[a]=null;
  }
  else{
    this.markersInfos[a] = new Array<MarkerInfo>();
    this.linijeBul[a]=true;
    this.polylines[a] = new Polyline([], this.getRandomColor(), { url:"", scaledSize: {width: 30, height: 30}});
    this.mapService.getStation(a).subscribe(data=>{
      this.stations=data;
      this.stations.forEach(s=>{
        this.polylines[a].addLocation(new GeoLocation(s.X,s.Y))
        this.markersInfos[a].push(new MarkerInfo(new GeoLocation(s.X,s.Y),"","Address: "+s.Address,s.Name,""));
        for(var l in this.lines){
          if(this.markersInfos[this.lines[l]]!=null){
            for(var aa in this.markersInfos[this.lines[l]]){
                this.markersInfo.push(this.markersInfos[this.lines[l]][aa])
            }
          }
        }
    });
    });
  }
  
for(var l in this.lines){
  if(this.polylines[this.lines[l]]!=null){
    this.polylines1.push(this.polylines[this.lines[l]]);
  }
}

for(var l in this.lines){
  if(this.markersInfos[this.lines[l]]!=null){
    for(var aa in this.markersInfos[this.lines[l]]){
        this.markersInfo.push(this.markersInfos[this.lines[l]][aa])
    }
  }
}

}



  async ngOnInit() {
    this.markerInfo = new MarkerInfo(new GeoLocation(45.242268, 19.842954),"","Jugodrvo" , "" , "");

      this.polyline = new Polyline([], 'blue', { url:"assets/busicon.png", scaledSize: {width: 50, height: 50}});
      this.linesAll = await this.scheduleAdminService.getLines();
      this.linijeBul = new Map<string,boolean>();
      this.polylines1 = new Array<Polyline>();

      this.linesAll.forEach(e => {
        this.linijeBul.set(e.Number,false);
      });

      this.polylines = new Map<string,Polyline>();
      this.linesAll.forEach(v=>this.lines.push(v.Number));

      this.markersInfo = new Array<MarkerInfo>();
      this.markersInfos = new Map<string,Array<MarkerInfo>>();

    }

  constructor(private ngZone: NgZone,private scheduleAdminService: ScheduleAdminService,public mapService: MapService){

  }

  placeMarker($event){
    this.polyline.addLocation(new GeoLocation($event.coords.lat, $event.coords.lng))
    console.log(this.polyline)
  }

}
