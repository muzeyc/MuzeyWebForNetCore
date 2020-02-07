import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewEncapsulation, OnInit } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'muzey-button-controls',
    templateUrl: './muzey-button-controls.component.html'
})
export class MuzeyButtonControlsComponent implements OnInit{

    @Input() txt: string;
    @Input() icon: string;
    @Input() color: string;
    @Input() tem: string;

    temConfig: any = [];

    ngOnInit(): void {

            if (!isNullOrUndefined(this.tem)) {

                this.temConfig['search'] = { txt: '查询', icon: 'search', color: '#2196F3' };
                this.temConfig['add'] = { txt: '新增', icon: 'add_circle', color: '#8BC34A' };
                this.temConfig['edit'] = { txt: '编辑', icon: 'edit', color: '#FF9800' };
                this.temConfig['delete'] = { txt: '删除', icon: 'delete_forever', color: '#E20074' }; 
                this.temConfig['cancel'] = { txt: '取消', icon: 'rotate_left', color: '#9E9E9E' };
                this.temConfig['save'] = { txt: '保存', icon: 'near_me', color: '#3F51B5' };
                this.txt = this.temConfig[this.tem].txt;
                this.icon = this.temConfig[this.tem].icon;
                this.color = this.temConfig[this.tem].color;
            }
    }
}