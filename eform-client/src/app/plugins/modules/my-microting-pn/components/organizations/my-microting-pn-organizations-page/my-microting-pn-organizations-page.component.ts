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

        this.orgsModel = new MyMicrotingPnOrganizationsModel();
        this.orgsModel.total = 100;
        var orgs = new Array<MyMicrotingPnOrganizationModel>()
        orgs[0] = new MyMicrotingPnOrganizationModel();
        orgs[0].Id = 1;
        orgs[0].CustomerId = 2;
        orgs[0].DomainName = 'test.com';
        orgs[0].ServiceEmail = 'test@test.com';
        orgs[0].NumberOfLicenses = 3
        orgs[0].NumberOfLicensesUsed = 4;
        orgs[0].UpToDateStatus = 'UpToDateStatus';
        orgs[0].NextUpgrade = new Date();
        orgs[0].InstanceStatus = 'ddddd';
        orgs[0].InstanceId = 5;

        this.orgsModel.organizations = orgs;
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
