import { Component, Injector, OnInit, ViewChild, Optional, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MuzeyServiceProxy, MuzeyReqModelOfObject, MuzeyResModelOfObject } from '@shared/service-proxies/service-proxies';
import { Moment } from 'moment';
import { MuzeyTableControlsComponent, ColModel } from '@shared/muzey-component/muzey-table-controls/muzey-table-controls.component'
import { MuzeyDateModel } from '@shared/muzey-component/muzey-date-controls/muzey-date-controls.component'
import { MuzeySelectMuzeyModel } from '@shared/muzey-component/muzey-select-controls/muzey-select-controls.component'
import { AppComponentBase } from 'shared/app-component-base';
import { isNullOrUndefined } from 'util';
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './ac_recordmq.component.html'
})
export class ACRecordMQComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datePipe: DatePipe;
    sTime: string;
    eTime: string;
    datas: any[] = [];
    rIdOps: MuzeySelectMuzeyModel[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: any[] = [];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' }
    ];
    tOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '普通申报', val: '0' },
        { text: '关键申报', val: '1' }
    ]; 
        
    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        this.datePipe = new DatePipe('zh-Hans');
        this.cols['AE'] = [
            { label: '订单号', name: 'orderNum' },
            { label: 'VIN', name: 'vin' },
            { label: '计划类型', name: 'planTypeName' },
            { label: '车型', name: 'wsCarType' },
            { label: '订单类型', name: 'OrderType' },
            { label: '流水号', name: 'SerialNumber' },
            { label: '型号', name: 'model' },
            { label: '分类', name: 'typeName' },
            { label: '过点类型', name: 'reportTypeName' },
            { label: '物理点编码', name: 'reportID' },
            { label: '物理点名称', name: 'reportName' },
            { label: '状态码', name: 'reportStatus' },
            { label: '时间', name: 'reportTime' }
        ];

        this.cols['BE'] = [
            { label: '订单号', name: 'orderNum' },
            { label: 'VIN', name: 'vin' },
            { label: '计划类型', name: 'planTypeName' },
            { label: '车型', name: 'wsCarType' },
            { label: '白车身选配码', name: 'BodySelCode' },
            { label: '订单类型', name: 'OrderType' },
            { label: '流水号', name: 'SerialNumber' },
            { label: '型号', name: 'model' },
            { label: '分类', name: 'typeName' },
            { label: '过点类型', name: 'reportTypeName' },
            { label: '物理点编码', name: 'reportID' },
            { label: '物理点名称', name: 'reportName' },
            { label: '状态码', name: 'reportStatus' },
            { label: '时间', name: 'reportTime' }
        ];
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') { this.reqModel.workShop = this.appSession.Person.workShop; this.wsDisable = true; } else { this.reqModel.workShop = this.wsOps[0].val; }

        let nowD = new Date();
        this.reqModel.sTime = this.datePipe.transform(new Date(nowD.getTime()), 'yyyy-MM-dd') + ' 00:00:00';
        this.reqModel.eTime = this.datePipe.transform(new Date(nowD.getTime()), 'yyyy-MM-dd') + ' 23:59:59';
        this.sTime = this.datePipe.transform(new Date(nowD.getTime()), 'yyyy-MM-dd');
        this.eTime = this.datePipe.transform(new Date(nowD.getTime()), 'yyyy-MM-dd');
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.GetSelectData";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => { })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.rIdOps = result.datas[0].rIdOps;
            });
    }

    dateChange(dm: MuzeyDateModel): void{
        this.reqModel.sTime = dm.sTime;
        this.reqModel.eTime = dm.eTime;
    }

    wsValChange(val: string): void {

        this.reqModel.workShop = val;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.GetSelectData";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => { })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.rIdOps = result.datas[0].rIdOps;
            });

        this.reqModel.beCarType = '';
        this.reqModel.bodySelCode = '';
        this.reqModel.ReportID = '';
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.GetDatas";
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

    edit(id?: string): void {
        let editDialog;
        if (isNullOrUndefined(id)) {
            editDialog = this._dialog.open(ACRecordMQEditComponent, { data: { workShop: this.reqModel.workShop } });
        } else {
            editDialog = this._dialog.open(ACRecordMQEditComponent, {
                data: { id: id, workShop: this.reqModel.workShop }
            });
        }

        editDialog.afterClosed().subscribe(result => {
            if (result) {
                if (result) {

                    this.mtc.pageChangeSub(1);
                }

            }
        });
    }

    delete(id: string): void {

        abp.message.confirm(
            this.l('请确认数据'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACRecordMQ.Delete";
                    if (!isNullOrUndefined(this.mtc.itemObj.ID)) {
                        this.reqModel.saveData = {};
                        this.reqModel.saveData.ID = this.mtc.itemObj.ID;
                        serverReq.datas.push(this.reqModel);
                        this._service.req(serverReq).subscribe(result => {
                            abp.notify.success(this.l('删除成功'));
                            this.mtc.pageChangeSub(1);
                        });
                    }
                }
            }
        );
    }
}
@Component({
    templateUrl: './ac_recordmq_edit.component.html'
})
export class ACRecordMQEditComponent extends AppComponentBase implements OnInit {

