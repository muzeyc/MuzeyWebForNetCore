import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'muzey-time-controls',
    templateUrl: './muzey-time-controls.component.html'
})
export class MuzeyTimeControlsComponent {

    @Input() placeholder: string = '时间';
    @Input() bindVal: string;
    @Output() bindValChange = new EventEmitter();
    @Output() onChange: EventEmitter<string> = new EventEmitter<string>();

    public valChange() {
        this.bindValChange.emit(this.bindVal);
    }
}