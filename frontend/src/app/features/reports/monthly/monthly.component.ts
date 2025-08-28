import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { ContactRequestsService } from '../../../core/services/contact-requests/contact-requests.service';
import { MonthlyReport } from '../../../core/models/monthly-report.interface';
import { Observable, of } from 'rxjs'; // Import 'of'

@Component({
  selector: 'app-monthly-report',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule
  ],
  templateUrl: './monthly.component.html',
  styleUrl: './monthly.component.scss'
})
export class MonthlyReportComponent implements OnInit {
  monthlyReports$: Observable<MonthlyReport[]> = of([]); // Initialize with an empty array
  displayedColumns: string[] = ['month', 'totalRequests', 'departmentBreakdown'];

  constructor(private contactRequestsService: ContactRequestsService) { }

  ngOnInit(): void {
    this.monthlyReports$ = this.contactRequestsService.getMonthlyReport();
  }

  getMonthName(monthNumber: number): string {
    const date = new Date();
    date.setMonth(monthNumber - 1); // Month is 0-indexed in Date object
    return date.toLocaleString('en-US', { month: 'long' });
  }

  getDepartmentBreakdown(breakdown: { [key: string]: number }): string {
    return Object.entries(breakdown)
      .map(([department, count]) => `${department}: ${count}`)
      .join(', ');
  }
}
