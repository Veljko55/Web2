import { Component, OnInit } from '@angular/core';
import { Line } from 'src/app/models/Line';
import { FormGroup, FormBuilder, FormArray, FormControl, ValidatorFn, Validators } from '@angular/forms';
import { LineMeshAdminService } from '../../services/line-mesh-admin.service';
import { of } from 'rxjs';
import { Station } from 'src/app/admin-station/map/model/station';


@Component({
  selector: 'app-line-mesh-admin',
  templateUrl: './line-mesh-admin.component.html',
  styleUrls: ['./line-mesh-admin.component.css']
})
export class LineMeshAdminComponent implements OnInit {

  public isBtnAddClicked: boolean;
  public isBtnEditClicked: boolean;
  lines: Line[];
  message: string;
  messageEdit: string;
  stations: Station[];
  line: Line;
  public editForm: FormGroup;
  public addForm: FormGroup;
checkedStations: Station[];
currentStations: Station[];


  TypeLine:Array<Object> = [
    {name: "Town"},
    {name: "Suburban"}
  
  ];


  onChange(s){
    var exist = this.checkedStations.indexOf(s);
    if(exist == -1){
    this.checkedStations.push(s);
    }else{
      this.checkedStations.splice(s,1);
    }
  }

  constructor(private fb: FormBuilder,private LineMeshAdminService: LineMeshAdminService) {
   
    this.addForm = this.fb.group({
      number: ['', Validators.required],
      typeOfLine: ['', Validators.required],
      stations: [''] 

    });
    this.editForm = this.fb.group({
      number: ['', Validators.required],
      typeOfLine: ['', Validators.required],
      stations: [''] 
      
    });

    this.isBtnAddClicked = false;
    this.isBtnEditClicked = false;
    this.lines = new Array<Line>();
    this.line = new Line();
    this.message="";
    this.messageEdit="";
    this.stations = new Array<Station>();
    this.checkedStations = new Array<Station>();
    this.currentStations = new Array<Station>();
    
   }

   

  async ngOnInit() {
    this.lines = await this.LineMeshAdminService.getLines();
    this.stations = await this.LineMeshAdminService.getStations();
    this.message="";
    this.messageEdit="";
  }

  public async addLine(){
    this.isBtnAddClicked = true;
    this.stations = await this.LineMeshAdminService.getStations();
  }


  public async onSubmit(){
    this.line.Number= this.addForm.controls['number'].value;
    this.line.TypeOfLine = this.addForm.controls['typeOfLine'].value;
    this.line.Stations = this.checkedStations;


    if(this.addForm.controls['number'].value==""){
      this.message = "Must fill Number field.";
    }else if(this.addForm.controls['typeOfLine'].value==""){
      this.message = "Must chose type of line.";
    }
    else{
    
    this.LineMeshAdminService.addLine(this.line).subscribe(data=>{
      this.message=data; 
      this.getLines();
      }, err => console.log(err));
    }
    
  }

  public async getLines(){
    
    if(this.message == "ok"){
      this.isBtnAddClicked = false;
    this.message=" ";
    }
    if(this.messageEdit == "ok"){
      this.isBtnEditClicked = false;
    this.messageEdit=" ";
    }

    this.lines = await this.LineMeshAdminService.getLines();
  }


  public  editLine(line){/////////////////////////////////////////////////////////
    this.currentStations = line.Stations;
    this.checkedStations = new Array<Station>();
    this.isBtnEditClicked = true;
    this.editForm = this.fb.group({
      number: [line.Number],
      typeOfLine: [line.TypeOfLine],
      IDTypeOfLine:[line.IDTypeOfLine],
      currentStations: [this.currentStations],
      stations:[this.stations],
      Version:[line.Version]
    });
  }


  public async onSubmitEdit(){
///////////////////////
    this.line.Number= this.editForm.controls['number'].value;
    this.line.TypeOfLine = this.editForm.controls['typeOfLine'].value;
    this.line.IDtypeOfLine = this.editForm.controls['IDTypeOfLine'].value;
    this.line.Version = this.editForm.controls['Version'].value;
    if(this.checkedStations.length!=0){
    this.line.Stations = this.checkedStations;
    }else{
      this.line.Stations=this.currentStations;
    }

   this.LineMeshAdminService.editLine(this.line).subscribe(data=>{
      this.getLines();
      if(data!="ok")
      alert(data);
      this.messageEdit=data;
    }, err => console.log(err));

    this.isBtnEditClicked = false;

  }

  public deleteLine(line){
    this.LineMeshAdminService.deleteLine(line).subscribe(()=>{
      this.getLines();
     
    }, err => console.log(err));
  }
}

function minSelectedCheckboxes(min = 1) {
  const validator: ValidatorFn = (formArray: FormArray) => {
    const totalSelected = formArray.controls
      .map(control => control.value)
      .reduce((prev, next) => next ? prev + next : prev, 0);

    return totalSelected >= min ? null : { required: true };
  };

  return validator;
}