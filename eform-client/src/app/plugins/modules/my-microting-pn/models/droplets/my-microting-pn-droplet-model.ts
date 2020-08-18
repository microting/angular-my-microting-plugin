export class MyMicrotingPnDropletModel {
  id: number;
  doUid: number;
  customerNo: number;
  publicIpV4: string;
  privateIpV4: string;
  publicIpV6: string;
  currentImageName: string;
  name: string;
  requestedImageName: string;
  currentImageId: number;
  requestedImageId: number;
  userData: string;
  monitoringEnabled: boolean;
  ipV6Enabled: boolean;
  backupsEnabled: boolean;
  tags: Array<string>;
}
