import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MuzeyServiceProxy, MuzeyReqModelOfObject, MuzeyResModelOfObject } from '@shared/service-proxies/service-proxies';
import { Moment } from 'moment';
import { MuzeyTableControlsComponent, ColModel } from '@shared/muzey-component/muzey-table-controls/muzey-table-controls.component'
import { MuzeyDateModel } from '@shared/muzey-component/muzey-date-controls/muzey-date-controls.component'
import { MuzeySelectMuzeyModel } from '@shared/muzey-component/muzey-select-controls/muzey-select-controls.component'
import { AppComponentBase } from 'shared/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './ac_warn.component.html'
})
export class ACWarnComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datePipe: DatePipe;
    datas: any[] = [];
    sTime: string;
    eTime: string;
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    wType: string;
    title: string;
    cols: ColModel[] = [
        //{ label: '线体编号', name: 'lineCode' },
        { label: '线体', name: 'lineName' },
        //{ label: '工位编码', name: 'stationCode' },
        { label: '工位', name: 'stationName' },
        //{ label: '报警类型编码', name: 'alarmTypeCode' },
        { label: '报警类型', name: 'alarmTypeName' },
        //{ label: '设备类型编码', name: 'deviceTypeCode' },
        { label: '设备类型', name: 'deviceTypeName' },
        //{ label: '系统类型编码', name: 'systemTypeCode' },
        { label: '系统类型', name: 'alarmSysName' },
        //{ label: '报警状态', name: 'alarmStatus' },
        { label: '报警状态', name: 'alarmStatuName' },
        { label: '报警描述', name: 'AlarmDesc' },
        { label: '开始时间', name: 'sTime' },
        { label: '结束时间', name: 'eTime' },
        { label: '持续时间', name: 'continueT' },
        { label: '是否已播放', name: 'PlayFlag' },
        { label: '播放次数', name: 'PlayNum' },
        { label: '播放时间', name: 'PlayTime' },
        { label: '优先级', name: 'Proprity' },
        { label: '消音标识', name: 'NoiseFlag' },
        { label: '传给MES的标识', name: 'ToReportMESFlag' },
        { label: '传给MES的时间', name: 'ToReprotMESTime' },
        { label: '确认标识', name: 'ConfirmedFlag' },
        { label: '确认人', name: 'ConfirmedUser' },
        { label: '确认时间', name: 'ConfirmedTime' },
        //{ label: '报警类型ID', name: 'alarmID' },
        { label: '备注', name: 'Remarks' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' },
        { text: '冲压', val: 'SE' }
    ];
    atOps: MuzeySelectMuzeyModel[] = [];
    stOps: MuzeySelectMuzeyModel[] = [
        { text: '全部', val: '' },
        { text: 'PMC报警信息', val: '1' },
        { text: '质量Andon报警记录', val: '2' }
    ]; 
    dtOps: MuzeySelectMuzeyModel[] = []; 

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
        private activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.datePipe = new DatePipe('zh-Hans');

        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') {this.reqModel.workShop = this.appSession.Person.workShop;this.wsDisable = true;} else {this.reqModel.workShop = this.wsOps[0].val;}
        this.title = '报警明细';
        this.wType = this.activatedRoute.snapshot.params['wType'];
        if (this.wType[0] != '0') {

            this.wType = this.wType.substring(0, this.wType.length - 1);
            let param = this.wType.split(',');
            let paramDatas = [];
            paramDatas[param[1]] = param[2];
            if (param.length > 3) {

                this.wsOps = [
                    { text: '总装', val: 'AE' },
                    { text: '焊装', val: 'BE' }
                ];

                paramDatas[param[3]] = param[4];
            } else {

                this.wsOps = [
                    { text: '焊装', val: 'BE' }
                ];
            }

            if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') {this.reqModel.workShop = this.appSession.Person.workShop;this.wsDisable = true;} else {this.reqModel.workShop = this.wsOps[0].val;}
            this.title = decodeURI(param[0]) + '报警';
            this.reqModel.alarmTypeCode = paramDatas[this.reqModel.workShop];
        } else {
            let nowD = new Date();
            this.reqModel.sTime = this.datePipe.transform(new Date(nowD.getTime() - (24 * 3600 * 1000 * 30)), 'yyyy-MM-dd') + ' 00:00:00';
            this.reqModel.eTime = this.datePipe.transform(new Date(nowD.getTime() + (24 * 3600 * 1000 * 30)), 'yyyy-MM-dd') + ' 23:59:59';
            this.sTime = this.datePipe.transform(new Date(nowD.getTime() - (24 * 3600 * 1000 * 30)), 'yyyy-MM-dd');
            this.eTime = this.datePipe.transform(new Date(nowD.getTime() + (24 * 3600 * 1000 * 30)), 'yyyy-MM-dd');
        }

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "BaseData.GetAlarmTypes";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq).subscribe((result: MuzeyResModelOfObject) => {
            this.atOps = result.datas;
        });
        serverReq.action = "BaseData.GetDeviceTypes";
        this._service.req(serverReq).subscribe((result: MuzeyResModelOfObject) => {
            this.dtOps = result.datas;
        });
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
    }

    dateChange(dm: MuzeyDateModel): void {
        this.reqModel.sTime = dm.sTime;
        this.reqModel.eTime = dm.eTime;
    }

    wsChange(val: any): void {
        
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "BaseData.GetAlarmTypes";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq).subscribe((result: MuzeyResModelOfObject) => {
            this.atOps = result.datas;
        });
        serverReq.action = "BaseData.GetDeviceTypes";
        this._service.req(serverReq).subscribe((result: MuzeyResModelOfObject) => {
            this.dtOps = result.datas;
        });

        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACWarn.GetDatas";
        serverReq.offset = offset;
        serverReq.pageSize = this.mtc.pageSize;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.mtc.isTableLoading = false;
                })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.datas = result.datas;
                this.totalCount = result.totalCount;
            });
    }
}
