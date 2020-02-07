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
    templateUrl: './ac_linese.component.html',
})
export class ACLineSEComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number; wsDisable: boolean = false;
    title: string = '上下线统计';
    colss: any[] = [];
    cols: ColModel[] = [];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '总装', val: 'AE' },
        { text: '焊装', val: 'BE' }
    ];
    ltnOps: MuzeySelectMuzeyModel[] = [
        { text: '主线', val: '主线' },
        { text: '分线', val: '分线' }
    ];

    lineOps: any[] = [];

    belineOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '(DA)前围', val: 'DA' },
        { text: '(MC)机舱', val: 'MC' },
        { text: '(FF)前地板', val: 'FF' },
        { text: '(RC)后地板', val: 'RC' },
        { text: '(SIL)左侧围内板', val: 'SIL' },
        { text: '(SIR)右侧围内板', val: 'SIR' },
        { text: '(SOL)左侧围外板', val: 'SOL' },
        { text: '(SOR)右侧围外板', val: 'SOR' },
        { text: '(RF)顶盖', val: 'RF' },
        { text: '(HD)前盖', val: 'HD' },
        { text: '(DL)后举门', val: 'DL' },
        { text: '(FDL)左前门', val: 'FDL' },
        { text: '(FDR)右前门', val: 'FDR' },
        { text: '(RDL)左后门', val: 'RDL' },
        { text: '(RDR)右后门', val: 'RDR' },
        { text: '(UB)地板点定', val: 'UB' },
        { text: '(UR1)地板补焊1线', val: 'UR1' },
        { text: '(UR2)地板补焊2线', val: 'UR2' },
        { text: '(FI)车身内总拼线', val: 'FI' },
        { text: '(RL2)车身补焊2线', val: 'RL2' },
        { text: '(FO)车身外总拼线', val: 'FO' },
        { text: '(RL4)车身补焊4线', val: 'RL4' },
        { text: '(RL5)车身补焊5线', val: 'RL5' },
        { text: '(RL6)车身补焊6线', val: 'RL6' },
        { text: '(MF1)装配线', val: 'MF1' },
        { text: '(MF3)表调线', val: 'MF3' }
    ];

    aelineOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: 'AGV动力总成', val: '6' },
        { text: '车门装配', val: '7' },
        { text: '仪表分装', val: '8' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') { this.reqModel.workShop = this.appSession.Person.workShop; this.wsDisable = true; } else { this.reqModel.workShop = this.wsOps[0].val; }
        this.colss['AE'] = [
            { label: '订单号', name: 'orderNum' },
            { label: 'VIN', name: 'vin' },
            { label: '总装车型码', name: 'wsCarType' },
            { label: '订单类型', name: 'orderType' },
            //{ label: '白车身选装码', name: 'bodySelCode' },
            //{ label: '车身用途', name: 'bodyUse' },
            { label: '流水号', name: 'serialNumber' },
            //{ label: '产线分类编码', name: 'lineTypeCode' },
            //{ label: '产线分类', name: 'lineTypeName' },
            { label: '产线编码', name: 'line' },
            { label: '产线名称', name: 'lineName' },
            { label: '基础车型码', name: 'carType' },
            { label: '上线时间', name: 'sTime' },
            { label: '完成时间', name: 'eTime' }
        ];

        this.colss['BE'] = [
            { label: '订单号', name: 'orderNum' },
            { label: 'VIN', name: 'vin' },
            { label: '焊装车型码', name: 'wsCarType' },
            { label: '白车身选装码', name: 'bodySelCode' },
            { label: '订单类型', name: 'orderType' },
            { label: '车身用途', name: 'bodyUse' },
            { label: '流水号', name: 'serialNumber' },
            //{ label: '产线分类编码', name: 'lineTypeCode' },
            //{ label: '产线分类', name: 'lineTypeName' },
            { label: '产线编码', name: 'line' },
            { label: '产线名称', name: 'lineName' },
            { label: '基础车型码', name: 'carType' },
            { label: '上线时间', name: 'sTime' },
            { label: '完成时间', name: 'eTime' }
        ];

        this.cols = this.colss[this.reqModel.workShop];

        this.lineOps['AE'] = this.aelineOps;
        this.lineOps['BE'] = this.belineOps;
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
        serverReq.action = "ACLineSE.GetDatas";
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
                this.title = '上下线统计(共' + this.totalCount + '条记录)';
            });
    }

    wsChange(val: any): void {

        this.cols = this.colss[val];
        this.mtc.pageChangeSub(1);
    }
}
