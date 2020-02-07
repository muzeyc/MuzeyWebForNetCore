import { Component, Injector, OnInit, ViewChild, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, MatCheckboxChange } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MuzeyServiceProxy, MuzeyReqModelOfObject, MuzeyResModelOfObject } from '@shared/service-proxies/service-proxies';
import { Moment } from 'moment';
import { MuzeyTableControlsComponent, ColModel } from '@shared/muzey-component/muzey-table-controls/muzey-table-controls.component'
import { MuzeyDateModel } from '@shared/muzey-component/muzey-date-controls/muzey-date-controls.component'
import { MuzeySelectMuzeyModel } from '@shared/muzey-component/muzey-select-controls/muzey-select-controls.component'
import { AppComponentBase } from 'shared/app-component-base';
import { isNullOrUndefined } from 'util';

@Component({
    templateUrl: './ac_workplanmq.component.html',
})
export class ACWorkPlanMQComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    QcosShow: boolean = true;
    colss: any[] = [];
    cols: ColModel[] = [];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' }
    ];
    wdOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '完工', val: '1' },
        { text: '未完工', val: '0' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') {this.reqModel.workShop = this.appSession.Person.workShop;this.wsDisable = true;} else {this.reqModel.workShop = this.wsOps[0].val;}
        this.colss['AE'] = [
            { label: '订单号', name: 'OrderNum' },
            { label: 'VIN号', name: 'VIN' },
            { label: '总装车型码', name: 'AECarType' },
            { label: '订单类型', name: 'OrderType' },
            { label: '流水号', name: 'SerialNumber' },
            { label: '车身用途', name: 'BodyUse' },
            { label: 'PLC下发时间', name: 'DownloadTime' },
            { label: '下发状态', name: 'DownloadState' },
            { label: '完工状态', name: 'WorkdoneName' },
            { label: '完工时间', name: 'WorkdoneTime' },
            { label: '基础型码', name: 'CarType' },
            { label: '拉入工位点', name: 'SetIn' },
            { label: '拉出工位点', name: 'SetOut' },
            { label: '拉出原因', name: 'ReportReason' },
            { label: 'MES下发时间', name: 'DateTime' },
            { label: '总装上线顺序号', name: 'AEOnSeq' },
            { label: 'SAP收车顺序号', name: 'SAPFinSeq' },
            { label: '内饰颜色', name: 'InTrim' },
            { label: '外饰颜色', name: 'Extrim' },
            { label: '主色', name: 'BColor1' },
            { label: '副色', name: 'BColor2' },
            { label: '工厂代码', name: 'Plant' },
            { label: '生产线号', name: 'Line' },
            { label: '项目车标记', name: 'ProFlag' },
            { label: '进总装日期', name: 'InAETime' },
            { label: '总装收车日期', name: 'AEFinTime' },
            { label: '操作状态', name: 'OperMode' },
            { label: '车身选装包', name: 'OpPac' }
        ];

        this.colss['BE'] = [
            { label: '订单号', name: 'OrderNum' },
            { label: 'VIN号', name: 'VIN' },
            { label: '焊装车型码', name: 'BECarType' },
            { label: '白车身选配码', name: 'BodySelCode' },
            { label: '订单类型', name: 'OrderType' },
            { label: '流水号', name: 'SerialNumber' },
            { label: '车身用途', name: 'BodyUse' },
            { label: 'PLC下发时间', name: 'DownloadTime' },
            { label: '下发状态', name: 'DownloadState' },
            { label: '完工状态', name: 'WorkdoneName' },
            { label: '完工时间', name: 'WorkdoneTime' },
            { label: '基础型码', name: 'CarType' },
            { label: '拉入工位点', name: 'SetIn' },
            { label: '拉出工位点', name: 'SetOut' },
            { label: '拉出原因', name: 'ReportReason' },
            { label: 'MES下发时间', name: 'DateTime' },
            { label: '焊装上线顺序号', name: 'BEOnSeq' },
            { label: '涂装上线顺序号', name: 'PEOnSeq' },
            { label: '总装上线顺序号', name: 'AEOnSeq' },
            { label: 'SAP收车顺序号', name: 'SAPFinSeq' },
            { label: '内饰颜色', name: 'InTrim' },
            { label: '外饰颜色', name: 'Extrim' },
            { label: '主色', name: 'BColor1' },
            { label: '副色', name: 'BColor2' },
            { label: '工厂代码', name: 'Plant' },
            { label: '生产线号', name: 'Line' },
            { label: '项目车标记', name: 'ProFlag' },
            { label: '进焊装日期', name: 'InBETime' },
            { label: '进涂装日期', name: 'InPETime' },
            { label: '进总装日期', name: 'InAETime' },
            { label: '总装收车日期', name: 'AEFinTime' },
            { label: '操作状态', name: 'OperMode' },
            { label: '车身选装包', name: 'OpPac' }
        ];

        this.cols = this.colss[this.reqModel.workShop];
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
        if (this.reqModel.workShop == 'BE') {

            this.QcosShow = false;
        } else {

            this.QcosShow = true;
        }
    }

    dateChange(dm: MuzeyDateModel): void {
        this.reqModel.sTime = dm.sTime;
        this.reqModel.eTime = dm.eTime;
    }

    wsChange(val:any): void {

        this.cols = this.colss[val];

        if (val == 'BE') {

            this.QcosShow = false;
        } else {

            this.QcosShow = true;
        }
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACWorkPlanMQ.GetDatas";
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

    getQcosData(QcosIp: string, QcosJobs: string): void {

        let dialogData = { QcosIp: QcosIp, QcosJobs: QcosJobs, type:'Qcos' };
        this._dialog.open(ACWorkPlanMQEditComponent, { data: dialogData});
    }

    getFData(FamilyCode: string, FeatureCode: string): void {

        let dialogData = { FamilyCode: FamilyCode, FeatureCode: FeatureCode, type:'Family' };
        this._dialog.open(ACWorkPlanMQEditComponent, { data: dialogData });
    }

    workDone(): void {
        abp.message.confirm(
            this.l(
                '订单号:' + this.mtc.itemObj.OrderNum
                + '\r\nVIN:' + this.mtc.itemObj.VIN
                + '\r\n' + (this.reqModel.workShop == 'AE' ? '总装车型码:' : '焊装车型码:') + (this.reqModel.workShop == 'AE' ? this.mtc.itemObj.AECarType : this.mtc.itemObj.BECarType)
                + (this.reqModel.workShop == 'BE' ? '->白车身选配码:' + this.mtc.itemObj.BodySelCode : '')
                        + '\r\n订单类型:' + this.mtc.itemObj.OrderType
                        + '->流水号:' + this.mtc.itemObj.SerialNumber
            ),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACWorkPlanMQ.Workdone";
                    if (!isNullOrUndefined(this.mtc.itemObj.ID)) {
                        this.reqModel.dto = {};
                        this.reqModel.dto.ID = this.mtc.itemObj.ID;
                        serverReq.datas.push(this.reqModel);
                        this._service.req(serverReq).subscribe(result => {
                            abp.notify.success(this.l('标记成功'));
                            this.mtc.pageChangeSub(1);
                        });
                    }
                }
            }
        );
    }

    batAddData(file: string) {

        abp.message.confirm(
            this.l('请确认是否批量导入'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACWorkPlanMQ.UploadExcel";
                    this.reqModel.dto = {};
                    this.reqModel.file = file;
                    serverReq.datas.push(this.reqModel);
                    this._service.req(serverReq).subscribe(result => {
                        abp.notify.success(this.l('导入成功'));
                        this.mtc.pageChangeSub(1);
                    });
                }
            }
        );
    }
}

