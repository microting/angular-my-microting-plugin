import {Injectable} from '@angular/core';
import {BaseService} from 'src/app/common/services/base.service';
import {ToastrService} from 'ngx-toastr';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {Observable} from 'rxjs';
import {OperationDataResult} from 'src/app/common/models';
import {MyMicrotingPnImagesModel, MyMicrotingPnImagesRequestModel} from 'src/app/plugins/modules/my-microting-pn/models/images';

export let MyMicrotingPnImagesMethods = {
  getImages: 'api/my-microting-pn/images'
};

@Injectable()
export class MyMicrotingPnImagesService extends BaseService {
  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getImages(model: MyMicrotingPnImagesRequestModel): Observable<OperationDataResult<MyMicrotingPnImagesModel>> {
    return this.get(MyMicrotingPnImagesMethods.getImages, model);
  }
}
