import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ScheduleService } from 'src/app/services/schedule.service';
import { Line } from 'src/app/models/Line';
import { ScheduleLine } from 'src/app/models/ScheduleLine';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})


export class ScheduleComponent implements OnInit {
  public typeOfDayForm: FormGroup;
  public typeOfLineForm: FormGroup;
  public lineForm: FormGroup;
  public ScheduleForm: FormGroup;

  public lines: Line[];
  public times: string;
  public parser:string[];
  public message: string;
  public empty: boolean;
  public schedules : ScheduleLine[];
  public schedule : ScheduleLine;

  selectedLine;
  selectedDay;
  selectedNumber;

  TypeLine:Array<Object> = [
    {name: "Town"},
    {name: "Suburban"},

];

TypeDay:Array<Object> = [
  {name: "Work day"},
  {name: "Weekend"},

];

  constructor( private fb: FormBuilder, private scheduleService: ScheduleService) { 
   
    this.ScheduleForm = this.fb.group({
      line: ['', Validators.required],
      day: ['', Validators.required],
      number: ['', Validators.required]

    });

    this.lines = new Array<Line>();
    this.empty = true;
  }


 

  async ngOnInit() {
    
    let typeOfLine = this.ScheduleForm.controls['line'].value;
    
    console.log(this.ScheduleForm.controls['line'].value);
    this.lines = await this.scheduleService.getScheduleLines(typeOfLine);
  }

  public async typeSelected(){
    let typeOfLine = this.ScheduleForm.controls['line'].value;
    
   
    this.lines = await this.scheduleService.getScheduleLines(typeOfLine);
    
  }

  public async ScheduleShow(){
    let typeOfLine = this.ScheduleForm.controls['line'].value;
    let typeOfDay = this.ScheduleForm.controls['day'].value;
    let Number = this.ScheduleForm.controls['number'].value;

    this.schedules = await this.scheduleService.getSchedule(typeOfLine,typeOfDay,Number);
    if(this.schedules){
      this.empty = false;
    }
    
  }

}
