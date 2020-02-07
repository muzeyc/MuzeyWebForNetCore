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
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './ac_seplanorder.component.html',
})
export class ACSEPlanOrderComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [
        { label: '工厂代码', name: 'Plant' },
        { label: '生产线号', name: 'Line' },
        { label: '订单号', name: 'OrderNum' },
        { label: '订单类型', name: 'OrderType' },
        { label: '冲压件编码', name: 'ProNum' },
        { label: '冲压件名称', name: 'ProName' },
        { label: '冲压件计划数量', name: 'PlanCount' },
        { label: '计划的模具组编号', name: 'MouldCode' },
        { label: '计划的配方号', name: 'Formula' },
        { label: '计划的节拍', name: 'SPM' },
        { label: '冲压上线顺序号', name: 'SEOnSeq' },
        { label: '计划生产日期', name: 'PlanDate' },
        { label: '计划开始生产时间', name: 'PlanStartTime' },
        { label: '计划结束生产时间', name: 'PlanStopTime' },
        { label: '类型', name: 'Type' },
        { label: '车系编码', name: 'CarType' },
        { label: '冻结状态', name: 'FreezeState' }
        //{ label: '备注', name: 'Remarks' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '冲压', val: 'SE' },
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
        serverReq.action = "ACSEPlanOrder.GetDatas";
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

    changeState(id: string, freezeState: string): void {

        if (freezeState == '0') {

            let editDialog;
            if (isNullOrUndefined(id)) {
                editDialog = this._dialog.open(ACSEPlanOrderEditComponent, { data: { workShop: this.reqModel.workShop } });
            } else {
                editDialog = this._dialog.open(ACSEPlanOrderEditComponent, {
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
        } else {

            abp.message.confirm(
                this.l('请确认数据'),
                (result: boolean) => {
                    if (result) {
                        let serverReq = new MuzeyReqModelOfObject();
                        serverReq.datas = [];
                        serverReq.action = "ACSEPlanOrder.Save";
                        if (!isNullOrUndefined(this.mtc.itemObj.ID)) {
                            this.reqModel.saveData = {};
                            this.reqModel.saveData.ID = this.mtc.itemObj.ID;
                            this.reqModel.saveData.FreezeState = freezeState;
                            serverReq.datas.push(this.reqModel);
                            this._service.req(serverReq).subscribe(result => {
                                abp.notify.success(this.l('状态更新成功'));
                                this.mtc.pageChangeSub(1);
                            });
                        }
                    }
                }
            );
        }
    }
}

@Component({
    templateUrl: './ac_seplanorder_edit.component.html'
})
export class ACSEPlanOrderEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    reg: RegExp = new RegExp("^[0-9]*$");
    datePipe: DatePipe;

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACSEPlanOrderEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.datePipe = new DatePipe('zh-Hans');
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACSEPlanOrder.GetData";
        if (!isNullOrUndefined(this._data.id)) {
            this.reqModel.saveData = {};
            this.reqModel.saveData.ID = this._data.id;
            serverReq.datas.push(this.reqModel);
            this._service.req(serverReq).subscribe(result => {
                this.reqModel.saveData = result.datas[0];
            });
        }
    }

    save(): void {

        if (!this.reg.test(this.reqModel.saveData.SEOnSeq)) {
            this.notify.error('错误', '请输出纯数字');
            return;
        }

        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACSEPlanOrder.Save";
        this.reqModel.saveData.FreezeState = '0';
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('保存成功'));
                this.close(true);
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }
}
