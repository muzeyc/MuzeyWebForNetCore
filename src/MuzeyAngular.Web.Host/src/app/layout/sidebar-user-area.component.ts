import { Component, OnInit, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { MuzeyServiceProxy, MuzeyReqModelOfObject, MuzeyResModelOfObject } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './sidebar-user-area.component.html',
    selector: 'sidebar-user-area',
    encapsulation: ViewEncapsulation.None
})
export class SideBarUserAreaComponent extends AppComponentBase implements OnInit {

    shownLoginName = '';

    constructor(
        injector: Injector,
        private _authService: AppAuthService,
        private _service: MuzeyServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.shownLoginName = this.appSession.getShownLoginName();
        let serverReq = new MuzeyReqModelOfObject();
        serverReq.datas = [];
        serverReq.action = "BaseData.GetLogin";
        serverReq.datas.push({ user: this.appSession.userId });
        this._service.req(serverReq)
            .subscribe((result: MuzeyResModelOfObject) => {

                if (result.datas.length > 0) {

                    this.shownLoginName = result.datas[0].PersonName;
                    for (let i = 1; i < result.datas.length;i++)
                        this.shownLoginName += ',' + result.datas[i].PersonName;
                } 
            });
    }

    logout(): void {
        this._authService.logout();
    }
}
