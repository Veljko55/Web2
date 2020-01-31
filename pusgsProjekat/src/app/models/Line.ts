import { Station } from "../admin-station/map/model/station";

export class Line{
    Number: string;
    IDtypeOfLine: number;
    TypeOfLine: string;
    Stations: Station[];
    Version:number;
}