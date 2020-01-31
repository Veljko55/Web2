import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PriceListLine } from '../models/PriceListLine';

@Injectable({
  providedIn: 'root'
})
export class PriceListAdminService {

  private baseUrl: string;

  constructor(private client: HttpClient) { 
    this.baseUrl = "http://localhost:52295/";
  }

  public getPriceList() : Promise<PriceListLine[]>{
    return this.client.get<PriceListLine[]>(this.baseUrl+"api/PriceList/GetPriceListAdmin").toPromise<PriceListLine[]>();
    
   }

   public addPriceListLine(priceListLine :PriceListLine){
     return this.client.post<any>(this.baseUrl+"api/PriceList/PostPriceListLine",priceListLine, {'headers': {'Content-type': 'application/json'}});
   }

   public editLine(priceListLine: PriceListLine){
    return this.client.post<any>(this.baseUrl+"api/PriceList/EditPriceListLine",priceListLine, {'headers': {'Content-type': 'application/json'}});
   
   // return this.client.post<any>(this.baseUrl+"api/PriceList/EditLine",priceListLine, {'headers': {'Content-type': 'application/json'}});
  }

  public deleteLine(priceListLine: PriceListLine){

    return this.client.delete(`http://localhost:52295/api/PriceList/DeleteLine/${priceListLine.IDPriceList}/${priceListLine.IDPrice}`);
  
    }
}
