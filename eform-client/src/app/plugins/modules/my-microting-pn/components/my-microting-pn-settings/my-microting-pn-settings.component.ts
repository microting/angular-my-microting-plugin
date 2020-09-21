import { Component, OnInit } from '@angular/core';
import {MyMicrotingPnImagesService, MyMicrotingPnSettingsService} from '../../services';
import { Router } from '@angular/router';
import { MyMicrotingPnSettingsModel } from '../../models/my-microting-pn-settings.model';
import {MyMicrotingPnImagesModel, MyMicrotingPnImagesRequestModel} from 'src/app/plugins/modules/my-microting-pn/models/images';

@Component({
  selector: 'app-my-microting-pn-settings',
  templateUrl: './my-microting-pn-settings.component.html',
  styleUrls: ['./my-microting-pn-settings.component.scss']
})
export class MyMicrotingPnSettingsComponent implements OnInit {

  myMicrotingPnSettingsModel: MyMicrotingPnSettingsModel = new MyMicrotingPnSettingsModel();
  allImages: MyMicrotingPnImagesModel = new MyMicrotingPnImagesModel();
  imagesRequestModel: MyMicrotingPnImagesRequestModel = new MyMicrotingPnImagesRequestModel();

  constructor(private settingsService: MyMicrotingPnSettingsService,
    private imagesService: MyMicrotingPnImagesService,
    private router: Router) { }

  ngOnInit(): void {
    this.getSettings();
  }

  getSettings() {
    this.settingsService.getAllSettings().subscribe((data => {
      if (data && data.success) {
        this.myMicrotingPnSettingsModel = data.model;
        this.myMicrotingPnSettingsModel.imageId = Number(this.myMicrotingPnSettingsModel.imageId);
        this.getAllImages();
      }
    }));
  }

  updateSettings() {
    this.settingsService.updateSettings(this.myMicrotingPnSettingsModel).subscribe((data) => {
      if (data && data.success) {
            this.router.navigate(['/plugins-settings']);
      }
    });
  }

  getAllImages() {
    this.imagesService.getImages(this.imagesRequestModel).subscribe((data) => {
      if (data && data.success) {
        this.allImages = data.model;
      }
    });
  }

  updateSelectedImage(event: any) {
    this.myMicrotingPnSettingsModel.imageId = event;
  }
}
