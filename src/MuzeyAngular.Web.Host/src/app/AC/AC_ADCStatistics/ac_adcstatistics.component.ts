import { Component, Injector, OnInit, ViewChild, Optional, Inject, AfterViewInit } from '@angular/core';
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
import { Chart } from 'chart.js'

@Component({
    templateUrl: './ac_adcstatistics.component.html',
})
export class ACADCStatisticsComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    colors: any[] = ['rgb(255, 99, 132)', 'rgb(255, 159, 64)', 'rgb(255, 205, 86)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)', 'rgb(201, 203, 207)'];
    totalCount: number; wsDisable: boolean = false;
    cols: ColModel[] = [
        { label: '模具编号', name: 'MouldNumber' },
        { label: '线体', name: 'LineCodeName' },
        { label: '换模次数', name: 'ADCNum' },
        { label: '换模成功率', name: 'ADCSucScale' },
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
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

    ngAfterViewInit(): void {
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACADCStatistics.GetDatas";
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

                let dataDonut: any[] = [];
                let dataBar: any[] = [];
                for (let i = 0; i < this.datas.length; i++) {

                    dataDonut.push({ label: '模具编号:' + this.datas[i].MouldNumber, value: this.datas[i].ADCSucScale.split('%')[0] });
                    dataBar.push({ label: '模具编号:' + this.datas[i].MouldNumber, value: this.datas[i].ADCNum });
                }
                ((window as any).Morris).Donut({
                    element: 'donut_chart',
                    data: dataDonut,
                    //colors: this.colors,
                    formatter: function (y) {
                        return y + '%';
                    }
                });

                ((window as any).Morris).Bar({
                    element: 'line_chart',
                    data: dataBar,
                    //colors: this.colors,
                    xkey: 'label',
                    ykeys: ['value'],
                    labels: ['换模次数']
                });
            });
    }
}
