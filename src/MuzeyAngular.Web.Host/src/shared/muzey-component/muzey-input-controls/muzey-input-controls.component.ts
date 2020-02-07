import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'muzey-input-controls',
    templateUrl: './muzey-input-controls.component.html'
})
export class MuzeyInputControlsComponent {

    @Input() placeholder: string;
    @Input() bindVal: string;
    @Output() bindValChange = new EventEmitter();
    @Output() onChange: EventEmitter<string> = new EventEmitter<string>();

    public valChange() {
        this.bindValChange.emit(this.bindVal);
    }
}