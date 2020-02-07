import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { isNullOrUndefined } from 'util';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AppConsts } from '@shared/AppConsts';

import { Router } from '@angular/router';

@Component({
    selector: 'muzey-pageedit-controls',
    templateUrl: './muzey-pageedit-controls.component.html',
})
export class MuzeyPageEditControlsComponent {

    @Input() title: string;
    @Input() width: string;
    @Input() height: string;
    //页面类型 单页面:single 多页面:multipage
    @Input() type: string;
    @Output() onCloseClick = new EventEmitter();
    @Output() onSaveClick = new EventEmitter();

    @Input() SaveShow: boolean;

    constructor() {

        this.SaveShow = true;
        this.type = "single";
    }
    
}