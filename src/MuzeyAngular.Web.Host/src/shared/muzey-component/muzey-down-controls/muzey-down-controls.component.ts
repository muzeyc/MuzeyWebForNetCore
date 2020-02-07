import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation, OnInit, Injector, ViewChild, ElementRef } from '@angular/core';
import { isNullOrUndefined } from 'util';
import { MuzeyServiceProxy, MuzeyReqModelOfObject, MuzeyResModelOfObject } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
    selector: 'muzey-down-controls',
    templateUrl: './muzey-down-controls.component.html'
})
export class MuzeyDownControlsComponent extends AppComponentBase implements OnInit {

    @ViewChild('downCore') downCore: ElementRef;

    @Input() txt: string;
    @Input() url: string;
    @Input() downName: string;
    @Input() reqModel: any;
    @Input() cols: any;
    downing: boolean = false;

    constructor(
        injector: Injector,
        public _service: MuzeyServiceProxy,
    ) {
        super(injector);
        this.txt = "导出";
    }

    ngOnInit(): void { }

    /**
     * 上传文件方法
     */
    downFile() {

        if (isNullOrUndefined(this.url) || isNullOrUndefined(this.downName)) {

            this.notify.error('下载错误', '请联系管理员');
            return;
        }

        if (this.downing) {

            this.notify.error('提示', '请等待当前任务下载完成');
            return;
        }

        let downingStr = this.txt;
        this.downing = true;
        this.txt = '导出中';
        
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = this.url;
        serverReq.fileName = this.downName;
        serverReq.cols = this.cols;
        serverReq.datas.push(this.reqModel);
        this._service.req(serverReq)
            .subscribe((result: MuzeyResModelOfObject) => {
                if (result.resStatus == 'err') {

                    this.notify.error('错误', result.resMsg);
                    this.txt = downingStr;
                    this.downing = false;
                    return;
                }

                let intArray = new Uint8Array(result.bs);
                let newIntArr = intArray.buffer.slice(0, result.bs.length);
                let blob = new Blob([newIntArr]);
                this.downCore.nativeElement.download = this.downName;
                this.downCore.nativeElement.href = URL.createObjectURL(blob);
                this.downCore.nativeElement.click();
                this.txt = downingStr;
                this.downing = false;
            });
    }
}