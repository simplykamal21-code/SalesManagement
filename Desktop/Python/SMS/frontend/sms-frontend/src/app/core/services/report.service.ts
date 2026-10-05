import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DashboardSummary, SalesReportRow } from '../../shared/models/models';

@Injectable({ providedIn: 'root' })
export class ReportService {
  constructor(private http: HttpClient) {}

  getSalesReport(fromDate?: string, toDate?: string): Observable<SalesReportRow[]> {
    const params: any = {};
    if (fromDate) params.fromDate = fromDate;
    if (toDate) params.toDate = toDate;
    return this.http.get<SalesReportRow[]>(`${environment.apiUrl}/reports/sales`, { params });
  }

  getDashboardSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${environment.apiUrl}/dashboard/summary`);
  }
}
