import { Component, Injector, ViewEncapsulation, Inject } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MenuItem } from '@shared/layout/menu-item';
import { MuzeyServiceProxy, MuzeyMenuModel, API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { forEach } from '@angular/router/src/utils/collection';
import { isNullOrUndefined } from 'util';

@Component({
    templateUrl: './sidebar-nav.component.html',
    selector: 'sidebar-nav',
    encapsulation: ViewEncapsulation.None
})
export class SideBarNavComponent extends AppComponentBase {

    
    menuItems: MenuItem[] = [];

    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector);
        abp.ajax({
            url: baseUrl + "/api/services/app/Muzey/GetMenuData?user=" + this.appSession.userId,
            method: 'GET',
            async: false,
            headers: {
                Authorization: 'Bearer ' + abp.auth.getToken(),
                '.AspNetCore.Culture': abp.utils.getCookieValue('Abp.Localization.CultureName'),
                'Abp.TenantId': abp.multiTenancy.getTenantIdCookie()
            }
        }).done(result => {
            for (let i = 0; i < result.childItems.length; i++) {
                let vs = result.childItems[i];
                if (vs.route == '' || vs.route == null) {

                    let mis: MenuItem[] = [];
                    for (let j = 0; j < vs.childItems.length; j++) {

                        let v = vs.childItems[j];
                        mis.push(new MenuItem(this.l(v.name), '', v.icon, v.route));
                    }

                    this.menuItems.push(new MenuItem(this.l(vs.name), '', vs.icon, '', mis));
                } else {

                    this.menuItems.push(new MenuItem(this.l(vs.name), '', vs.icon, vs.route));
                }
            }
        });

        //this._service.getMenuData('')
        //    .subscribe((result: MuzeyMenuModel) => {
        //        for (let i = 0; i < result.childItems.length; i++) {
        //            let vs = result.childItems[i];
        //            if (vs.route == '' || vs.route == null) {

        //                let mis: MenuItem[] = [];
        //                for (let j = 0; j < vs.childItems.length; j++) {

        //                    let v = vs.childItems[j];
        //                    mis.push(new MenuItem(this.l(v.name), '', v.icon, v.route));
        //                }

        //                this.menuItems.push(new MenuItem(this.l(vs.name), '', vs.icon, '', mis));
        //            } else {

        //                this.menuItems.push(new MenuItem(this.l(vs.name), '', vs.icon, vs.route));
        //            }
        //        }
        //    });
    }

    showMenuItem(menuItem): boolean {
        if (menuItem.permissionName) {
            return this.permission.isGranted(menuItem.permissionName);
        }

        return true;
    }
}
