import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {Observable} from 'rxjs';
import {BaseService} from 'src/app/common/services/base.service';
import {OperationDataResult} from 'src/app/common/models/operation.models';
import { MyMicrotingPnOrganizationsModel, MyMicrotingPnOrganizationsRequestModel } from '../../models/organizations';

export let MyMicrotingPnOrganizationsMethods = {
  getOrganizations: 'api/my-microting-pn/organizations'
};

@Injectable()
export class MyMicrotingPnOrganizationsService extends BaseService {

  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getOrganizations(model: MyMicrotingPnOrganizationsRequestModel): Observable<OperationDataResult<MyMicrotingPnOrganizationsModel>> {
    return this.get(MyMicrotingPnOrganizationsMethods.getOrganizations, model);
  }
}
