import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { MatDialog } from '@angular/material/dialog';
import { ContactRequestsService } from '../../../core/services/contact-requests/contact-requests.service';
import { ContactRequest } from '../../../core/models/contact-request.interface';
import { Router } from '@angular/router';
import { Observable, switchMap } from 'rxjs';
import { ContactRequestDialogComponent } from '../dialog/dialog.component';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { SnackbarService } from '../../../core/services/snackbar/snackbar.service';
import { ContactRequestCardComponent } from '../../../shared/components/contact-request-card/contact-request-card.component'; // Import ContactRequestCardComponent
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-contact-requests-list',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule,
    MatListModule,
    ContactRequestCardComponent, // Add ContactRequestCardComponent to imports
    MatProgressSpinnerModule
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ContactRequestsListComponent implements OnInit {
  contactRequests$: Observable<ContactRequest[]> | undefined;
  cols: number = 3; // Default to 3 columns
  loading = false;

  constructor(
    private contactRequestsService: ContactRequestsService,
    private router: Router,
    public dialog: MatDialog,
    private snackbarService: SnackbarService,
    private breakpointObserver: BreakpointObserver // Inject BreakpointObserver
  ) { }

  ngOnInit(): void {
    this.fetchContactRequests();
    this.breakpointObserver.observe([
      Breakpoints.HandsetPortrait,
      Breakpoints.HandsetLandscape,
      Breakpoints.TabletPortrait,
      Breakpoints.TabletLandscape
    ]).subscribe(result => {
      if (result.matches) {
        if (result.breakpoints[Breakpoints.HandsetPortrait] || result.breakpoints[Breakpoints.HandsetLandscape]) {
          this.cols = 1; // 1 column for mobile
        } else if (result.breakpoints[Breakpoints.TabletPortrait] || result.breakpoints[Breakpoints.TabletLandscape]) {
          this.cols = 2; // 2 columns for tablet
        }
      } else {
        this.cols = 3; // 3 columns for desktop
      }
    });
  }

  fetchContactRequests(): void {
    this.loading = true;
    this.contactRequests$ = this.contactRequestsService.getAllContactRequests();
    this.contactRequests$.subscribe(() => this.loading = false);
  }

  editRequest(id: string): void {
    this.contactRequestsService.getContactRequestById(id).subscribe(request => {
      const dialogRef = this.dialog.open(ContactRequestDialogComponent, {
        width: '600px',
        data: request
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.fetchContactRequests();
          this.snackbarService.info('Contact request updated successfully!');
        }
      });
    });
  }

  deleteRequest(id: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Confirm Deletion',
        message: 'Are you sure? This action cannot be undone.'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.contactRequestsService.deleteContactRequest(id).subscribe({
          next: () => {
            this.snackbarService.delete('Contact request deleted successfully!');
            this.fetchContactRequests();
          },
          error: (err) => {
            console.error('Error deleting request:', err);
            this.snackbarService.error(`Error deleting request: ${err.message || 'Unknown error'}`);
          }
        });
      }
    });
  }

  addRequest(): void {
    const dialogRef = this.dialog.open(ContactRequestDialogComponent, {
      width: '600px',
      data: null
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchContactRequests();
        this.snackbarService.add('Contact request added successfully!');
      }
    });
  }
}
