import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../core/services/report.service';
import { DashboardSummary } from '../../shared/models/models';

@Component({
  selector: 'sms-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  summary: DashboardSummary | null = null;
  loading = true;

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.reportService.getDashboardSummary().subscribe(data => {
      this.summary = data;
      this.loading = false;
    });
  }
}
