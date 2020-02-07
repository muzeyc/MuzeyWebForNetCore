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
    templateUrl: './ac_rclog.component.html',
})
export class ACRCLogComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;
    cols: ColModel[] = [
        { label: '区域', name: 'Area' },
        { label: '车道', name: 'Road' },
        { label: '车位', name: 'Place' },
        { label: 'VIN', name: 'VIN' },
        { label: '状态', name: 'State' },
        //{ label: '操作时间', name: 'OpTime' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: 'PBS', val: 'PBS' },
        { text: 'WBS', val: 'WBS' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRCLog.GetDatas";
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
            editDialog = this._dialog.open(ACRCLogEditComponent, { data: { workShop: this.reqModel.workShop }});
        } else {
            editDialog = this._dialog.open(ACRCLogEditComponent, {
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
                    serverReq.action = "ACRCLog.Delete";
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
    templateUrl: './ac_rclog_edit.component.html'
})
export class ACRCLogEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    roOps = [];
    plOps = [];
    arOps: MuzeySelectMuzeyModel[] = [
        { text: 'PBS', val: 'PBS' },
        { text: 'WBS', val: 'WBS' }
    ];
    sOps: MuzeySelectMuzeyModel[] = [
        //{ text: '车道冻结', val: '5' },
        { text: '车辆预冻结', val: '7' },
        { text: '预约快速道', val: '9' },
        { text: '进道预设', val: 'A' }
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
        private _dialogRef: MatDialogRef<ACRCLogEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.roOps['PBS'] = this.pbsRoOps;
        this.roOps['WBS'] = this.wbsRoOps;
        this.plOps['PBS'] = this.pbsPlOps;
        this.plOps['WBS'] = this.wbsPlOps;
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRCLog.GetData";
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
        serverReq.action = "ACRCLog.Save";
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
