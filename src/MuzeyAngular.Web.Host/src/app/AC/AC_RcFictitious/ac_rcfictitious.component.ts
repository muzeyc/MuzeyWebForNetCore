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
import { ACWorkPlanMQEditComponent } from '@app/AC/AC_WorkPlanMQ/ac_workplanmq.component';
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './ac_rcfictitious.component.html',
    animations: [appModuleAnimation()]
})
export class ACRcFictitiousComponent extends AppComponentBase implements OnInit {

    @ViewChild('mtcLog')
    private mtcLog: MuzeyTableControlsComponent;

    //连接状态
    conState: string;
    plcIp: string;
    readState: string;

    private scanBool: boolean = false;
    Count: string;
    datePipe: DatePipe;

    //一页显示数
    pageSize = 10;

    //当前页数
    curPageNum = 1;
    //是否显示读取效果
    isTableLoading = false;
    //当前选中行
    selectedRowNum: number = 0;
    //当前选中列
    selectedColNum: number = 0;
    //选中VIN
    selectedVIN: string = '';
    runMode: string;

    //当前数据
    itemObj: any;

    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;
    colss: any[] = [];
    cols: ColModel[] = [];
    aOps: MuzeySelectMuzeyModel[] = [
        { text: 'PBS', val: 'PBS' },
        { text: 'WBS', val: 'WBS' }
    ];
    rtOps: MuzeySelectMuzeyModel[] = [
        { text: '物理队列', val: 'RC_Cache' },
        { text: '虚拟队列', val: 'RC_CacheInvented' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        this.datePipe = new DatePipe('zh-Hans');
        this.reqModel.area = "PBS";
        this.reqModel.rcType = "RC_Cache";
        this.colss['PBS'] = [
            { label: '1位', name: 'col01' },
            { label: '2位', name: 'col02' },
            { label: '3位', name: 'col03' },
            { label: '4位', name: 'col04' },
            { label: '5位', name: 'col05' },
            { label: '6位', name: 'col06' },
            { label: '7位', name: 'col07' },
            { label: '8位', name: 'col08' }
        ];

        this.colss['WBS'] = [
            { label: '1位', name: 'col01' },
            { label: '2位', name: 'col02' },
            { label: '3位', name: 'col03' },
            { label: '4位', name: 'col04' },
            { label: '5位', name: 'col05' },
            { label: '6位', name: 'col06' },
            { label: '7位', name: 'col07' },
            { label: '8位', name: 'col08' },
            { label: '9位', name: 'col09' },
            { label: '10位', name: 'col10' },
            { label: '11位', name: 'col11' },
            { label: '12位', name: 'col12' },
            { label: '13位', name: 'col13' },
            { label: '14位', name: 'col14' },
            { label: '15位', name: 'col15' },
            { label: '16位', name: 'col16' },
            { label: '17位', name: 'col17' },
        ];

        this.cols = this.colss['PBS'];
    }

    ngOnInit(): void {
        this.pageChangeSub(1);
        let as = abp.signalr.hubs.common;
        let obj = this;
        as.on("MuzeySignalr", function (user, message: string) {
            //console.warn(message);
            if (message.split('→')[0].split('#')[1] == obj.reqModel.area) {
                message = message.split('→')[1];
            } else {
                return;
            }
            let resArr = message.split(';');
            for (let i = 0; i < resArr.length;i++) {
                let cmdArr = resArr[i].split('#');
                switch (cmdArr[0]) {

                    case 'RCConIp':
                        obj.plcIp = cmdArr[1];
                        break;
                    case 'RCConState':
                        obj.conState = cmdArr[1]=='1' ? '连接正常' : '连接失败';
                        break;
                    case 'RCCurReadFlag':
                        obj.readState = cmdArr[1] == '1' ? '触发中' : '未触发';
                        break;
                    case 'RCReset':
                        obj.pageChangeSub(1);
                        break;
                    case 'RCCurMode':
                        obj.runMode = cmdArr[1] == 'True' ? '接管模式' : '监控模式';
                        break;
                    case 'Count':
                        obj.Count = cmdArr[1];
                        break;
                    case 'RCLog':

                        break;
                }
            }
        });
    }

    lockClick(road: number, type:string, state:string): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.Preinstall";
        this.reqModel.rcType = this.reqModel.rcType;
        this.reqModel.savePreinstallData = {};
        this.reqModel.savePreinstallData.Road = road > 9 ? road : ('0' + road);
        if (type == '0') {

            if (state == 'lock') {

                this.reqModel.savePreinstallData.State = 'C';
            } else {

                this.reqModel.savePreinstallData.State = 'B';
            }
        }

        if (type == '1') {

            if (state == 'lock') {

                this.reqModel.savePreinstallData.State = 'E';
            } else {

                this.reqModel.savePreinstallData.State = 'D';
            }
        }

        this.reqModel.savePreinstallData.Area = this.reqModel.area;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                })
            )
            .subscribe(result => {

                if (result.resStatus == 'err') {
                    this.notify.error(result.resMsg);
                    return;
                }

                this.notify.info(this.l('修改道锁成功'));
                this.pageChangeSub(1);
            });
    }

    changeMode(): void {

        let as = abp.signalr.hubs.common;
        let obj = this;
        as.invoke("ChangeMode", obj.reqModel.area, obj.runMode).catch(function (err) {
            return console.error(err.toString());
        });
    }

    overscan(): void {

        if (this.scanBool) {
            $('#MuzeyTopBar').show();
            $('#MuzeyOutsideSection').show();
            $('#MuzeyContentSection').addClass('content');
            this.exitFullScreen();
        } else {
            $('#MuzeyTopBar').hide();
            $('#MuzeyOutsideSection').hide();
            $('#MuzeyContentSection').removeClass('content');
            this.fullScreen();
        }

        this.scanBool = !this.scanBool;
    }

    fullScreen(): void {
        var el = document.documentElement as any,
            rfs = el.requestFullScreen || el.webkitRequestFullScreen || el.mozRequestFullScreen || el.msRequestFullScreen,
            wscript;

        if (typeof rfs != "undefined" && rfs) {
            rfs.call(el);
            return;
        }
    }

    exitFullScreen(): void {
        var el = document as any,
            cfs = el.cancelFullScreen || el.webkitCancelFullScreen || el.mozCancelFullScreen || el.exitFullScreen,
            wscript;

        if (typeof cfs != "undefined" && cfs) {
            cfs.call(el);
            return;
        }
    }

    getDataValue(data, col): string {

        let val = eval("data." + col.name);
        return val;
    }

    getDataState(data, col): string {

        let val = eval("data.state" + (col.name as string).replace('col',''));
        return val;
    }

    getDataSeq(data, col): string {

        let val = eval("data.seq" + (col.name as string).replace('col', ''));
        return val;
    }

    getFData(): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.GetFData";
        this.reqModel.VIN = this.selectedVIN;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq).subscribe(result => {

            if (result.resStatus == 'err') {

                this.notify.error('错误', result.resMsg);
                return;
            }

            let dialogData = { FamilyCode: result.datas[0].FamilyCode, FeatureCode: result.datas[0].FeatureCode, type: 'Family' };
            this._dialog.open(ACWorkPlanMQEditComponent, { data: dialogData });
        });
    }

    onColClick = function (rowNum,colNum, item) {
        if (this.selectedRowNum == rowNum && this.selectedColNum == colNum) {
            return;
        }
        this.selectedRowNum = rowNum;
        this.selectedColNum = colNum;
        this.itemObj = item;
        this.selectedVIN = eval("item.col" + (Array(2).join('0') + (parseInt(colNum) + 1)).slice(-2));
    }

    aChange(val: any): void {

        this.cols = this.colss[val];
        this.pageChangeSub(1);
        this.conState = '';
        this.plcIp = '';
        this.readState = '';
    }

    rtChange(val: any): void {

        this.pageChangeSub(1);
    }

    pageChangeSub(curPageNum: number) {

        this.selectedRowNum = 0;
        this.selectedColNum = 0;
        this.curPageNum = curPageNum;
        this.isTableLoading = true;
        this.getDataPage((curPageNum - 1) * this.pageSize);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.GetDatas";
        serverReq.offset = offset;
        serverReq.pageSize = this.pageSize;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.isTableLoading = false;
                })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.datas = result.datas;
                this.totalCount = result.totalCount;
                this.selectedVIN = this.datas[0].col01;
            });
    }

    cleanSeq(): void {

        abp.message.confirm(
            this.l('清除出车顺序后需要手动录入顺序号！'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACRcFictitious.CleanSeq";
                    serverReq.datas.push(this.reqModel);
                    this._service.req(serverReq).subscribe(result => {
                        abp.notify.success(this.l('顺序号清除成功请重新录入顺序号！'));
                        this.pageChangeSub(1);
                    });
                }
            }
        );
    }

    PlaceEdit(): void {
        let editDialog;
        editDialog = this._dialog.open(ACRcFictitiousEditComponent,
            {
                data:
                    {
                        Area: this.reqModel.area
                        ,Road: this.selectedRowNum + 1
                        , Place: this.selectedColNum
                        , rcType: this.reqModel.rcType
                    }
            });
        editDialog.afterClosed().subscribe(result => {
            if (result) {
                if (result) {

                    this.pageChangeSub(1);
                }
            }
        });
    }

    Preinstall(): void {
        let editDialog;
        editDialog = this._dialog.open(ACRcFictitiousPreinstallComponent,
            {
                data:
                    {
                        Area: this.reqModel.area
                        , Road: this.selectedRowNum + 1
                        , Place: this.selectedColNum
                        , rcType: this.reqModel.rcType
                    }
            });
        editDialog.afterClosed().subscribe(result => {
            if (result) {
                if (result) {

                    this.pageChangeSub(1);
                }
            }
        });
    }

    Invented(): void {
        let editDialog;
        editDialog = this._dialog.open(ACRcFictitiousInventedComponent,
            {
                data:
                    {
                        Area: this.reqModel.area
                        , Road: this.selectedRowNum + 1
                        , Place: this.selectedColNum
                        , rcType: this.reqModel.rcType
                    }
            });
        editDialog.afterClosed().subscribe(result => {
            this.pageChangeSub(1);
        });
    }

    OutCar(): void {
        this.isTableLoading = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.OutCarPlan";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                this.isTableLoading = false;
                })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                if (result.resStatus == 'ok') {
                    let data = result.datas[0];
                    abp.message.confirm(
                        this.l('出道号:' + data.Road + ' VIN:' + data.VIN + ' 类型:' + data.OutType + ' 确认更新队列?'),
                        (result: boolean) => {
                            if (result) {
                                let serverReq = new MuzeyReqModelOfObject();
                                serverReq.datas = [];
                                serverReq.action = "ACRcFictitious.OutCarUpdateDL";
                                this.reqModel.saveData = {};
                                this.reqModel.saveData.Road = data.Road;
                                this.reqModel.saveData.VIN = data.VIN;
                                serverReq.datas.push(this.reqModel);
                                this._service.req(serverReq).subscribe(result => {
                                    this.pageChangeSub(1);
                                    abp.notify.success(this.l('队列更新成功！'));
                                });
                            }
                        }
                    );
                }
                
                if (result.resStatus == 'err') {
                    this.notify.error(result.resMsg);
                }
            });
    }

    CopyFictitious(): void {
        abp.message.confirm(
            this.l('虚拟队列同步将覆盖当前虚拟队列数据！'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACRcFictitious.CopyFictitious";
                    serverReq.datas.push(this.reqModel);
                    this._service.req(serverReq).subscribe(result => {
                        abp.notify.success(this.l('虚拟队列同步成功！'));
                        this.pageChangeSub(1);
                    });
                }
            }
        );
    }

    delete(id: string): void {

        abp.message.confirm(
            this.l('请确认数据'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACRcFictitious.Delete";
                    if (!isNullOrUndefined(this.itemObj.ID)) {
                        this.reqModel.saveData = {};
                        this.reqModel.saveData.ID = this.itemObj.ID;
                        serverReq.datas.push(this.reqModel);
                        this._service.req(serverReq).subscribe(result => {
                            abp.notify.success(this.l('删除成功'));
                            this.pageChangeSub(1);
                        });
                    }
                }
            }
        );
    }
}

