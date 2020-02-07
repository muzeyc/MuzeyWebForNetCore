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
    templateUrl: './ac_personinfo.component.html',
})
export class ACPersonInfoComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [
        { label: '名字', name: 'PersonName' },
        { label: '工厂', name: 'Factory' },
        { label: '车间', name: 'WorkShop' },
        { label: '账号', name: 'UserId' },
        { label: '权限等级', name: 'PermissionType' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' },
        { text: '冲压', val: 'SE' },
        { text: '物流', val: 'Logistics' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3' || this.appSession.Person.permissionType == '4')
        {
            this.reqModel.workShop = this.appSession.Person.workShop; this.wsDisable = true;
        } else
        {
            this.reqModel.workShop = this.wsOps[0].val;
        }
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPersonInfo.GetDatas";
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
            editDialog = this._dialog.open(ACPersonInfoEditComponent, { data: { workShop: this.reqModel.workShop }});
        } else {
            if (this.mtc.itemObj.PersonName == this.appSession.Person.personName) {
                this.notify.error('错误', '只有系统管理员能编辑你');
                return;
            }
            editDialog = this._dialog.open(ACPersonInfoEditComponent, {
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
                    serverReq.action = "ACPersonInfo.Delete";
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
    templateUrl: './ac_personinfo_edit.component.html'
})
export class ACPersonInfoEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    wsDisable: boolean = false;
    fOps: MuzeySelectMuzeyModel[] = [];
    wsOps: MuzeySelectMuzeyModel[] = [];
    dOps: MuzeySelectMuzeyModel[] = [];
    aOps: MuzeySelectMuzeyModel[] = [];
    pOps: MuzeySelectMuzeyModel[] = [
        { text: '一级管理员(无限制)', val: '0' },
        { text: '用户(车间+指定画面)', val: '1' },
        { text: '用户(指定画面)', val: '2' },
        { text: '二级管理员(车间+无限制)', val: '3' },
        { text: '二级管理员(部门+无限制)', val: '4' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACPersonInfoEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};

        this.reqModel.Views = {};

        this.reqModel.workShop = _data.workShop;

        this.reqModel.saveData.PermissionType = '0';
        this.reqModel.saveData.ID = _data.id;
        this.reqModel.id = this.appSession.Person.userId;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPersonInfo.GetSelectData";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .pipe(
                finalize(() => { })
            )
            .subscribe((result: MuzeyResModelOfObject) => {
                this.fOps = result.datas[0].fOps;
                this.wsOps = result.datas[0].wsOps;
                this.dOps = result.datas[0].dOps;
                this.aOps = result.datas[0].aOps;
                this.reqModel.Views = result.datas[0].Views;
                if (this.appSession.Person.permissionType == '3') {

                    this.pOps = [
                        { text: '用户(车间+指定画面)', val: '1' }
                    ];

                    this.reqModel.saveData.PermissionType = '1';
                    this.wsDisable = true;

                    this.reqModel.saveData.Factory = this.appSession.Person.factory;
                    this.reqModel.saveData.WorkShop = this.appSession.Person.workShop;
                    this.reqModel.saveData.Department = this.appSession.Person.department;
                }
                if (this.appSession.Person.permissionType == '4') {

                    this.pOps = [
                        { text: '用户(指定画面)', val: '2' }
                    ];

                    this.reqModel.saveData.PermissionType = '2';
                    this.wsDisable = true;

                    this.reqModel.saveData.Factory = this.appSession.Person.factory;
                    this.reqModel.saveData.WorkShop = this.appSession.Person.workShop;
                    this.reqModel.saveData.Department = this.appSession.Person.department;
                }
            });
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPersonInfo.GetData";
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
        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACPersonInfo.Save";
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
