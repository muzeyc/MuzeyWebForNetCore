import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation, OnInit, Injector, ViewChild, ElementRef } from '@angular/core';
import { isNullOrUndefined } from 'util';
import { MuzeyServiceProxy, MuzeyReqModelOfObject } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
    selector: 'muzey-upload-controls',
    templateUrl: './muzey-upload-controls.component.html'
})
export class MuzeyUploadControlsComponent extends AppComponentBase implements OnInit{

    @ViewChild('fileInput') fileInput: ElementRef;

    @Input() txt: string;
    @Output() aftUpload = new EventEmitter();

    constructor(
        injector: Injector,
        public _service: MuzeyServiceProxy,
    ) {
        super(injector);
        this.txt = "上传";
    }

    ngOnInit(): void { }

    selectedFileOnChanged(event: any) {

        if (event.target.files.length < 1) {

            return;
        }

        let thisObj = this;
        let txtCace = this.txt;
        this.txt = "上传中";
        let fileName = event.target.files[0].name;
        let reader = new FileReader();
        reader.readAsDataURL(event.target.files[0]);
        reader.onload = function (e: any) {

            let base64 = e.target.result.split(',')[1];
            let serverReq = new MuzeyReqModelOfObject();
            serverReq.datas = [];
            serverReq.datas.push({ base64: base64, fileName: fileName });
            thisObj._service.fileUpload(serverReq)
                .subscribe(result => {
                    thisObj.notify.info('文件上传成功');
                    thisObj.txt = txtCace;
                    thisObj.fileInput.nativeElement.value = ''
                    thisObj.aftUpload.emit(result);
                });
        }
    }

    /**
     * 上传文件方法
     */
    uploadFile() {

        this.fileInput.nativeElement.click();
    }
}