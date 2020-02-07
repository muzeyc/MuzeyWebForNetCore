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
    templateUrl: './ac_lineplanmq.component.html',
})
export class ACLinePlanMQComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;wsDisable: boolean = false;
    cols: ColModel[] = [
        { label: '订单号', name: 'OrderNum' },
        { label: 'VIN号', name: 'VIN' },
        { label: '焊装车型码', name: 'BECarType' },
        { label: '白车身选配码', name: 'BodySelCode' },
        { label: '订单类型', name: 'OrderType' },
        { label: '流水号', name: 'SerialNumber' },
        { label: 'PLC下发时间', name: 'DownloadTime' },
        { label: '下发状态', name: 'DownloadState' },
        { label: '完工状态', name: 'WorkdoneName' },
        { label: '完工时间', name: 'WorkdoneTime' },
        { label: '基础型码', name: 'CarType' },
        { label: '拉入工位点', name: 'SetIn' },
        { label: '拉出工位点', name: 'SetOut' },
        { label: '拉出原因', name: 'ReportReason' },
        { label: 'MES下发时间', name: 'DateTime' },
        { label: '分总成顺序号', name: 'BESubOnSeq' },
        { label: '工厂代码', name: 'Plant' },
        { label: '生产线号', name: 'Line' },
        { label: '项目车标记', name: 'ProFlag' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: '焊装', val: 'BE' }
    ];
    lineOps: MuzeySelectMuzeyModel[] = [
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
        { text: '(RDR)右后门', val: 'RDR' }
    ];
    wdOps: MuzeySelectMuzeyModel[] = [
        { text: '', val: '' },
        { text: '完工', val: '1' },
        { text: '未完工', val: '0' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        if (this.appSession.Person.permissionType == '1' || this.appSession.Person.permissionType == '3') {this.reqModel.workShop = this.appSession.Person.workShop;this.wsDisable = true;} else {this.reqModel.workShop = this.wsOps[0].val;}
        //let serverReq = new MuzeyReqModelOfObject();
        //serverReq.datas = [];
        //serverReq.action = "BaseData.GetLines";
        //serverReq.datas.push(this.reqModel);
        //this._service.req(serverReq)
        //    .pipe(
        //        finalize(() => {
                    
        //        })
        //    )
        //    .subscribe((result: MuzeyResModelOfObject) => {
        //        this.lineOps = result.datas;
        //    });
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
        serverReq.action = "ACLinePlanMQ.GetDatas";
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

    workDone(): void {
        abp.message.confirm(
            this.l('请确认数据'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACLinePlanMQ.Workdone";
                    if (!isNullOrUndefined(this.mtc.itemObj.ID)) {
                        this.reqModel.dto = {};
                        this.reqModel.dto.ID = this.mtc.itemObj.ID;
                        serverReq.datas.push(this.reqModel);
                        this._service.req(serverReq).subscribe(result => {
                            abp.notify.success(this.l('标记成功'));
                            this.mtc.pageChangeSub(1);
                        });
                    }
                }
            }
        );
    }

    batAddData(file: string) {

        abp.message.confirm(
            this.l('请确认是否批量导入'),
            (result: boolean) => {
                if (result) {
                    let serverReq = new MuzeyReqModelOfObject();
                    serverReq.datas = [];
                    serverReq.action = "ACLinePlanMQ.UploadExcel";
                    this.reqModel.dto = {};
                    this.reqModel.file = file;
                    serverReq.datas.push(this.reqModel);
                    this._service.req(serverReq).subscribe(result => {
                        abp.notify.success(this.l('导入成功'));
                        this.mtc.pageChangeSub(1);
                    });
                }
            }
        );
    }
}
