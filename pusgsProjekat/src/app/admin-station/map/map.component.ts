import { Component, OnInit, Input, NgZone } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MarkerInfo } from './model/marker-info.model';

import { Polyline } from './model/polyline';


import { Station } from './model/station';
import { MapService } from './mapService';



@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}'] //postavljamo sirinu i visinu mape
})
export class MapComponent implements OnInit {

  public message1:string;
  markerInfo: MarkerInfo;
  public polyline: Polyline;
  public zoom: number;
  public stationObj:Station;
  public message:string;
  public stations:Station[];
  public isLoaded:boolean;
  public canEdit:boolean;
  public statin:Station;
public name:string;

  station = this.fb.group({
    Name: ['', Validators.required],
    Address: ['', Validators.required],
    Latitude: ['', Validators.required],
    Longitude: ['', Validators.required]
  });

  stationForm = this.fb.group({
    Address: ['', Validators.required],
    Latitude: ['', Validators.required],
    Longitude: ['', Validators.required]
  });

  ngOnInit() {
    
  }

  constructor(private ngZone: NgZone,private fb: FormBuilder,public mapService: MapService){
    this.isLoaded=false;
    this.canEdit=false;
    this.getStations();
    this.message="";
    this.message1="";
  }




  // placeMarker($event){
  //   if(this.canEdit){
  //     this.stationForm.controls['Latitude'].setValue($event.coords.lat);
  //     this.stationForm.controls['Longitude'].setValue($event.coords.lng);
  //   }
  //   this.polyline.removeLocation();
  //   this.polyline.addLocation(new GeoLocation($event.coords.lat, $event.coords.lng))
  //   if(this.stationObj==null)
  //   this.stationObj = new Station();
  //   this.stationObj.Latitude = $event.coords.lat;
  //   this.stationObj.Longitude = $event.coords.lng;
  //  // this.message="Now it's good :D";
  // }

  addStation():void{
    this.isLoaded=true;
    if( this.station.controls['Name'].value=="" || this.station.controls['Address'].value=="" || this.station.controls['Longitude'].value=="" || this.station.controls['Latitude'].value==""){
      this.message1 = "All fields must be filled";
    }else{
      if(this.stationObj==null){
        this.stationObj = new Station();
      }
      this.stationObj.Name = this.station.controls['Name'].value;
      this.stationObj.Address = this.station.controls['Address'].value;
      this.stationObj.X = this.station.controls['Longitude'].value;
      this.stationObj.Y = this.station.controls['Latitude'].value;
        if(this.stationObj.X==null || this.stationObj.Y==null){
          this.message1 = "Pick location!";
        }else{ 
          this.mapService.add(this.stationObj).subscribe(ok=>{
          this.getStations();
          this.canEdit=false;
          this.message1 = "";
        })
          this.station.controls['Address'].setValue("");
          this.station.controls['Name'].setValue("");
          this.station.controls['Longitude'].setValue("");
          this.station.controls['Latitude'].setValue("");
         
      }
   }
  }


  getStations():void{
    this.mapService.getStations().subscribe(stations=>{
     this.stations = stations;
     this.isLoaded=true;
    }); 
   }


   Update(){
    this.statin.Address =this.stationForm.controls['Address'].value;
    this.statin.X =this.stationForm.controls['Latitude'].value;
    this.statin.Y =this.stationForm.controls['Longitude'].value;
      if(this.statin.X==null || this.statin.Y==null){
          this.message = "Please type only numbers or choose from map";
      }else{
        this.message = "";

    this.mapService.update(this.statin).subscribe(ok=>{
     //this.getStations();
     this.canEdit=false;
   });
  }
   }

   Edit(name){
    this.name=name;
    this.stations.forEach(elem => {
      if(elem.Name==name){
        this.statin=elem;
      }
    });
    this.stationForm.controls['Address'].setValue(this.statin.Address);
    this.stationForm.controls['Latitude'].setValue(this.statin.X);
    this.stationForm.controls['Longitude'].setValue(this.statin.Y);
    this.canEdit=true;
   }

   Delete(name){
     this.canEdit=false;
     this.mapService.delete(name).subscribe(data=>
      {console.log(data);
      this.getStations();
      if(this.stations.length==0){
        this.isLoaded=false;
      }
     }
      );
   }


}
