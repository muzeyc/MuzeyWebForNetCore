import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation, OnInit } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { DatePipe } from '@angular/common';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'muzey-date-controls',
    templateUrl: './muzey-date-controls.component.html',
    providers: [{ provide: DatePipe }]
})
export class MuzeyDateControlsComponent implements OnInit{

    //开始时间
    @Input() sTime: string;
    //结束时间
    @Input() eTime: string;

    //SE_Date:开始结束时间 Date:普通控件
    @Input() StyleType:string = 'SE_Date' 
    @Output() sTimeChange = new EventEmitter();
    @Output() eTimeChange = new EventEmitter();
    @Output() onChange: EventEmitter<MuzeyDateModel> = new EventEmitter<MuzeyDateModel>();

    @Input() captionS: string = '开始时间';
    @Input() captionE: string = '结束时间';

    datePipe: DatePipe;
    chanStime: string = '1900-01-01 00:00:00';
    chanEtime: string = '9999-12-31 23:59:59';

    constructor() {
        this.datePipe = new DatePipe('zh-Hans');
    }

    ngOnInit(): void {
        if (!isNullOrUndefined(this.sTime)) {

            this.chanStime = this.sTime + ' 00:00:00';
        }

        if (!isNullOrUndefined(this.eTime)) {

            this.chanEtime = this.eTime + ' 23:59:59';
        }
    }

    public sValChange() {

        this.sTimeChange.emit(this.sTime);
    }

    public eValChange() {

        this.eTimeChange.emit(this.eTime);
    }

    dateOnChange(type: string, event: MatDatepickerInputEvent<Date>) {

        let time: string;
        if (!isNullOrUndefined(event) && !isNullOrUndefined(event.value)) {

            time = event.value.toString();
            time = this.datePipe.transform(time, 'yyyy-MM-dd');
        } else {
            if (type == 's') {
                time = "1900-01-01";
            } else {
                time = "9999-12-31";
            }
        }

        if (type == 's') {
            time += ' 00:00:00';
            this.chanStime = time;
        } else {
            time += ' 23:59:59';
            this.chanEtime = time;
        }

        let dm: MuzeyDateModel = { sTime: this.chanStime, eTime: this.chanEtime }
        this.onChange.emit(dm);
    }
}

export class MuzeyDateModel {

    sTime: string;
    eTime: string;
}