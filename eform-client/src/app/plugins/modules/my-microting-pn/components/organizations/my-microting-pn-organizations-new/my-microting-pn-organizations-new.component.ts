import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MyMicrotingPnNewOrganizationModel } from '../../../models/organizations/my-microting-pn-new-organization-model';

@Component({
  selector: 'app-my-microting-pn-organizations-new',
  templateUrl: './my-microting-pn-organizations-new.component.html',
  styleUrls: ['./my-microting-pn-organizations-new.component.scss']
})
export class MyMicrotingPnOrganizationsNewComponent implements OnInit {
  @ViewChild('frame', { static: true }) frame;

  @Input() model: MyMicrotingPnNewOrganizationModel;

  action: Function;

  constructor() { }

  ngOnInit(): void {
  }

  show(action: Function) {
    this.action = action;

    this.frame.show();
  }

}
