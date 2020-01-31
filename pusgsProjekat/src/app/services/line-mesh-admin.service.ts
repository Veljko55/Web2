import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Line } from '../models/Line';
import { Station } from '../admin-station/map/model/station';

@Injectable({
  providedIn: 'root'
})
export class LineMeshAdminService {

  private baseUrl: string;

  constructor(private client: HttpClient) {
    this.baseUrl = "http://localhost:52295/";
   }


   public getLines() : Promise<Line[]>{
    return this.client.get<Line[]>(this.baseUrl+"api/Line/GetLines").toPromise<Line[]>();
    
   }

   public getStations() : Promise<Station[]>{
    return this.client.get<Station[]>(this.baseUrl+"api/Station/GetStations").toPromise<Station[]>();
    
   }

   public addLine(line :Line){
    return this.client.post<any>(this.baseUrl+"api/Line/AddLine",line, {'headers': {'Content-type': 'application/json'}});
  }

  public editLine(line :Line){
    return this.client.post<any>(this.baseUrl+"api/Line/EditLine",line, {'headers': {'Content-type': 'application/json'}});
  }

  public deleteLine(line :Line){

    return this.client.delete(`http://localhost:52295/api/Line/DeleteLine/${line.Number}`);
  
    }
}
