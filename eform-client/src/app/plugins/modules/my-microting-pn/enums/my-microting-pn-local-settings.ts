import {
  ApplicationPageModel,
  PageSettingsModel
} from 'src/app/common/models/settings/application-page-settings.model';

export const MyMicrotingPnLocalSettings =
  new ApplicationPageModel({
      name: 'myMicrotingPnSettings',
      settings: new PageSettingsModel({
        pageSize: 10,
        sort: '',
        isSortDsc: false
      })
    }
  );