import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgSelectModule} from '@ng-select/ng-select';
import {TranslateModule} from '@ngx-translate/core';
import {MDBRootModule} from 'angular-bootstrap-md';
import { Papa } from 'ngx-papaparse';

import {MyMicrotingLayoutComponent} from './layouts';
import {MyMicrotingPnRouting} from './my-microting-pn.routing';
import {SharedPnModule} from '../shared/shared-pn.module';

import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {EformSharedModule} from 'src/app/common/modules/eform-shared/eform-shared.module';
import { MyMicrotingPnDropletsPageComponent } from './components/droplets/my-microting-pn-droplets-page/my-microting-pn-droplets-page.component';
import { MyMicrotingPnDropletsService, MyMicrotingPnSettingsService, MyMicrotingPnOrganizationsService } from './services';
import { MyMicrotingPnSettingsComponent } from './components/my-microting-pn-settings/my-microting-pn-settings.component';
import { MyMicrotingPnCofirmModalComponent } from './components/shared/my-microting-pn-confirm-modal.component.ts/my-microting-pn-confirm-modal.component';
import { MyMicrotingPnOrganizationsPageComponent } from './components/organizations/my-microting-pn-organizations-page/my-microting-pn-organizations-page.component';
import { MyMicrotingPnOrganizationsNewComponent } from './components/organizations/my-microting-pn-organizations-new/my-microting-pn-organizations-new.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SharedPnModule,
        MyMicrotingPnRouting,
        TranslateModule,
        MDBRootModule,
        NgSelectModule,
        FontAwesomeModule,
        EformSharedModule
    ],
  declarations: [
    MyMicrotingLayoutComponent,
    MyMicrotingPnDropletsPageComponent,
    MyMicrotingPnSettingsComponent,
    MyMicrotingPnCofirmModalComponent,
    MyMicrotingPnOrganizationsPageComponent,
    MyMicrotingPnOrganizationsNewComponent
  ],
  providers: [
    MyMicrotingPnDropletsService,
    MyMicrotingPnOrganizationsService,
    MyMicrotingPnSettingsService
  ]
})
export class MyMicrotingPnModule {
}
