import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'muzey-table-controls',
    templateUrl: './muzey-table-controls.component.html',
    providers: [{ provide: DatePipe }]
})
export class MuzeyTableControlsComponent {

    @Input() id: string;
    //表头配置
    @Input() cols: ColModel[];
    //数据
    @Input() datas: any[];
    @Input('datas') set setDatas(value: any[])
    {
        if (value.length > 0) {

            this.datas = value;
            this.itemObj = value[0];
        } else {

            this.datas = [];
        }
    }
    
    
    //一页显示数
    @Input() pageSize = 10;
    //当前数据总行数
    @Input() totalCount: number;

    @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();
    @Output() rowClick = new EventEmitter();

    //当前页数
    curPageNum = 1;
    //是否显示读取效果
    isTableLoading = false;
    //当前选中行
    selectedIndex: number = 0;
    //当前数据
    itemObj: any;

    datePipe: DatePipe;

    constructor() {

        this.id = 'server';
        this.datePipe = new DatePipe('zh-Hans');
    }

    getDataValue(data, col): string{

        let val = eval("data." + col.name);
        if (col.name.indexOf('Time') > 0) {

            try {
                val = this.datePipe.transform(val, 'yyyy-MM-dd HH:mm:ss');
            } catch (err) {

                //console.warn(err);
            }
        }
        
        return val;
    }

    pageChangeSub(curPageNum: number) {

        this.selectedIndex = 0;
        this.curPageNum = curPageNum;
        this.isTableLoading = true;
        this.pageChange.emit((curPageNum - 1) * this.pageSize);
    }

    onRowClick = function (index, item) {
        if (this.selectedIndex == index) {
            return;
        }
        this.itemObj = item;
        this.selectedIndex = index;
        this.rowClick.emit({ index: index, item: item });
    }
}

export class ColModel{

    label: string;
    name: string;
}