@Component({
    templateUrl: './ac_rcfictitious_edit.component.html'
})
export class ACRcFictitiousEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    roOps = [];
    plOps = [];
    reg: RegExp = new RegExp("^[0-9]*$");
    sOps: MuzeySelectMuzeyModel[] = [
        { text: '正常', val: '0' },
        { text: '冻结', val: '1' }
    ];
    pbsRoOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '1道', val: '01' },
        { text: '2道', val: '02' },
        { text: '3道', val: '03' },
        { text: '4道', val: '04' },
        { text: '5道', val: '05' },
        { text: '6道', val: '06' },
        { text: '7道', val: '07' },
        { text: '8道', val: '08' },
        { text: '9道', val: '09' },
        { text: '10道', val: '10' }
    ];
    wbsRoOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '1道', val: '01' },
        { text: '2道', val: '02' },
        { text: '3道', val: '03' },
        { text: '4道', val: '04' }
    ];

    pbsPlOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '1位', val: '01' },
        { text: '2位', val: '02' },
        { text: '3位', val: '03' },
        { text: '4位', val: '04' },
        { text: '5位', val: '05' },
        { text: '6位', val: '06' },
        { text: '7位', val: '07' },
        { text: '8位', val: '08' }
    ];
    wbsPlOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '1位', val: '01' },
        { text: '2位', val: '02' },
        { text: '3位', val: '03' },
        { text: '4位', val: '04' },
        { text: '5位', val: '05' },
        { text: '6位', val: '06' },
        { text: '7位', val: '07' },
        { text: '8位', val: '08' },
        { text: '9位', val: '09' },
        { text: '10位', val: '10' },
        { text: '11位', val: '11' },
        { text: '12位', val: '12' },
        { text: '13位', val: '13' },
        { text: '14位', val: '14' },
        { text: '15位', val: '15' },
        { text: '16位', val: '16' },
        { text: '17位', val: '17' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACRcFictitiousEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.reqModel.rcType = this._data.rcType;
        this.roOps['PBS'] = this.pbsRoOps;
        this.roOps['WBS'] = this.wbsRoOps;
        this.plOps['PBS'] = this.pbsPlOps;
        this.plOps['WBS'] = this.wbsPlOps;
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.GetData";
        this.reqModel.saveData = {};
        this.reqModel.saveData.Area = this._data.Area;
        this.reqModel.saveData.Road = this._data.Road;
        this.reqModel.saveData.Place = this._data.Place;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq).subscribe(result => {
            this.reqModel.saveData = result.datas[0].saveData;
        });
    }

    save(): void {

        if (!this.reg.test(this.reqModel.saveData.Seq)) {
            this.notify.error('入车顺序号', '请输出纯数字');
            return;
        }

        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.Save";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('队列更新成功'));
                this.close(true);
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }
}

