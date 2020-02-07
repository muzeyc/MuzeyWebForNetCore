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
    templateUrl: './ac_mailadressee.component.html',
})
export class ACMailAdresseeComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;
    cols: ColModel[] = [
        { label: '车间', name: 'WorkShop' },
        { label: '收件人账号', name: 'Adressee' },
        { label: '报警类型编码', name: 'AlarmTypeCode' },
        { label: '报警类型描述', name: 'AlarmTypeDesc' },
        { label: '发送状态', name: 'AdresseeStateStr' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
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
        //this.reqModel.workShop = "AE";
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACMailAdressee.GetDatas";
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
            editDialog = this._dialog.open(ACMailAdresseeEditComponent, { data: { workShop: this.reqModel.workShop }});
        } else {
            editDialog = this._dialog.open(ACMailAdresseeEditComponent, {
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
                    serverReq.action = "ACMailAdressee.Delete";
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
    templateUrl: './ac_mailadressee_edit.component.html'
})
export class ACMailAdresseeEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' },
        { text: '冲压', val: 'SE' }
    ];
    atOps: MuzeySelectMuzeyModel[] = [];
    asOps: MuzeySelectMuzeyModel[] = [
        { text: '发送', val: '1' },
        { text: '不发送', val: '0' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACMailAdresseeEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
        this.reqModel.saveData.AdresseeState = '1';
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "BaseData.GetAlarmTypes";
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq).subscribe(result => {
            this.atOps = result.datas;
        });
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACMailAdressee.GetData";
        if (!isNullOrUndefined(this._data.id)) {
            this.reqModel.saveData = {};
            this.reqModel.saveData.ID = this._data.id;
            serverReq.datas.push(this.reqModel);
            this._service.req(serverReq).subscribe(result => {
                this.reqModel.saveData = result.datas[0];
                this.reqModel.saveData.AdresseeState = result.datas[0].AdresseeState == 0 ? '0' : '1';
                if (this.reqModel.saveData.WorkShop != '') {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "BaseData.GetAlarmTypes";
                    this.reqModel.workShop = this.reqModel.saveData.WorkShop;
                    serverReq.datas.push(this.reqModel);
                    this._service.req(serverReq).subscribe(result => {
                        this.atOps = result.datas;
                    });
                }
            });
        }
    }

    save(): void {
        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACMailAdressee.Save";
        let check: boolean = false;
        for (let i = 0; i < this.atOps.length; i++) {
            if (this.atOps[i].val == this.reqModel.saveData.AlarmTypeCode) {
                this.reqModel.saveData.AlarmTypeDesc = this.atOps[i].text;
                check = true;
            }
        }
        if (!check) {
            this.reqModel.saveData.AlarmTypeDesc = '';
        }
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

    wsChange(val: any): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "BaseData.GetAlarmTypes";
        this.reqModel.workShop = val;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq).subscribe(result => {
            this.atOps = result.datas;
        });
    }
}
