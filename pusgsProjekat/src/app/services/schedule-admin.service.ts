import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ScheduleLine } from '../models/ScheduleLine';
import { Line } from '../models/Line';
import { catchError, map } from 'rxjs/operators';
import { stringify } from '@angular/compiler/src/util';

@Injectable({
  providedIn: 'root'
})
export class ScheduleAdminService {

  private baseUrl: string;
  constructor(private client: HttpClient) {
    this.baseUrl = "http://localhost:52295/";
   }

   public getSchedule() : Promise<ScheduleLine[]>{
    return this.client.get<ScheduleLine[]>(this.baseUrl+"api/Line/GetScheduleAdmin").toPromise<ScheduleLine[]>();
    
 
   }

   public getLines() : Promise<Line[]> {
    return this.client.get<Line[]>(this.baseUrl+"api/Line/GetLines").toPromise<Line[]>();
  }

  public postSchedule(sl :ScheduleLine){
   // this.client.post<any>(this.baseUrl+"api/Line/PostLineSchedule","");
    return this.client.post<any>(this.baseUrl+"api/Schedule/PostLineSchedule",sl, {'headers': {'Content-type': 'application/json'}});
  }

  public deleteLine(scheduleLine: ScheduleLine){

  return this.client.delete(`http://localhost:52295/api/Schedule/DeleteLineSchedule/${scheduleLine.IDDay}`);  

  }

  public editLine(sl: ScheduleLine){
    return this.client.post<any>(this.baseUrl+"api/Schedule/EditLineSchedule",sl, {'headers': {'Content-type': 'application/json'}});
  }
}
