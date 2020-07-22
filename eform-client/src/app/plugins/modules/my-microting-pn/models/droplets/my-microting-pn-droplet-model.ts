export class MyMicrotingPnDropletModel {
  Id: number;
  DoUid: number;
  CustomerNo: number; 
  PublicIpV4: string; 
  PrivateIpV4: string;  
  PublicIpV6: string;  
  CurrentImageName: string; 
  Name: string; 
  RequestedImageName: string; 
  CurrentImageId: number;
  RequestedImageId: number;
  UserData: string;   
  MonitoringEnabled: boolean;  
  IpV6Enabled: boolean;  
  BackupsEnabled: boolean;  
  Tags: Array<string>;
}