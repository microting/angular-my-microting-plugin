import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {Observable} from 'rxjs';
import {BaseService} from 'src/app/common/services/base.service';
import {OperationDataResult} from 'src/app/common/models/operation.models';
import { MyMicrotingPnOrganizationsModel, MyMicrotingPnOrganizationsRequestModel } from '../../models/organizations';
import {MyMicrotingPnDropletsModel, MyMicrotingPnDropletsRequestModel} from 'src/app/plugins/modules/my-microting-pn/models/droplets';
import {MyMicrotingPnDropletsMethods} from 'src/app/plugins/modules/my-microting-pn/services';

export let MyMicrotingPnOrganizationsMethods = {
  getOrganizations: 'api/my-microting-pn/organizations'
};

@Injectable()
export class MyMicrotingPnOrganizationsService extends BaseService {

  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  // tslint:disable-next-line:max-line-length
  fetchOrganizationsFromApi(model: MyMicrotingPnOrganizationsRequestModel): Observable<OperationDataResult<MyMicrotingPnOrganizationsModel>> {
    return this.get(MyMicrotingPnOrganizationsMethods.getOrganizations + '/fetch', model);
  }

  getOrganizations(model: MyMicrotingPnOrganizationsRequestModel): Observable<OperationDataResult<MyMicrotingPnOrganizationsModel>> {
    return this.get(MyMicrotingPnOrganizationsMethods.getOrganizations, model);
  }
}
