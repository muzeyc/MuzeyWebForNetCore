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
    templateUrl: './ac_calendars.component.html',
})
export class ACCalendarsComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [
        { label: '车间', name: 'WorkShop' },
        { label: '生产时间', name: 'workDay' },
        { label: '班次名', name: 'ShiftName' },
        //{ label: '班次开始时间', name: 'BeginTime' },
        //{ label: '班次结束时间', name: 'EndTime' }
        { label: '班次开始时间', name: 'sT' },
        { label: '班次结束时间', name: 'eT' }
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
        serverReq.action = "ACCalendars.GetDatas";
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
            editDialog = this._dialog.open(ACCalendarsEditComponent, { data: { workShop: this.reqModel.workShop } });
        } else {
            editDialog = this._dialog.open(ACCalendarsEditComponent, {
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
                    serverReq.action = "ACCalendars.Delete";
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
    templateUrl: './ac_calendars_edit.component.html'
})
export class ACCalendarsEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' },
        { text: '冲压', val: 'SE' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACCalendarsEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACCalendars.GetData";
        if (!isNullOrUndefined(this._data.id)) {
            this.reqModel.saveData = {};
            this.reqModel.saveData.ID = this._data.id;
            serverReq.datas.push(this.reqModel);
            this._service.req(serverReq).subscribe(result => {
                this.reqModel.saveData = result.datas[0];
                this.reqModel.saveData.BeginTime = this.reqModel.saveData.BeginTime.split('T')[1];
                this.reqModel.saveData.EndTime = this.reqModel.saveData.EndTime.split('T')[1];
            });
        }
    }

    save(): void {
        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACCalendars.Save";
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