@Component({
    templateUrl: './ac_rcfictitious_preinstall.component.html'
})
export class ACRcFictitiousPreinstallComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    sOps: MuzeySelectMuzeyModel[] = [
        { text: '车道冻结', val: '5' },
        { text: '车道解冻', val: '6' },
        { text: '车辆预冻结', val: '7' },
        { text: '预约快速道', val: '9' },
        { text: '进道预设', val: 'A' },
        { text: '进道锁定', val: 'B' },
        { text: '进道解锁', val: 'C' },
        { text: '出道锁定', val: 'D' },
        { text: '出道解锁', val: 'E' }
    ];
    roOps = [];
    pbsRoOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '1道', val: '01' },
        { text: '2道', val: '02' },
        { text: '3道', val: '03' },
        { text: '4道', val: '04' },
        { text: '5道', val: '05' },
        { text: '6道', val: '06' },
        { text: '7道', val: '07' },
        { text: '8道', val: '08' },
        { text: '9道', val: '09' },
        { text: '10道', val: '10' }
    ];
    wbsRoOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '1道', val: '01' },
        { text: '2道', val: '02' },
        { text: '3道', val: '03' },
        { text: '4道', val: '04' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACRcFictitiousPreinstallComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.savePreinstallData = {};
        this.reqModel.savePreinstallData.Area = this._data.Area;
        this.roOps['PBS'] = this.pbsRoOps;
        this.roOps['WBS'] = this.wbsRoOps;
    }

    ngOnInit(): void {
    }

    save(): void {
        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.Preinstall";
        this.reqModel.rcType = this._data.rcType;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(result => {

                if (result.resStatus == 'err') {
                    this.notify.error(result.resMsg);
                    return;
                }

                this.notify.info(this.l('预设成功'));
                this.close(true);
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }

    rChange(val: any): void {

        this.reqModel.savePreinstallData.Road = '';
    }
}

@Component({
    templateUrl: './ac_rcfictitious_invented.component.html'
})
export class ACRcFictitiousInventedComponent extends AppComponentBase implements OnInit {

    saving = false;
    title: string;
    reqModel: any = {};
    vin: string;
    datas: any[] = [];
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    totalCount: number;
    cols: ColModel[] = [
        { label: '道号', name: 'Road' },
        { label: 'VIN', name: 'VIN' },
        { label: '方向', name: 'OutType' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACRcFictitiousPreinstallComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.savePreinstallData = {};
        this.reqModel.savePreinstallData.Area = this._data.Area;
        this.reqModel.area = this._data.Area;
        this.title = this._data.Area + '模拟排产';
    }

    ngOnInit(): void {
    }

    save(): void {
        //this.saving = true;
        //let serverReq = new MuzeyReqModelOfObject();
        //serverReq.datas = [];
        //serverReq.action = "ACRcFictitious.Preinstall";
        //serverReq.datas.push(this.reqModel);
        //this._service.req(serverReq)
        //    .pipe(
        //        finalize(() => {
        //            this.saving = false;
        //        })
        //    )
        //    .subscribe(() => {
        //        this.notify.info(this.l('预设成功'));
        //        this.close(true);
        //    });
    }

    inroadpc(vin: string): void {

        this.mtc.isTableLoading = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.InRoadPc";
        this.reqModel.VIN = this.vin;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.mtc.isTableLoading = false;
                })
            )
            .subscribe((result: MuzeyResModelOfObject) => {

                if (result.resStatus == 'err') {
                    this.notify.error(result.resMsg);
                } else {
                    this.datas = result.datas;
                    this.totalCount = result.datas.length;
                }
            });
    }

    invented(): void {
        this.mtc.isTableLoading = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRcFictitious.Invented";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
            finalize(() => {
                this.mtc.isTableLoading = false;
                })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.datas = result.datas;
                this.totalCount = result.datas.length;

                if (result.resStatus == 'err') {
                    this.notify.error(result.resMsg);
                }
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }

    getDataPage(offset: number): void {

        this.mtc.isTableLoading = false;
    }
}