    datePipe: DatePipe;
    saving = false;
    reqModel: any = {};
    cdOps: MuzeySelectMuzeyModel[] = [
        { text: '是', val: '1' },
        { text: '否', val: '0' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' },
    ];
    rIdOps: MuzeySelectMuzeyModel[] = [];
    rtOps: MuzeySelectMuzeyModel[] = [];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACRecordMQEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.datePipe = new DatePipe('zh-Hans');
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
        if (this.reqModel.workShop == 'BE') {
            this.rtOps = [
                { text: '上线', val: 'S' },
                { text: '下线', val: 'X' },
                { text: '计划拉出', val: 'P' },
                { text: '质量拉出', val: 'Q' },
                { text: '拉入', val: 'H' },
                { text: '转挂', val: 'Z' }
            ];
        } else {
            this.rtOps = [
                { text: '上线', val: 'S' },
                { text: '下线', val: 'X' },
                { text: '拉出', val: 'L' },
                { text: '拉入', val: 'H' },
                { text: '转挂', val: 'Z' }
            ];
        }
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.GetSelectData";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => { })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.rIdOps = result.datas[0].rIdOps;
            });
    }

    ngOnInit(): void {
        this.reqModel.saveData.WorkShop = this.reqModel.workShop;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.GetData";
        if (!isNullOrUndefined(this._data.id)) {
            this.reqModel.saveData = {};
            this.reqModel.saveData.ID = this._data.id;
            serverReq.datas.push(this.reqModel);
            this._service.req(serverReq).subscribe(result => {
                this.reqModel.saveData = result.datas[0];
            });
        }
    }

    wsValChange(val: string): void {

        this.reqModel.workShop = val;
        if (this.reqModel.workShop == 'BE') {
            this.rtOps = [
                { text: '上线', val: 'S' },
                { text: '下线', val: 'X' },
                { text: '计划拉出', val: 'P' },
                { text: '质量拉出', val: 'Q' },
                { text: '拉入', val: 'H' },
                { text: '转挂', val: 'Z' }
            ];
        } else {
            this.rtOps = [
                { text: '上线', val: 'S' },
                { text: '下线', val: 'X' },
                { text: '拉出', val: 'L' },
                { text: '拉入', val: 'H' },
                { text: '转挂', val: 'Z' }
            ];
        }
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.GetSelectData";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => { })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.rIdOps = result.datas[0].rIdOps;
            });
    }

    save(): void {
        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRecordMQ.Save";
        this.reqModel.saveData.ReportTime = this.datePipe.transform(this.reqModel.saveData.ReportTime, 'yyyy-MM-dd');
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(result => {

                if (result.resStatus == 'err') {

                    this.notify.error('错误', result.resMsg);
                } else {
                    this.notify.info(this.l('保存成功'));
                    this.close(true);
                }
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }
}

