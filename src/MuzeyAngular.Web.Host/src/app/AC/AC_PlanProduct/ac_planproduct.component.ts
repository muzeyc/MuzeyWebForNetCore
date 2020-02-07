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
    templateUrl: './ac_planproduct.component.html',
})
export class ACPlanProductComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [
        { label: '车间', name: 'WorkShop' },
        { label: '主线', name: 'LineMain' },
        { label: '分线', name: 'LineBranch' },
        { label: '车型', name: 'CarType' },
        { label: '计划产量', name: 'PlanProduct' },
        { label: '日期', name: 'PlanDate' },
        { label: '班次名称', name: 'ShiftrestName' },
        { label: '开始时间', name: 'PlanTimeS' },
        { label: '结束时间', name: 'PlanTimeE' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' }
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

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPlanProduct.GetDatas";
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
            editDialog = this._dialog.open(ACPlanProductEditComponent, { data: { workShop: this.reqModel.workShop }});
        } else {
            editDialog = this._dialog.open(ACPlanProductEditComponent, {
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
                    serverReq.action = "ACPlanProduct.Delete";
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
    templateUrl: './ac_planproduct_edit.component.html'
})
export class ACPlanProductEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    wsDisable: boolean = false;
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' }
    ];
    mOps: MuzeySelectMuzeyModel[] = [
        { text: '地板', val: '地板' },
        { text: '车身', val: '车身' },
        { text: '侧围', val: '侧围' },
        { text: '门盖', val: '门盖' },
        { text: '表调', val: '表调' }
    ];
    bOps: MuzeySelectMuzeyModel[] = [];
    bData: any[] = [];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACPlanProductEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
        this.reqModel.saveData.LineMain = '地板';
        this.bData["地板"] = [
            { text: 'DA', val: 'DA' },
            { text: 'MC', val: 'MC' },
            { text: 'FF', val: 'FF' },
            { text: 'RC', val: 'RC' },
            { text: 'UB', val: 'UB' },
            { text: 'UR1', val: 'UR1' },
            { text: 'UR2', val: 'UR2' }
        ];
        this.bData["车身"] = [
            { text: 'FI', val: 'FI' },
            { text: 'RL2', val: 'RL2' },
            { text: 'FO', val: 'FO' },
            { text: 'RL4', val: 'RL4' },
            { text: 'RL5', val: 'RL5' },
            { text: 'RL6', val: 'RL6' }
        ];
        this.bData["侧围"] = [
            { text: 'SIL', val: 'SIL' },
            { text: 'SIR', val: 'SIR' },
            { text: 'SOL', val: 'SOL' },
            { text: 'SOR', val: 'SOR' }
        ];
        this.bData["门盖"] = [
            { text: 'FDL', val: 'FDL' },
            { text: 'FDR', val: 'FDR' },
            { text: 'RDL', val: 'RDL' },
            { text: 'RDR', val: 'RDR' },
            { text: 'HD', val: 'HD' },
            { text: 'DL', val: 'DL' },
            { text: 'RF', val: 'RF' }
        ];
        this.bData["表调"] = [
            { text: 'MF1', val: 'MF1' },
            { text: 'MF3', val: 'MF3' }
        ];

        this.bOps = this.bData['地板'];
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPlanProduct.GetData";
        if (!isNullOrUndefined(this._data.id)) {
            this.reqModel.saveData = {};
            this.reqModel.saveData.ID = this._data.id;
            serverReq.datas.push(this.reqModel);
            this._service.req(serverReq).subscribe(result => {
                this.reqModel.saveData = result.datas[0];
            });
        } else {
            let serverReq = new MuzeyReqModelOfObject();
            serverReq.datas = [];
            serverReq.action = "ACPlanProduct.GetTime";
            this.reqModel.workShop = this._data.workShop;
            serverReq.datas.push(this.reqModel);
            this._service.req(serverReq).subscribe(result => {
                this.reqModel.saveData = result.datas[0];
            });
        }

        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3')
        {
            this.reqModel.workShop = this.appSession.Person.workShop;
            this.wsDisable = true;
        } 
    }

    mainLineChange(val: string) {

        this.bOps = this.bData[val];
        this.reqModel.saveData.LineBranch = '';
    }

    save(): void {
        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPlanProduct.Save";
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
