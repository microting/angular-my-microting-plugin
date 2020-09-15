import { Component, OnInit, ViewChild } from '@angular/core';
import { SharedPnService } from 'src/app/plugins/modules/shared/services';
import { PageSettingsModel } from 'src/app/common/models';
import { MyMicrotingPnOrganizationsService } from '../../../services/';
import { MyMicrotingPnOrganizationsModel, MyMicrotingPnOrganizationsRequestModel, MyMicrotingPnOrganizationModel, MyMicrotingPnNewOrganizationModel } from '../../../models/organizations';

@Component({
  selector: 'app-my-microting-pn-organizations-page',
  templateUrl: './my-microting-pn-organizations-page.component.html',
  styleUrls: ['./my-microting-pn-organizations-page.component.scss']
})
export class MyMicrotingPnOrganizationsPageComponent implements OnInit {
  @ViewChild('modalconfirm', { static: true }) modalConfirm;
  @ViewChild('modalnew', { static: true }) modalNew;

  orgsRequestModel: MyMicrotingPnOrganizationsRequestModel = new MyMicrotingPnOrganizationsRequestModel();
  localPageSettings: PageSettingsModel = new PageSettingsModel();
  orgsModel: MyMicrotingPnOrganizationsModel = new MyMicrotingPnOrganizationsModel();
  newOrg: MyMicrotingPnNewOrganizationModel = new MyMicrotingPnNewOrganizationModel();
  constructor(private sharedPnService: SharedPnService,
    private orgsService: MyMicrotingPnOrganizationsService,
) { }

  ngOnInit(): void {
    this.getLocalPageSettings();
  }

  getLocalPageSettings() {
    this.localPageSettings = this.sharedPnService.getLocalPageSettings
      ('myMicrontingPnOrgsSettings').settings;
    this.getOrgs();
  }

  getOrgs() {
    this.orgsRequestModel.isSortDsc = this.localPageSettings.isSortDsc;
    this.orgsRequestModel.sortColumnName = this.localPageSettings.sort;
    this.orgsRequestModel.pageSize = this.localPageSettings.pageSize;
    this.orgsService.getOrganizations(this.orgsRequestModel).subscribe((data) => {
      if (data && data.success) {
        this.orgsModel = data.model;
      }
    });
  }

  showCreateNewModal(){
    this.modalNew.show(() => {
      this.orgsService.getOrganizations(this.orgsRequestModel).subscribe((data) => {
        if (data && data.success) {
          this.orgsModel = data.model;
        }
      });
    });
  }

  showFetchModal(){
    this.modalConfirm.show('Are you sure you want to fetch all droplets information from Digital Ocean', () => {
      this.orgsService.fetchOrganizationsFromApi(this.orgsRequestModel).subscribe((data) => {
        if (data && data.success) {
          this.orgsModel = data.model;
        }
      });
    });
  }

  onSearchInputChanged(value: any) {
    this.orgsRequestModel.name = value;
    this.getOrgs();
  }

  sortTable(sort: string, sortByDesc: boolean) {
    this.localPageSettings.isSortDsc = sortByDesc;
    this.localPageSettings.sort = sort;
    this.updateLocalPageSettings();
  }

  updateLocalPageSettings() {
    this.sharedPnService.updateLocalPageSettings
    ('myMicrontingPnOrgsSettings', this.localPageSettings);
    this.getOrgs();
  }

  changePage(e: any) {
    if (e || e === 0) {
      this.orgsRequestModel.offset = e;
      if (e === 0) {
        this.orgsRequestModel.pageIndex = 0;
      } else {
        this.orgsRequestModel.pageIndex = Math.floor(e / this.orgsRequestModel.pageSize);
      }
      this.getOrgs();
    }
  }

}
