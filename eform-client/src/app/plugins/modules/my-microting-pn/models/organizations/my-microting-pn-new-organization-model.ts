import { MyMicrotingPnNewOrganizationPluginModel } from "./my-microting-pn-new-organization-plugin-model";

export class MyMicrotingPnNewOrganizationModel {
  Name: string;
  Address: string;
  Zip: string;
  City: string;
  VatNr: string;
  EanNo: string;
  Phone: string;
  Email: string;
  ContactPerson: string;
  LicencesCount: number;
  DomainName: string;
  Plugins: Array<MyMicrotingPnNewOrganizationPluginModel>;
}