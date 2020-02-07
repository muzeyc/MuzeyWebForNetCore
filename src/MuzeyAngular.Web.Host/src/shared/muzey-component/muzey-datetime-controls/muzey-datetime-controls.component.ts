import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation, OnInit } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { DatePipe } from '@angular/common';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'muzey-datetime-controls',
    templateUrl: './muzey-datetime-controls.component.html',
    providers: [{ provide: DatePipe }]
})
export class MuzeyDateTimeControlsComponent implements OnInit {

    @Input() placeholder: string = '时间';
    @Input() bindVal: string;
    @Output() bindValChange = new EventEmitter();
    datePipe: DatePipe;
    date: string;
    time: string = '00:00';

    constructor() {
        this.datePipe = new DatePipe('zh-Hans');
    }

    ngOnInit(): void {

    }

    public dateValChange(val: string) {

        this.bindVal = this.getDateTimeData();
        this.bindValChange.emit(this.bindVal);
    }

    public timeValChange(val: string) {

        this.bindVal = this.getDateTimeData();
        this.bindValChange.emit(this.bindVal);
    }

    getDateTimeData(): string {

        let dateFomat = this.datePipe.transform(this.date, 'yyyy-MM-dd');
        return dateFomat == null ? '' : dateFomat + " " + this.time;
    }
}
