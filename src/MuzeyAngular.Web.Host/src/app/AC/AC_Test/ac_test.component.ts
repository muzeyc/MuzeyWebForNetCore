import { Component, Injector, OnInit, ViewChild, Optional, Inject, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, MatCheckboxChange } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MuzeyServiceProxy, MuzeyReqModelOfObject, MuzeyResModelOfObject } from '@shared/service-proxies/service-proxies';
import { Moment } from 'moment';
import { MuzeyTableControlsComponent, ColModel } from '@shared/muzey-component/muzey-table-controls/muzey-table-controls.component'
import { MuzeyDateModel } from '@shared/muzey-component/muzey-date-controls/muzey-date-controls.component'
import { MuzeySelectMuzeyModel } from '@shared/muzey-component/muzey-select-controls/muzey-select-controls.component'
import { AppComponentBase } from 'shared/app-component-base';
import { isNullOrUndefined } from 'util';
import { Chart } from 'chart.js'

@Component({
    templateUrl: './ac_test.component.html',
})
export class ACTestComponent extends AppComponentBase implements OnInit, AfterViewInit {
    constructor(
        injector: Injector,
        private _service: MuzeyServiceProxy,
        private _dialog: MatDialog,
    ) {
        super(injector);
        (window as any).chartColors = {
            red: 'rgb(255, 99, 132)',
            orange: 'rgb(255, 159, 64)',
            yellow: 'rgb(255, 205, 86)',
            green: 'rgb(75, 192, 192)',
            blue: 'rgb(54, 162, 235)',
            purple: 'rgb(153, 102, 255)',
            grey: 'rgb(201, 203, 207)'
        };
    }

    ngOnInit(): void {
        //let as = abp.signalr.hubs.common;
        //as.on("MuzeySignalr", function (user, message) {
        //    console.log(user + '--------->' + message);
        //    abp.notify.success(user + '--------->' + message);
        //});
        //as.invoke("SendMessage", '初始化', '连接PLC成功').catch(function (err) {
        //    return console.error(err.toString());
        //});
    }

    ngAfterViewInit(): void {

        $(function () {
            // Widgets count
            $('.count-to').countTo();

            // Sales count to
            $('.sales-count-to').countTo({
                formatter: function (value, options) {
                    return '$' + value.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, ' ').replace('.', ',');
                }
            });

            initRealTimeChart();
            initDonutChart();
            initSparkline();

            let config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: [
                            randomScalingFactor(),
                            randomScalingFactor(),
                            randomScalingFactor(),
                            randomScalingFactor(),
                            randomScalingFactor(),
                        ],
                        backgroundColor: [
                            (window as any).chartColors.red,
                            (window as any).chartColors.orange,
                            (window as any).chartColors.yellow,
                            (window as any).chartColors.green,
                            (window as any).chartColors.blue,
                        ],
                        label: 'Dataset 1'
                    }],
                    labels: [
                        'Red',
                        'Orange',
                        'Yellow',
                        'Green',
                        'Blue'
                    ]
                },
                options: {
                    responsive: true
                }
            };

            var ctx = (document.getElementById('chart-area') as any).getContext('2d');
            (window as any).myPie = new Chart(ctx, config);
        });

        function randomScalingFactor() {
            return Math.round(Math.random() * 100);
        };

        let realtime = 'on';
        function initRealTimeChart() {
            // Real time ==========================================================================================
            const plot = ($ as any).plot('#real_time_chart', [getRandomData()], {
                series: {
                    shadowSize: 0,
                    color: 'rgb(0, 188, 212)'
                },
                grid: {
                    borderColor: '#f3f3f3',
                    borderWidth: 1,
                    tickColor: '#f3f3f3'
                },
                lines: {
                    fill: true
                },
                yaxis: {
                    min: 0,
                    max: 100
                },
                xaxis: {
                    min: 0,
                    max: 100
                }
            });

            function updateRealTime() {
                plot.setData([getRandomData()]);
                plot.draw();

                let timeout;
                if (realtime === 'on') {
                    timeout = setTimeout(updateRealTime, 320);
                } else {
                    clearTimeout(timeout);
                }
            }

            updateRealTime();

            $('#realtime').on('change', function () {
                realtime = (this as any).checked ? 'on' : 'off';
                updateRealTime();
            });
            // ====================================================================================================
        }

        function initSparkline() {
            $('.sparkline').each(function () {
                const $this = $(this);
                $this.sparkline('html', $this.data());
            });
        }

        function initDonutChart() {
            ((window as any).Morris).Donut({
                element: 'donut_chart',
                data: [{
                    label: 'Chrome',
                    value: 37
                }, {
                    label: 'Firefox',
                    value: 30
                }, {
                    label: 'Safari',
                    value: 18
                }, {
                    label: 'Opera',
                    value: 12
                },
                {
                    label: 'Other',
                    value: 3
                }],
                colors: ['rgb(233, 30, 99)', 'rgb(0, 188, 212)', 'rgb(255, 152, 0)', 'rgb(0, 150, 136)', 'rgb(96, 125, 139)'],
                formatter: function (y) {
                    return y + '%';
                }
            });
        }

        let data = [];
        const totalPoints = 110;

        function getRandomData() {
            if (data.length > 0) { data = data.slice(1); }

            while (data.length < totalPoints) {
                const prev = data.length > 0 ? data[data.length - 1] : 50;
                let y = prev + Math.random() * 10 - 5;
                if (y < 0) { y = 0; } else if (y > 100) { y = 100; }

                data.push(y);
            }

            const res = [];
            for (let i = 0; i < data.length; ++i) {
                res.push([i, data[i]]);
            }

            return res;
        }
    }
}
