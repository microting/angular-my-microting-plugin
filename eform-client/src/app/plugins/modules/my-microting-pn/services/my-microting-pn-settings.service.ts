import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';

import {Observable} from 'rxjs';
import {Router} from '@angular/router';
import {OperationDataResult, OperationResult} from 'src/app/common/models/operation.models';
import {BaseService} from 'src/app/common/services/base.service';
import { MyMicrotingPnSettingsModel } from '../models/my-microting-pn-settings.model';

export let MyMicrotingPnSettingsMethods = {
  MyMicrotingPnSettings: 'api/my-microting-pn/settings'
};

@Injectable()
export class MyMicrotingPnSettingsService extends BaseService {
  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getAllSettings(): Observable<OperationDataResult<MyMicrotingPnSettingsModel>> {
    return this.get(MyMicrotingPnSettingsMethods.MyMicrotingPnSettings);
  }

  updateSettings(model: MyMicrotingPnSettingsModel): Observable<OperationResult> {
    return this.post(MyMicrotingPnSettingsMethods.MyMicrotingPnSettings, model);
  }
}