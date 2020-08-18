import { Component, OnInit, ViewChild } from '@angular/core';
import { SharedPnService } from 'src/app/plugins/modules/shared/services';
import { PageSettingsModel } from 'src/app/common/models';
import { MyMicrotingPnDropletsService } from '../../../services';
import { MyMicrotingPnDropletsModel, MyMicrotingPnDropletsRequestModel, MyMicrotingPnDropletModel } from '../../../models/droplets';

@Component({
  selector: 'app-my-microting-pn-droplets-page',
  templateUrl: './my-microting-pn-droplets-page.component.html',
  styleUrls: ['./my-microting-pn-droplets-page.component.scss']
})
export class MyMicrotingPnDropletsPageComponent implements OnInit {
  @ViewChild('modalconfirm', { static: true }) modalConfirm;

  dropletsRequestModel: MyMicrotingPnDropletsRequestModel = new MyMicrotingPnDropletsRequestModel();
  localPageSettings: PageSettingsModel = new PageSettingsModel();
  dropletsModel: MyMicrotingPnDropletsModel = new MyMicrotingPnDropletsModel();

  constructor(private sharedPnService: SharedPnService,
            private dropletsService: MyMicrotingPnDropletsService,
    ) { }

  ngOnInit(): void {
    this.getLocalPageSettings();
  }

  getLocalPageSettings() {
    this.localPageSettings = this.sharedPnService.getLocalPageSettings
      ('myMicrontingPnDropletsSettings').settings;
    this.getDroplets();
  }

  getDroplets() {
    this.dropletsRequestModel.isSortDsc = this.localPageSettings.isSortDsc;
    this.dropletsRequestModel.sortColumnName = this.localPageSettings.sort;
    this.dropletsRequestModel.pageSize = this.localPageSettings.pageSize;
    this.dropletsService.getDroplets(this.dropletsRequestModel).subscribe((data) => {
      if (data && data.success) {
        this.dropletsModel = data.model;
      }
    });
  }

  showFetchModal(){
    this.modalConfirm.show('Are you sure you want to fetch all droplets information from Digital Ocean', () => {
      this.dropletsService.fetchDropletsFromDo(this.dropletsRequestModel).subscribe((data) => {
        if (data && data.success) {
          this.dropletsModel = data.model;
        }
      });
    });
  }

  showUpgradeDropletModal(dropletId: number, imageId: number){
    this.modalConfirm.show('Are you sure you want to upgrade droplet', () => {
      this.dropletsService.upgrade(dropletId, imageId, this.dropletsRequestModel).subscribe((data) => {
        if (data && data.success) {
          this.dropletsModel = data.model;
        }
      });
    });
  }

  onSearchInputChanged(value: any) {
    this.dropletsRequestModel.name = value;
    this.getDroplets();
  }

  sortTable(sort: string, sortByDesc: boolean) {
    this.localPageSettings.isSortDsc = sortByDesc;
    this.localPageSettings.sort = sort;
    this.updateLocalPageSettings();
  }

  updateLocalPageSettings() {
    this.sharedPnService.updateLocalPageSettings
    ('myMicrontingPnDropletsSettings', this.localPageSettings);
    this.getDroplets();
  }

  changePage(e: any) {
    if (e || e === 0) {
      this.dropletsRequestModel.offset = e;
      if (e === 0) {
        this.dropletsRequestModel.pageIndex = 0;
      } else {
        this.dropletsRequestModel.pageIndex = Math.floor(e / this.dropletsRequestModel.pageSize);
      }
      this.getDroplets();
    }
  }
}