@Component({
    templateUrl: './ac_workplanmq_edit.component.html'
})
export class ACWorkPlanMQEditComponent extends AppComponentBase implements OnInit {

    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    QcosIpArr: any[] = [];
    QcosJobsArr: any[] = [];
    FamilyCodeArr: any[] = [];
    FamilyNameArr: any[] = [];
    FeatureCodeArr: any[] = [];
    FeatureNameArr: any[] = [];
    totalCount: number;wsDisable: boolean = false;
    filter: string = '';
    fQcosIpArr: any[] = [];
    fQcosJobsArr: any[] = [];
    fFamilyCodeArr: any[] = [];
    fFeatureCodeArr: any[] = [];
    FamilyData: any[] = [];
    width: number;
    cols: ColModel[] = [
        { label: '拧紧枪IP', name: 'QcosIp' },
        { label: '拧紧枪值', name: 'QcosJobs' },
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACWorkPlanMQEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.filterData();
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.action = "BaseData.GetFamilyData";
        this._service.req(serverReq).subscribe(result => {
            for (let i = 0; i < result.datas.length; i++) {
                this.FamilyData[result.datas[i].val] = result.datas[i].text;
                this.mtc.pageChangeSub(1);
            }
        });
    }

    filterData() {

        this.fQcosIpArr = [];
        this.fQcosJobsArr = [];
        this.fFamilyCodeArr = [];
        this.fFeatureCodeArr = [];
        let ii = 0;
        if (this._data.type == 'Qcos') {
            this.width = 550;
            this.cols = [
                { label: '拧紧枪IP', name: 'QcosIp' },
                { label: '拧紧枪值', name: 'QcosJobs' },
            ];
            this.QcosIpArr = this._data.QcosIp.split(',');
            this.QcosJobsArr = this._data.QcosJobs.split(',');
            for (let i = 0; i < this.QcosIpArr.length; i++) {
                if (this.QcosIpArr[i].indexOf(this.filter) >= 0) {
                    this.fQcosIpArr[ii] = this.QcosIpArr[i];
                    this.fQcosJobsArr[ii] = this.QcosJobsArr[i];
                    ii++;
                }
            }
            this.totalCount = this.fQcosIpArr.length;
        } else {
            this.width = 850;
            this.cols = [
                { label: '特征族编码', name: 'FamilyCode' },
                { label: '特征族名称', name: 'FamilyName' },
                { label: '特征值编码', name: 'FeatureCode' },
                { label: '特征值名称', name: 'FeatureName' },
            ];
            this.FamilyCodeArr = this._data.FamilyCode.split(',');
            this.FeatureCodeArr = this._data.FeatureCode.split(',');
            for (let i = 0; i < this.FamilyCodeArr.length; i++) {
                if (this.FamilyCodeArr[i].indexOf(this.filter) >= 0) {
                    this.fFamilyCodeArr[ii] = this.FamilyCodeArr[i];
                    this.fFeatureCodeArr[ii] = this.FeatureCodeArr[i];
                    ii++;
                }
            }
            this.totalCount = this.fFamilyCodeArr.length;
        }
    }

    getDataPage(offset: number): void {

        if (offset < 0) {

            this.filterData();
            offset = 0;
        }

        this.datas = [];
        for (let i = offset; i < offset + this.mtc.pageSize; i++) {

            if (this._data.type == 'Qcos') {

                this.datas.push({ QcosIp: this.fQcosIpArr[i], QcosJobs: this.fQcosJobsArr[i] });
            } else {

                this.datas.push({
                    FamilyCode: this.fFamilyCodeArr[i]
                    , FamilyName: this.FamilyData[this.fFamilyCodeArr[i]]
                    , FeatureCode: this.fFeatureCodeArr[i]
                    , FeatureName: this.FamilyData[this.fFeatureCodeArr[i]]
                });
            }
        }
        this.mtc.isTableLoading = false;
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }
}
