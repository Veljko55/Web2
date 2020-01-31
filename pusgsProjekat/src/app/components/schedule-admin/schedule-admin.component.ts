import { Component, OnInit } from '@angular/core';
import { ScheduleLine } from '../../models/ScheduleLine';
import { Router } from '@angular/router';
import { ScheduleAdminService } from '../../services/schedule-admin.service';
import { Line } from 'src/app/models/Line';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-schedule-admin',
  templateUrl: './schedule-admin.component.html',
  styleUrls: ['./schedule-admin.component.css']
})
export class ScheduleAdminComponent implements OnInit {
  public isBtnAddClicked: boolean;
  public isBtnEditClicked: boolean;
  schedule: ScheduleLine[];
  lines: Line[];
sl : ScheduleLine;
scheduleLine: ScheduleLine;
public editForm: FormGroup;
  public addForm: FormGroup;
  d: Date;
  TypeDay:Array<Object> = [
    {name: "Work day"},
    {name: "Weekend"},
  
  ];

  constructor(private fb: FormBuilder,private scheduleAdminService: ScheduleAdminService) {
    this.addForm = this.fb.group({
      line: ['',Validators.required],
      day: ['',Validators.required],
      time: ['',Validators.required]

    });
    this.editForm = this.fb.group({
      line: [''],
      day: ['',Validators.required],
      time: ['',Validators.required]

    });

    this.isBtnAddClicked = false;
    this.isBtnEditClicked = false;
    this.schedule = new Array<ScheduleLine>();
    this.sl = new ScheduleLine();
    
   }

  async ngOnInit() {
    this.schedule = await this.scheduleAdminService.getSchedule();
    
    
  }

  public async addScheduleLine(){
    this.isBtnAddClicked = true;
    this.lines = await this.scheduleAdminService.getLines();
  }

  public async onSubmit(){

    
    this.sl.Number = this.addForm.controls['line'].value;
    this.sl.Day= this.addForm.controls['day'].value;
    this.sl.Time = this.addForm.controls['time'].value;
    

      this.scheduleAdminService.postSchedule(this.sl).subscribe(()=>{
        this.getSchedule();
      }, err => console.log(err));


    this.isBtnAddClicked = false;
  }

  public async onSubmitEdit(){

    this.sl.Number = this.editForm.controls['line'].value;
    this.sl.Day= this.editForm.controls['day'].value;
    this.sl.Time = this.editForm.controls['time'].value;
    this.sl.IDDay = this.editForm.controls['id'].value;

    this.scheduleAdminService.editLine(this.sl).subscribe(()=>{
      this.getSchedule();
     
    }, err => console.log(err));


  this.isBtnEditClicked = false;

  }

  public async getSchedule(){
    this.schedule = await this.scheduleAdminService.getSchedule();
  }

  
  public  editLine(scheduleLine){
    let t=new Date(scheduleLine.Time).toLocaleTimeString();
    this.isBtnEditClicked = true;
    this.editForm = this.fb.group({
      id:[scheduleLine.IDDay],
      line: [scheduleLine.Number],
      day: [scheduleLine.Day],
      time: [t]

    });
  }

  public deleteLine(scheduleLine){
    this.scheduleAdminService.deleteLine(scheduleLine).subscribe(()=>{
      this.getSchedule();
     
    }, err => console.log(err));
  }

}
