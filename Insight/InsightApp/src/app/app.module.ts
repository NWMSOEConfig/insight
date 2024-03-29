import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSelectModule } from '@angular/material/select';
import { SettingEditorComponent } from './setting-editor/setting-editor.component';
import { ParameterComponent } from './parameter/parameter.component';
import { CategoryListComponent } from './category-list/category-list.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ConfigurationPageComponent } from './configuration-page/configuration-page.component';
import { PublishPageComponent } from './publish-page/publish-page.component';
import { PublishPageFormComponent} from './publish-page/publish-form/publish-page.form';
import { HistoryPageComponent } from './history-page/history-page.component';
import { HistoryCardComponent } from './history-card/history-card.component';
import { TenantSelectorComponent } from './tenant-selector/tenant-selector.component';
import { CommitPageComponent } from './commit-page/commit-page.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTreeModule } from '@angular/material/tree';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    SettingEditorComponent,
    ParameterComponent,
    CategoryListComponent,
    ConfigurationPageComponent,
    PublishPageComponent,
    PublishPageFormComponent,
    HistoryPageComponent,
    HistoryCardComponent,
    TenantSelectorComponent,
    CommitPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MatButtonModule,
    MatSliderModule,
    MatBadgeModule,
    MatSlideToggleModule,
    MatFormFieldModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatTableModule,
    MatListModule,
    MatMenuModule,
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    MatSelectModule,
    MatBottomSheetModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatStepperModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatNativeDateModule,
    MatRippleModule,
    MatPaginatorModule,
    MatRadioModule,
    MatSnackBarModule,
    MatSortModule,
    MatTreeModule,
    MatTabsModule,
    MatTooltipModule,
    HttpClientModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
