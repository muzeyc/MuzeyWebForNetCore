import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { isNullOrUndefined } from 'util';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AppConsts } from '@shared/AppConsts';

import { Router, NavigationEnd } from '@angular/router';

@Component({
    selector: 'muzey-pagetable-controls',
    templateUrl: './muzey-pagetable-controls.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ]
})
export class MuzeyPageTableControlsComponent {

    @Input() title: string;
    @Input() sameRef: boolean;

    private scanBool: boolean = false;
    private sameNum: number;
    private url: string;

    constructor(
        public router: Router,
    ) {
        this.sameRef = false;
        this.url = this.router.url.split('/')[2];
    }

    ngAfterViewInit() {

        let sObj = this;
        if (this.sameRef && this.sameRef != undefined && !(this.router.url.indexOf("home") > 0) && !(this.router.url.indexOf("-") > 0)) {

            let res = this.router.events.subscribe((event: NavigationEnd) => {
                if (event instanceof NavigationEnd && !(this.router.url.indexOf("-") > 0) && !(this.router.url.indexOf("home") > 0)) {
                    if (this.router.url.indexOf(sObj.url) >= 0) {
                        this.sameRefresh();
                    } else {
                        res.unsubscribe();
                    }
                    
                }
            });
        }
    }

    refresh(): void {

        let rUrl = this.router.url;
        this.router.navigateByUrl("/app/home").then(() => {

            this.router.navigate([rUrl]);
        });
    }

    overscan(): void {

        if (this.scanBool) {
            $('#MuzeyTopBar').show();
            $('#MuzeyOutsideSection').show();
            $('#MuzeyContentSection').addClass('content');
            this.exitFullScreen();
        } else {
            $('#MuzeyTopBar').hide();
            $('#MuzeyOutsideSection').hide();
            $('#MuzeyContentSection').removeClass('content');
            this.fullScreen();
        }

        this.scanBool = !this.scanBool;
    }

    sameRefresh(): void {

        let rUrl = this.router.url+"-";
        this.router.navigateByUrl("/app/home").then(() => {

            this.router.navigate([rUrl]);
        });
    }

    fullScreen(): void {
        var el = document.documentElement as any,
            rfs = el.requestFullScreen || el.webkitRequestFullScreen || el.mozRequestFullScreen || el.msRequestFullScreen,
            wscript;

        if (typeof rfs != "undefined" && rfs) {
            rfs.call(el);
            return;
        }
    }

    exitFullScreen(): void {
        var el = document as any,
            cfs = el.cancelFullScreen || el.webkitCancelFullScreen || el.mozCancelFullScreen || el.exitFullScreen,
            wscript;

        if (typeof cfs != "undefined" && cfs) {
            cfs.call(el);
            return;
        }
    }
}