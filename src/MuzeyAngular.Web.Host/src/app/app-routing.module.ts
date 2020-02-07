import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ACWarnComponent } from './AC/AC_Warn/ac_warn.component';
import { ACStopTimeComponent } from './AC/AC_StopTime/ac_stoptime.component';
import { ACRecordMQComponent } from './AC/AC_RecordMQ/ac_recordmq.component';
import { ACLineComponent } from './AC/AC_Line/ac_line.component';
import { ACStationComponent } from './AC/AC_Station/ac_station.component';
import { ACWorkPlanMQComponent } from './AC/AC_WorkPlanMQ/ac_workplanmq.component';
import { ACLinePlanMQComponent } from './AC/AC_LinePlanMQ/ac_lineplanmq.component';
import { ACAlarmTypeComponent } from './AC/AC_AlarmType/ac_alarmtype.component';
import { ACDeviceTypeComponent } from './AC/AC_DeviceType/ac_devicetype.component';
import { ACAlarmSystemComponent } from './AC/AC_AlarmSystem/ac_alarmsystem.component';
import { ACSEPlanOrderComponent } from './AC/AC_SEPlanOrder/ac_seplanorder.component';
import { ACADCSingleComponent } from './AC/AC_ADCSingle/ac_adcsingle.component';
import { ACCalendarsComponent } from './AC/AC_Calendars/ac_calendars.component';
import { ACShiftrestComponent } from './AC/AC_Shiftrest/ac_shiftrest.component';
import { ACLineSEComponent } from './AC/AC_LineSE/ac_linese.component';
import { ACMaterielCallComponent } from './AC/AC_MaterielCall/ac_materielcall.component';
import { ACMCConfigComponent } from './AC/AC_MCConfig/ac_mcconfig.component';
import { ACADCStatisticsInfoComponent } from './AC/AC_ADCStatisticsInfo/ac_adcstatisticsinfo.component';
import { ACADCStatisticsComponent } from './AC/AC_ADCStatistics/ac_adcstatistics.component';
import { ACOfflineCarComponent } from './AC/AC_OfflineCar/ac_offlinecar.component';
import { ACFillDataComponent } from './AC/AC_FillData/ac_filldata.component';
import { ACMQAdressConfigComponent } from './AC/AC_MQAdressConfig/ac_mqadressconfig.component';
import { ACQcosInfoComponent } from './AC/AC_QcosInfo/ac_qcosinfo.component';
import { ACFactoryInfoComponent } from './AC/AC_FactoryInfo/ac_factoryinfo.component';
import { ACWorkShopInfoComponent } from './AC/AC_WorkShopInfo/ac_workshopinfo.component';
import { ACDepartmentInfoComponent } from './AC/AC_DepartmentInfo/ac_departmentinfo.component';
import { ACPersonInfoComponent } from './AC/AC_PersonInfo/ac_personinfo.component';
import { ACPlanProductComponent } from './AC/AC_PlanProduct/ac_planproduct.component';
import { ACOrderModifyComponent } from './AC/AC_OrderModify/ac_ordermodify.component';
import { ACSEMouldInfoComponent } from './AC/AC_SEMouldInfo/ac_semouldinfo.component';
import { ACTestComponent } from './AC/AC_Test/ac_test.component';
import { ACRcFictitiousComponent } from './AC/AC_RcFictitious/ac_rcfictitious.component';
import { ACRCRuleComponent } from './AC/AC_RCRule/ac_rcrule.component';
import { ACRCLogComponent } from './AC/AC_RCLog/ac_rclog.component';
import { ACMailBaseConfigComponent } from './AC/AC_MailBaseConfig/ac_mailbaseconfig.component';
import { ACMailAdresseeComponent } from './AC/AC_MailAdressee/ac_mailadressee.component';
import { ACBypassComponent } from './AC/AC_Bypass/ac_bypass.component';
import { ACWelcomeWordsComponent } from './AC/AC_WelcomeWords/ac_welcomewords.component';
//MuzeyAppend#{0}

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'ac_station', component: ACStationComponent, canActivate: [AppRouteGuard] },
                    { path: 'ac_line', component: ACLineComponent, canActivate: [AppRouteGuard] },
                    { path: 'ac_recordmq', component: ACRecordMQComponent, canActivate: [AppRouteGuard] },
                    { path: 'ac_stoptime', component: ACStopTimeComponent, canActivate: [AppRouteGuard] },
                    { path: 'ac_warn/:wType', component: ACWarnComponent, canActivate: [AppRouteGuard] },
                    { path: 'home', component: HomeComponent,  canActivate: [AppRouteGuard] },
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    { path: 'about', component: AboutComponent },
                    { path: 'update-password', component: ChangePasswordComponent }
                    ,{ path: 'ac_workplanmq', component: ACWorkPlanMQComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_lineplanmq', component: ACLinePlanMQComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_alarmtype', component: ACAlarmTypeComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_devicetype', component: ACDeviceTypeComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_alarmsystem', component: ACAlarmSystemComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_seplanorder', component: ACSEPlanOrderComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_adcsingle', component: ACADCSingleComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_calendars', component: ACCalendarsComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_shiftrest', component: ACShiftrestComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_linese', component: ACLineSEComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_materielcall', component: ACMaterielCallComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_mcconfig', component: ACMCConfigComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_adcstatisticsinfo', component: ACADCStatisticsInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_adcstatistics', component: ACADCStatisticsComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_offlinecar', component: ACOfflineCarComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_filldata', component: ACFillDataComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_mqadressconfig', component: ACMQAdressConfigComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_qcosinfo', component: ACQcosInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_factoryinfo', component: ACFactoryInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_workshopinfo', component: ACWorkShopInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_departmentinfo', component: ACDepartmentInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_personinfo', component: ACPersonInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_planproduct', component: ACPlanProductComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_ordermodify', component: ACOrderModifyComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_semouldinfo', component: ACSEMouldInfoComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_test', component: ACTestComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_rcfictitious', component: ACRcFictitiousComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_rcrule', component: ACRCRuleComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_rclog', component: ACRCLogComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_mailbaseconfig', component: ACMailBaseConfigComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_mailadressee', component: ACMailAdresseeComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_bypass', component: ACBypassComponent, canActivate: [AppRouteGuard] }
                    ,{ path: 'ac_welcomewords', component: ACWelcomeWordsComponent, canActivate: [AppRouteGuard] }
                    //MuzeyAppend#{1}
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
