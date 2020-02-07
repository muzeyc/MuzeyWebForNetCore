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
    templateUrl: './ac_offlinecar.component.html',
})
export class ACOfflineCarComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' }
    ];
    QcosShow: boolean = true;

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') {this.reqModel.workShop = this.appSession.Person.workShop;this.wsDisable = true;} else {this.reqModel.workShop = this.wsOps[0].val;}
        this.cols['AE'] = [
            { label: '订单号', name: 'OrderNum' },
            { label: 'VIN号', name: 'VIN' },
            { label: '总装车型码', name: 'AECarType' },
            { label: '订单类型', name: 'OrderType' },
            { label: '流水号', name: 'SerialNumber' },
            { label: '车身用途', name: 'BodyUse' },
            { label: '离线时间', name: 'SetOutTime' },
            { label: '基础型码', name: 'CarType' },
            { label: '总装上线顺序号', name: 'AEOnSeq' },
            { label: 'SAP收车顺序号', name: 'SAPFinSeq' },
            { label: '车身选装包', name: 'OpPac' },
            { label: '内饰颜色', name: 'InTrim' },
            { label: '外饰颜色', name: 'Extrim' },
            { label: '主色', name: 'BColor1' },
            { label: '副色', name: 'BColor2' },
            { label: '工厂代码', name: 'Plant' },
            { label: '生产线号', name: 'Line' },
            { label: '项目车标记', name: 'ProFlag' },
            //{ label: '进总装日期', name: 'InAETime' },
            //{ label: '总装收车日期', name: 'AEFinTime' }
        ];

        this.cols['BE'] = [
            { label: '订单号', name: 'OrderNum' },
            { label: 'VIN号', name: 'VIN' },
            { label: '焊装车型码', name: 'BECarType' },
            { label: '白车身选配码', name: 'BodySelCode' },
            { label: '订单类型', name: 'OrderType' },
            { label: '流水号', name: 'SerialNumber' },
            { label: '车身用途', name: 'BodyUse' },
            { label: '离线时间', name: 'SetOutTime' },
            { label: '基础型码', name: 'CarType' },
            { label: '焊装上线顺序号', name: 'BEOnSeq' },
            { label: '涂装上线顺序号', name: 'PEOnSeq' },
            { label: '总装上线顺序号', name: 'AEOnSeq' },
            { label: 'SAP收车顺序号', name: 'SAPFinSeq' },
            { label: '车身选装包', name: 'OpPac' },
            { label: '内饰颜色', name: 'InTrim' },
            { label: '外饰颜色', name: 'Extrim' },
            { label: '主色', name: 'BColor1' },
            { label: '副色', name: 'BColor2' },
            { label: '工厂代码', name: 'Plant' },
            { label: '生产线号', name: 'Line' },
            { label: '项目车标记', name: 'ProFlag' },
            //{ label: '进焊装日期', name: 'InBETime' },
            //{ label: '进涂装日期', name: 'InPETime' },
            //{ label: '进总装日期', name: 'InAETime' },
            //{ label: '总装收车日期', name: 'AEFinTime' }
        ];
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
        if (this.reqModel.workShop == 'BE') {

            this.QcosShow = false;
        } else {

            this.QcosShow = true;
        }
    }

    wsChange(val: any): void {
        if (val == 'BE') {

            this.QcosShow = false;
        } else {

            this.QcosShow = true;
        }
        this.mtc.pageChangeSub(1);
    }

    dateChange(dm: MuzeyDateModel): void {
        this.reqModel.sTime = dm.sTime;
        this.reqModel.eTime = dm.eTime;
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACOfflineCar.GetDatas";
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
            editDialog = this._dialog.open(ACOfflineCarEditComponent, { data: { workShop: this.reqModel.workShop } });	
        } else {
            editDialog = this._dialog.open(ACOfflineCarEditComponent, {
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
                    serverReq.action = "ACOfflineCar.Delete";
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

    changeState(id: string, operMode:string): void {

        abp.message.confirm(
            this.l('请确认数据'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACOfflineCar.Save";
                    if (!isNullOrUndefined(this.mtc.itemObj.ID)) {
                        this.reqModel.saveData = {};
                        this.reqModel.saveData.ID = this.mtc.itemObj.ID;
                        this.reqModel.saveData.OperMode = operMode;
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

@Component({
    templateUrl: './ac_offlinecar_edit.component.html'
})
export class ACOfflineCarEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACOfflineCarEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.reqModel.workShop = _data.workShop;
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACOfflineCar.GetData";
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
        serverReq.action = "ACOfflineCar.Save";
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
