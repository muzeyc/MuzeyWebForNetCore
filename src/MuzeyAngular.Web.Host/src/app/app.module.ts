import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JsonpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AbpModule } from '@abp/abp.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';

import { HomeComponent } from '@app/home/home.component';
import { AboutComponent } from '@app/about/about.component';
import { TopBarComponent } from '@app/layout/topbar.component';
import { TopBarLanguageSwitchComponent } from '@app/layout/topbar-languageswitch.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';
// tenants
import { TenantsComponent } from '@app/tenants/tenants.component';
import { CreateTenantDialogComponent } from './tenants/create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './tenants/edit-tenant/edit-tenant-dialog.component';
// roles
import { RolesComponent } from '@app/roles/roles.component';
import { CreateRoleDialogComponent } from './roles/create-role/create-role-dialog.component';
import { EditRoleDialogComponent } from './roles/edit-role/edit-role-dialog.component';
// users
import { UsersComponent } from '@app/users/users.component';
import { CreateUserDialogComponent } from '@app/users/create-user/create-user-dialog.component';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ResetPasswordDialogComponent } from './users/reset-password/reset-password.component';
//AC
import { ACWarnComponent } from '@app/AC/AC_Warn/ac_warn.component';
import { ACStopTimeComponent } from '@app/AC/AC_StopTime/ac_stoptime.component';
import { ACRecordMQComponent, ACRecordMQEditComponent } from '@app/AC/AC_RecordMQ/ac_recordmq.component';
import { ACLineComponent, ACLineEditComponent } from '@app/AC/AC_Line/ac_line.component';
import { ACStationComponent, ACStationEditComponent } from '@app/AC/AC_Station/ac_station.component';
import { ACWorkPlanMQComponent, ACWorkPlanMQEditComponent } from '@app/AC/AC_WorkPlanMQ/ac_workplanmq.component';
import { ACLinePlanMQComponent } from '@app/AC/AC_LinePlanMQ/ac_lineplanmq.component';
import { ACAlarmTypeComponent, ACAlarmTypeEditComponent } from '@app/AC/AC_AlarmType/ac_alarmtype.component';
import { ACDeviceTypeComponent, ACDeviceTypeEditComponent } from '@app/AC/AC_DeviceType/ac_devicetype.component';
import { ACAlarmSystemComponent, ACAlarmSystemEditComponent } from '@app/AC/AC_AlarmSystem/ac_alarmsystem.component';
import { ACSEPlanOrderComponent, ACSEPlanOrderEditComponent } from '@app/AC/AC_SEPlanOrder/ac_seplanorder.component';
import { ACADCSingleComponent } from '@app/AC/AC_ADCSingle/ac_adcsingle.component';
import { ACCalendarsComponent, ACCalendarsEditComponent } from '@app/AC/AC_Calendars/ac_calendars.component';
import { ACShiftrestComponent, ACShiftrestEditComponent } from '@app/AC/AC_Shiftrest/ac_shiftrest.component';
import { ACLineSEComponent } from '@app/AC/AC_LineSE/ac_linese.component';
import { ACMaterielCallComponent } from '@app/AC/AC_MaterielCall/ac_materielcall.component';
import { ACMCConfigComponent, ACMCConfigEditComponent } from '@app/AC/AC_MCConfig/ac_mcconfig.component';
import { ACADCStatisticsInfoComponent } from '@app/AC/AC_ADCStatisticsInfo/ac_adcstatisticsinfo.component';
import { ACADCStatisticsComponent } from '@app/AC/AC_ADCStatistics/ac_adcstatistics.component';
import { ACOfflineCarComponent, ACOfflineCarEditComponent } from '@app/AC/AC_OfflineCar/ac_offlinecar.component';
import { ACFillDataComponent, ACFillDataEditComponent } from '@app/AC/AC_FillData/ac_filldata.component';
import { ACMQAdressConfigComponent, ACMQAdressConfigEditComponent } from '@app/AC/AC_MQAdressConfig/ac_mqadressconfig.component';
import { ACQcosInfoComponent } from '@app/AC/AC_QcosInfo/ac_qcosinfo.component';
import { ACFactoryInfoComponent, ACFactoryInfoEditComponent } from '@app/AC/AC_FactoryInfo/ac_factoryinfo.component';
import { ACWorkShopInfoComponent, ACWorkShopInfoEditComponent } from '@app/AC/AC_WorkShopInfo/ac_workshopinfo.component';
import { ACDepartmentInfoComponent, ACDepartmentInfoEditComponent } from '@app/AC/AC_DepartmentInfo/ac_departmentinfo.component';
import { ACPersonInfoComponent, ACPersonInfoEditComponent } from '@app/AC/AC_PersonInfo/ac_personinfo.component';
import { ACPlanProductComponent, ACPlanProductEditComponent } from '@app/AC/AC_PlanProduct/ac_planproduct.component';
import { ACOrderModifyComponent } from '@app/AC/AC_OrderModify/ac_ordermodify.component';
import { ACSEMouldInfoComponent, ACSEMouldInfoEditComponent } from '@app/AC/AC_SEMouldInfo/ac_semouldinfo.component';
import { ACTestComponent } from '@app/AC/AC_Test/ac_test.component';
import { ACRcFictitiousComponent, ACRcFictitiousEditComponent, ACRcFictitiousPreinstallComponent, ACRcFictitiousInventedComponent } from '@app/AC/AC_RcFictitious/ac_rcfictitious.component';
import { ACRCRuleComponent, ACRCRuleEditComponent } from '@app/AC/AC_RCRule/ac_rcrule.component';
import { ACRCLogComponent, ACRCLogEditComponent } from '@app/AC/AC_RCLog/ac_rclog.component';
import { ACMailBaseConfigComponent, ACMailBaseConfigEditComponent } from '@app/AC/AC_MailBaseConfig/ac_mailbaseconfig.component';
import { ACMailAdresseeComponent, ACMailAdresseeEditComponent } from '@app/AC/AC_MailAdressee/ac_mailadressee.component';
import { ACBypassComponent, ACBypassEditComponent } from '@app/AC/AC_Bypass/ac_bypass.component';
import { ACWelcomeWordsComponent, ACWelcomeWordsEditComponent } from '@app/AC/AC_WelcomeWords/ac_welcomewords.component';
//MuzeyAppend#{0}

