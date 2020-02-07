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

@Component({
    templateUrl: './ac_stoptime.component.html',
})
export class ACStopTimeComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [
        //{ label: '线体编号', name: 'lineCode' },
        { label: '线体', name: 'lineName' },
        //{ label: '工位编码', name: 'stationCode' },
        { label: '工位', name: 'stationName' },
        { label: '停线描述', name: 'StopDesc' },
        //{ label: '停线状态', name: 'stopStatus' },
        { label: '停线状态', name: 'stopStatusName' },
        { label: '停线开始时间', name: 'sTime' },
        { label: '停线结束时间', name: 'eTime' },
        { label: '持续时间', name: 'continueTime' },
        //{ label: '传给MES的标识', name: 'ToReportMESFlag' },
        //{ label: '传给MES的时间', name: 'ToReprotMESTime' },
        //{ label: '确认标识', name: 'ConfirmedFlag' },
        //{ label: '确认人', name: 'ConfirmedUser' },
        //{ label: '确认时间', name: 'ConfirmedTime' },
        //{ label: '备注', name: 'Remarks' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' },
        { text: '冲压', val: 'SE' }
    ]; 

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') {this.reqModel.workShop = this.appSession.Person.workShop;this.wsDisable = true;} else {this.reqModel.workShop = this.wsOps[0].val;}
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
    }

    dateChange(dm: MuzeyDateModel): void {
        this.reqModel.sTime = dm.sTime;
        this.reqModel.eTime = dm.eTime;
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACStopTime.GetDatas";
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
