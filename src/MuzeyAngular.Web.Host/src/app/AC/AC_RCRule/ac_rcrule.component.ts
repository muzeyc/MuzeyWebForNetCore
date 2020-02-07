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
    templateUrl: './ac_rcrule.component.html',
})
export class ACRCRuleComponent extends AppComponentBase implements OnInit {
    @ViewChild('mtc')
    private mtc: MuzeyTableControlsComponent;
    datas: any[] = [];
    reqModel: any = {};
    totalCount: number;
    cols: ColModel[] = [
        { label: '规则注释', name: 'RuleDesign' },
        { label: '区域', name: 'Area' },
        { label: '车道', name: 'Road' },
        { label: '进出类型', name: 'InOutType' },
        { label: '可破坏性', name: 'IsDestroy' },
        { label: '规则顺序号', name: 'Seq' },
        //{ label: '规则脚本', name: 'RuleScript' },
        { label: '是否启动', name: 'IsEnable' }
    ];
    wsOps: MuzeySelectMuzeyModel[] = [
        { text: 'PBS', val: 'PBS' },
        { text: 'WBS', val: 'WBS' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        this.reqModel.workShop = "PBS";
    }

    ngOnInit(): void {
        this.mtc.pageChangeSub(1);
    }

    getDataPage(offset: number): void {

        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRCRule.GetDatas";
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
            editDialog = this._dialog.open(ACRCRuleEditComponent, { data: { workShop: this.reqModel.workShop }});
        } else {
            editDialog = this._dialog.open(ACRCRuleEditComponent, {
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
                    serverReq.action = "ACRCRule.Delete";
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
    templateUrl: './ac_rcrule_edit.component.html'
})
export class ACRCRuleEditComponent extends AppComponentBase implements OnInit {

    saving = false;
    reqModel: any = {};
    reg: RegExp = new RegExp("^[0-9]*$");
    arOps: MuzeySelectMuzeyModel[] = [
        { text: 'PBS', val: 'PBS' },
        { text: 'WBS', val: 'WBS' }
    ];

    ruleTypeOps = [];
    InRuleOps: MuzeySelectMuzeyModel[] = [
        { text: '进道冻结', val: 'ID' }
    ]

    OutRuleOps: MuzeySelectMuzeyModel[] = [
        { text: '快速车道出道优先', val: 'OSUP' },
        { text: '出道冻结', val: 'OD' },
        { text: '先进先出', val: 'OO' }
    ]

    roOps = [];
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
        { text: '4道', val: '04' },
        { text: '5道', val: '05' }
    ];
    ioOps: MuzeySelectMuzeyModel[] = [
        { text: '进道规则', val: '1' },
        { text: '出道规则', val: '2' }
    ];
    idOps: MuzeySelectMuzeyModel[] = [
        { text: '不可破坏', val: '0' },
        { text: '可破坏', val: '1' }
    ];
    ieOps: MuzeySelectMuzeyModel[] = [
        { text: '不启用', val: '0' },
        { text: '启用', val: '1' },
        { text: '内核启用', val: '2' }
    ];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialogRef: MatDialogRef<ACRCRuleEditComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _data: any
    ) {
        super(injector);
        this.reqModel.saveData = {};
        this.roOps['PBS'] = this.pbsRoOps;
        this.roOps['WBS'] = this.wbsRoOps;
        this.ruleTypeOps['Type1'] = this.InRuleOps;
        this.ruleTypeOps['Type2'] = this.OutRuleOps;
    }

    ngOnInit(): void {
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRCRule.GetData";
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

        if (!this.reg.test(this.reqModel.saveData.Seq)) {
            this.notify.error('规则顺序号错误', '请输出纯数字');
            return;
        }

        if (isNullOrUndefined(this.reqModel.saveData.RuleScript) || this.reqModel.saveData.RuleScript=='') {
            this.notify.error('规则脚本错误', '请输入正确规则脚本');
            return;
        }

        if (isNullOrUndefined(this.reqModel.saveData.InOutType) || this.reqModel.saveData.InOutType == '') {
            this.notify.error('进出道类型错误', '请输入正确进出道类型');
            return;
        }

        this.saving = true;
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "ACRCRule.Save";
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

    ruleTypeChange(val: any): void {

        this.reqModel.saveData.RuleScript = val + '#';
    }
}
