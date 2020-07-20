import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';

import {Observable} from 'rxjs';
import {Router} from '@angular/router';
import {OperationDataResult, OperationResult} from 'src/app/common/models/operation.models';
import {BaseService} from 'src/app/common/services/base.service';
import {MyMicrotingPnDropletsModel, MyMicrotingPnDropletsRequestModel} from '../../models/droplets'

export let MyMicrotingPnDropletsMethods = {
  getDroplets: 'api/my-microting-pn/droplets'
};

@Injectable()
export class MyMicrotingPnDropletsService extends BaseService {
  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getDroplets(model: MyMicrotingPnDropletsRequestModel): Observable<OperationDataResult<MyMicrotingPnDropletsModel>> {
    return this.get(MyMicrotingPnDropletsMethods.getDroplets, model);
  }
}