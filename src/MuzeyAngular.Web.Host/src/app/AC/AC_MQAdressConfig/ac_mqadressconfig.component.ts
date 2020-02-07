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
    templateUrl: './ac_mqadressconfig.component.html',
})
export class ACMQAdressConfigComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    wsCols: any[] = [];
    beCols: ColModel[];
    cols: ColModel[] = [];
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

        this.wsCols["AE"] = [
            { label: '车间', name: 'WorkShop' },
            { label: '配置文件一级类别', name: 'TopCategory' },
            { label: '配置文件二级类别', name: 'Reclassify' },
            { label: '生产线', name: 'Line' },
            { label: '工位', name: 'Station' },
            { label: '线体类型', name: 'LineType' },
            //{ label: '关键点类型', name: 'KeyPoint' },
            { label: '关键点类型', name: 'KeyPointName' },
        ];

        this.wsCols["BE"] = [
            { label: '车间', name: 'WorkShop' },
            { label: '配置文件类别', name: 'ConfigType' },
            { label: '生产线', name: 'Line' },
            { label: '工位', name: 'Station' },
            { label: '线体类型', name: 'LineType' },
            //{ label: '关键点类型', name: 'KeyPoint' },
            { label: '关键点类型', name: 'KeyPointName' },
            { label: '备注', name: 'Remarks' }
        ];


        this.wsChange('AE');
    }

    wsChange(val: string) {

        this.cols = this.wsCols[val];
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACMQAdressConfig.GetDatas";
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
            editDialog = this._dialog.open(ACMQAdressConfigEditComponent, { data: { workShop: this.reqModel.workShop } });	
        } else {
            editDialog = this._dialog.open(ACMQAdressConfigEditComponent, {
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
                    serverReq.action = "ACMQAdressConfig.Delete";
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
    templateUrl: './ac_mqadressconfig_edit.component.html'
})
export class ACMQAdressConfigEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    wd: string;

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACMQAdressConfigEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
        if (_data.workShop == 'AE') {

            this.wd = '700';
        } else {

            this.wd = '900';
        }
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACMQAdressConfig.GetData";
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
        serverReq.action = "ACMQAdressConfig.Save";
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
