import { Component, OnInit } from '@angular/core';
import { MyMicrotingPnSettingsService } from '../../services';
import { Router } from '@angular/router';
import { MyMicrotingPnSettingsModel } from '../../models/my-microting-pn-settings.model';

@Component({
  selector: 'app-my-microting-pn-settings',
  templateUrl: './my-microting-pn-settings.component.html',
  styleUrls: ['./my-microting-pn-settings.component.scss']
})
export class MyMicrotingPnSettingsComponent implements OnInit {

  myMicrotingPnSettingsModel: MyMicrotingPnSettingsModel = new MyMicrotingPnSettingsModel();

  constructor(private settingsService: MyMicrotingPnSettingsService,
    private router: Router) { }

  ngOnInit(): void {
    this.getSettings();
  }

  getSettings() {
    this.settingsService.getAllSettings().subscribe((data => {
      if (data && data.success) {
        this.myMicrotingPnSettingsModel = data.model;
      }
    }));
  }

  updateSettings() {
    this.settingsService.updateSettings(this.myMicrotingPnSettingsModel).subscribe((data) => {
      if (data && data.success) {
            this.router.navigate(['/plugins-settings']);
      }
    })}
}