@NgModule({
  declarations: [
      AppComponent,
      HomeComponent,
      AboutComponent,
      TopBarComponent,
      TopBarLanguageSwitchComponent,
      SideBarUserAreaComponent,
      SideBarNavComponent,
      SideBarFooterComponent,
      RightSideBarComponent,
      // tenants
      TenantsComponent,
      CreateTenantDialogComponent,
      EditTenantDialogComponent,
      // roles
      RolesComponent,
      CreateRoleDialogComponent,
      EditRoleDialogComponent,
      // users
      UsersComponent,
      CreateUserDialogComponent,
      EditUserDialogComponent,
      ChangePasswordComponent,
      ResetPasswordDialogComponent,
      //AC
      ACWarnComponent,
      ACStopTimeComponent,
      ACRecordMQComponent,
      ACRecordMQEditComponent,
      ACLineComponent,
      ACLineEditComponent,
      ACStationComponent,
      ACStationEditComponent
      , ACWorkPlanMQComponent
      , ACWorkPlanMQEditComponent
      ,ACLinePlanMQComponent
      ,ACAlarmTypeComponent,      
      ACAlarmTypeEditComponent
      ,ACDeviceTypeComponent,      
      ACDeviceTypeEditComponent
      ,ACAlarmSystemComponent,      
      ACAlarmSystemEditComponent
      , ACSEPlanOrderComponent
      , ACSEPlanOrderEditComponent
      ,ACADCSingleComponent
      ,ACCalendarsComponent,      
      ACCalendarsEditComponent
      ,ACShiftrestComponent,      
      ACShiftrestEditComponent
      ,ACLineSEComponent
      ,ACMaterielCallComponent
      ,ACMCConfigComponent,      
      ACMCConfigEditComponent
      ,ACADCStatisticsInfoComponent
      ,ACADCStatisticsComponent
      ,ACOfflineCarComponent,      
      ACOfflineCarEditComponent
      ,ACFillDataComponent,      
      ACFillDataEditComponent
      ,ACMQAdressConfigComponent,      
      ACMQAdressConfigEditComponent
      ,ACQcosInfoComponent
      ,ACFactoryInfoComponent,      
      ACFactoryInfoEditComponent
      ,ACWorkShopInfoComponent,      
      ACWorkShopInfoEditComponent
      ,ACDepartmentInfoComponent,      
      ACDepartmentInfoEditComponent
      ,ACPersonInfoComponent,      
      ACPersonInfoEditComponent
      ,ACPlanProductComponent,      
      ACPlanProductEditComponent
      ,ACOrderModifyComponent
      ,ACSEMouldInfoComponent,      
      ACSEMouldInfoEditComponent
      ,ACTestComponent
      ,ACRcFictitiousComponent,      
      ACRcFictitiousEditComponent,
      ACRcFictitiousPreinstallComponent,
      ACRcFictitiousInventedComponent
      ,ACRCRuleComponent,      
      ACRCRuleEditComponent
      ,ACRCLogComponent,      
      ACRCLogEditComponent
      ,ACMailBaseConfigComponent,      
      ACMailBaseConfigEditComponent
      ,ACMailAdresseeComponent,      
      ACMailAdresseeEditComponent
      ,ACBypassComponent,      
      ACBypassEditComponent
      ,ACWelcomeWordsComponent,      
      ACWelcomeWordsEditComponent
//MuzeyAppend#{1}
    
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    JsonpModule,
    ModalModule.forRoot(),
    AbpModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
  ],
  providers: [],
  entryComponents: [
      // tenants
      CreateTenantDialogComponent,
      EditTenantDialogComponent,
      // roles
      CreateRoleDialogComponent,
      EditRoleDialogComponent,
      // users
      CreateUserDialogComponent,
      EditUserDialogComponent,
      ResetPasswordDialogComponent,
      //AC
      ACLineEditComponent,
      ACStationEditComponent,
      ACRecordMQEditComponent
      , ACWorkPlanMQEditComponent
      ,ACAlarmTypeEditComponent
      ,ACDeviceTypeEditComponent
      ,ACAlarmSystemEditComponent
      ,ACCalendarsEditComponent
      ,ACShiftrestEditComponent
      ,ACMCConfigEditComponent
      ,ACOfflineCarEditComponent
      ,ACFillDataEditComponent
      ,ACMQAdressConfigEditComponent
      ,ACFactoryInfoEditComponent
      ,ACWorkShopInfoEditComponent
      ,ACDepartmentInfoEditComponent
      ,ACPersonInfoEditComponent
      ,ACPlanProductEditComponent
      , ACSEMouldInfoEditComponent
      , ACSEPlanOrderEditComponent
      , ACRcFictitiousEditComponent
      , ACRcFictitiousPreinstallComponent
      , ACRcFictitiousInventedComponent
      ,ACRCRuleEditComponent
      ,ACRCLogEditComponent
      ,ACMailBaseConfigEditComponent
      ,ACMailAdresseeEditComponent
      ,ACBypassEditComponent
      ,ACWelcomeWordsEditComponent
//MuzeyAppend#{2}
  ]
})
export class AppModule {}
