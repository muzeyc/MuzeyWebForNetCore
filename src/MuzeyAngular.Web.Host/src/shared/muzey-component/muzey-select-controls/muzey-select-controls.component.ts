import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'muzey-select-controls',
    templateUrl: './muzey-select-controls.component.html'
})
export class MuzeySelectControlsComponent {

    @Input() placeholder: string;
    //下拉数据
    @Input() options: MuzeySelectMuzeyModel[];
    @Input() disab: boolean;
    @Input() bindVal: string;
    @Output() bindValChange = new EventEmitter();

    public valChange() {
        this.bindValChange.emit(this.bindVal);
    }
}

export class MuzeySelectMuzeyModel {

    //值
    val: string;
    //文本
    text: string;
}