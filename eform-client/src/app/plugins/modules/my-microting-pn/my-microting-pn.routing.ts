import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {AdminGuard, AuthGuard, PermissionGuard} from 'src/app/common/guards';
import {MyMicrotingLayoutComponent} from './layouts';
import {MyMicrotingPnDropletsPageComponent} from './components/droplets/my-microting-pn-droplets-page/my-microting-pn-droplets-page.component';
import {MyMicrotingPnClaims} from './enums';
import { MyMicrotingPnSettingsComponent } from './components/my-microting-pn-settings/my-microting-pn-settings.component';
import { MyMicrotingPnOrganizationsPageComponent } from './components/organizations/my-microting-pn-organizations-page/my-microting-pn-organizations-page.component';

export const routes: Routes = [
  {
    path: '',
    component: MyMicrotingLayoutComponent,
    //canActivate: [PermissionGuard],
    data: {requiredPermission: MyMicrotingPnClaims.accessMyMicrotingPlugin},
    children: [
      {
        path: 'droplets',
       // canActivate: [AdminGuard],
        component: MyMicrotingPnDropletsPageComponent
      },
      {
        path: 'settings',
       // canActivate: [AdminGuard],
        component: MyMicrotingPnSettingsComponent
      },
      {
        path: 'organizations',
       // canActivate: [AdminGuard],
        component: MyMicrotingPnOrganizationsPageComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyMicrotingPnRouting {}
