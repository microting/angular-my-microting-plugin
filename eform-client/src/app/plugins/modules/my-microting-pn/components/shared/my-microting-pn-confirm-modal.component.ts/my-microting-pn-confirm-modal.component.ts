import { Component, OnInit, ViewChild} from '@angular/core';

@Component({
  selector: 'app-my-microting-pn-confirm-modal',
  templateUrl: './my-microting-pn-confirm-modal.component.html',
  styleUrls: ['./my-microting-pn-confirm-modal.component.scss']
})
export class MyMicrotingPnCofirmModalComponent implements OnInit {
  @ViewChild('frame', { static: true }) frame;

  message: string;
  action: Function;

  constructor() { }

  ngOnInit(): void {
  }

  show(message: string, action: Function) {
    this.message = message;
    this.action = action;

    this.frame.show();
  }
  
  confirm(){
    this.action();
    this.frame.hide();
  }

}
