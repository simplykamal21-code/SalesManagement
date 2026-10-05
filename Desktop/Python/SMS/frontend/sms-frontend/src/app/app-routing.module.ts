import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { CustomersComponent } from './features/customers/customers.component';
import { ItemsComponent } from './features/items/items.component';
import { SalesInvoiceComponent } from './features/sales-invoice/sales-invoice.component';
import { SalesReportComponent } from './features/reports/sales-report.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'customers', component: CustomersComponent, canActivate: [AuthGuard] },
  { path: 'items', component: ItemsComponent, canActivate: [AuthGuard] },
  { path: 'sales', component: SalesInvoiceComponent, canActivate: [AuthGuard] },
  { path: 'reports', component: SalesReportComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: '/dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
