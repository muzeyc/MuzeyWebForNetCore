import { CommonModule } from '@angular/common';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { AbpModule } from '@abp/abp.module';
import { RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule } from '@angular/forms';

import { AppSessionService } from './session/app-session.service';
import { AppUrlService } from './nav/app-url.service';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { AbpPaginationControlsComponent } from './pagination/abp-pagination-controls.component';

//Muzey页面级控件
import { MuzeyPageTableControlsComponent } from './muzey-component/muzey-pagetable-controls/muzey-pagetable-controls.component';
import { MuzeyPageEditControlsComponent } from './muzey-component/muzey-pageedit-controls/muzey-pageedit-controls.component';

//Muzey控件
import { MuzeyTableControlsComponent } from './muzey-component/muzey-table-controls/muzey-table-controls.component';
import { MuzeyDateControlsComponent } from './muzey-component/muzey-date-controls/muzey-date-controls.component';
import { MuzeySelectControlsComponent } from './muzey-component/muzey-select-controls/muzey-select-controls.component';
import { MuzeyInputControlsComponent } from './muzey-component/muzey-input-controls/muzey-input-controls.component';
import { MuzeyButtonControlsComponent } from './muzey-component/muzey-button-controls/muzey-button-controls.component';
import { MuzeyUploadControlsComponent } from './muzey-component/muzey-upload-controls/muzey-upload-controls.component';
import { MuzeyDownControlsComponent } from './muzey-component/muzey-down-controls/muzey-down-controls.component';
import { MuzeyTimeControlsComponent } from './muzey-component/muzey-time-controls/muzey-time-controls.component';
import { MuzeyDateTimeControlsComponent } from './muzey-component/muzey-datetime-controls/muzey-datetime-controls.component';

import { LocalizePipe } from '@shared/pipes/localize.pipe';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CdkTableModule } from '@angular/cdk/table';
import { CdkTreeModule } from '@angular/cdk/tree';
import {
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,
} from '@angular/material';
import { BlockDirective } from './directives/block.directive';
import { BusyDirective } from './directives/busy.directive';
import { EqualValidator } from './directives/equal-validator.directive';
@NgModule({
    imports: [
        CommonModule,
        AbpModule,
        RouterModule,
        NgxPaginationModule,
        FormsModule,
        MatAutocompleteModule,
        MatBadgeModule,
        MatBottomSheetModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatChipsModule,
        MatDatepickerModule,
        MatDialogModule,
        MatDividerModule,
        MatExpansionModule,
        MatGridListModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatRippleModule,
        MatSelectModule,
        MatSidenavModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatSortModule,
        MatStepperModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        MatTreeModule,
    ],
    declarations: [
        AbpPaginationControlsComponent,
        MuzeyPageTableControlsComponent,
        MuzeyPageEditControlsComponent,
        MuzeyTableControlsComponent,
        MuzeyDateControlsComponent,
        MuzeySelectControlsComponent,
        MuzeyInputControlsComponent,
        MuzeyButtonControlsComponent,
        MuzeyUploadControlsComponent,
        MuzeyDownControlsComponent,
        MuzeyTimeControlsComponent,
        MuzeyDateTimeControlsComponent,
        LocalizePipe,
        BlockDirective,
        BusyDirective,
        EqualValidator
    ],
    exports: [
        AbpPaginationControlsComponent,
        MuzeyPageTableControlsComponent,
        MuzeyPageEditControlsComponent,
        MuzeyTableControlsComponent,
        MuzeyDateControlsComponent,
        MuzeySelectControlsComponent,
        MuzeyInputControlsComponent,
        MuzeyButtonControlsComponent,
        MuzeyUploadControlsComponent,
        MuzeyDownControlsComponent,
        MuzeyTimeControlsComponent,
        MuzeyDateTimeControlsComponent,
        LocalizePipe,
        BlockDirective,
        BusyDirective,
        EqualValidator,
        CdkTableModule,
        CdkTreeModule,
        DragDropModule,
        MatAutocompleteModule,
        MatBadgeModule,
        MatBottomSheetModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatChipsModule,
        MatStepperModule,
        MatDatepickerModule,
        MatDialogModule,
        MatDividerModule,
        MatExpansionModule,
        MatGridListModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatRippleModule,
        MatSelectModule,
        MatSidenavModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatSortModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        MatTreeModule,
        ScrollingModule,
    ]
})
export class SharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                AppSessionService,
                AppUrlService,
                AppAuthService,
                AppRouteGuard
            ]
        };
    }
}
