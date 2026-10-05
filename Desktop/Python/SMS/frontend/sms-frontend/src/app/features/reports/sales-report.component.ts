import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../core/services/report.service';
import { SalesReportRow } from '../../shared/models/models';

@Component({
  selector: 'sms-sales-report',
  templateUrl: './sales-report.component.html',
  styleUrls: ['./sales-report.component.scss']
})
export class SalesReportComponent implements OnInit {
  rows: SalesReportRow[] = [];
  loading = false;

  // Grouping panel config for the DevExtreme grid - lets the user drag columns
  // here to group by Customer, Category, Brand, etc. per the spec.
  groupPanel = { visible: true };
  filterRow = { visible: true };
  headerFilter = { visible: true };

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.loading = true;
    this.reportService.getSalesReport().subscribe(rows => {
      this.rows = rows;
      this.loading = false;
    });
  }
}
