import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { DxDataGridModule } from 'devextreme-angular/ui/data-grid';
import { DxChartModule } from 'devextreme-angular/ui/chart';
import { DxPieChartModule } from 'devextreme-angular/ui/pie-chart';
import { DxSelectBoxModule } from 'devextreme-angular/ui/select-box';
import { DxTemplateModule } from 'devextreme-angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { LoginComponent } from './features/auth/login/login.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { CustomersComponent } from './features/customers/customers.component';
import { ItemsComponent } from './features/items/items.component';
import { SalesInvoiceComponent } from './features/sales-invoice/sales-invoice.component';
import { SalesReportComponent } from './features/reports/sales-report.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    DashboardComponent,
    CustomersComponent,
    ItemsComponent,
    SalesInvoiceComponent,
    SalesReportComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    DxDataGridModule,
    DxChartModule,
    DxPieChartModule,
    DxSelectBoxModule,
    DxTemplateModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
