import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unregistred',
  templateUrl: './unregistred.component.html',
  styleUrls: ['./unregistred.component.css']
})
export class UnregistredComponent implements OnInit {

  constructor(    private router: Router,    ) { }

  ngOnInit() {
  }


  start(){
    this.router.navigate(['/start']);
  }

}
