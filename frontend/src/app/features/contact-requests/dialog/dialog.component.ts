import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { ContactRequest } from '../../../core/models/contact-request.interface';
import { ContactRequestsService } from '../../../core/services/contact-requests/contact-requests.service';
import { SnackbarService } from '../../../core/services/snackbar/snackbar.service'; // Import SnackbarService
import { Observable } from 'rxjs';

@Component({
  selector: 'app-contact-request-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatOptionModule
  ],
  templateUrl: './dialog.component.html',
  styleUrl: './dialog.component.scss'
})
export class ContactRequestDialogComponent implements OnInit {
  contactRequestForm!: FormGroup;
  isEditMode: boolean = false;
  departments: string[] = ['Legal', 'Finance', 'HR', 'IT', 'Marketing'];
  statuses: ('pending' | 'resolved' | 'rejected')[] = ['pending', 'resolved', 'rejected'];

  constructor(
    public dialogRef: MatDialogRef<ContactRequestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ContactRequest | null,
    private contactRequestsService: ContactRequestsService,
    private snackbarService: SnackbarService // Inject SnackbarService
  ) { }

  ngOnInit(): void {
    this.contactRequestForm = new FormGroup({
      name: new FormControl('', Validators.required),
      phone: new FormControl('', Validators.pattern(/^[0-9]{10}$/)), // Basic 10-digit phone validation
      email: new FormControl('', [Validators.required, Validators.email]),
      departments: new FormControl([]),
      description: new FormControl('', Validators.required),
      status: new FormControl('pending', Validators.required)
    });

    if (this.data) {
      this.isEditMode = true;
      this.contactRequestForm.patchValue({
        name: this.data.name,
        phone: this.data.phone,
        email: this.data.email,
        departments: this.data.departments || [],
        description: this.data.description,
        status: this.data.status
      });
    }
  }

  onSubmit(): void {
    if (this.contactRequestForm.valid) {
      const formValue: ContactRequest = this.contactRequestForm.value;
      let operation: Observable<ContactRequest | void>;

      if (this.isEditMode && this.data) {
        operation = this.contactRequestsService.updateContactRequest(this.data.id, formValue);
      } else {
        // Generate a dummy ID for new requests for now
        const newRequest: ContactRequest = { ...formValue, id: Date.now().toString(), createdAt: new Date().toISOString() };
        operation = this.contactRequestsService.createContactRequest(newRequest);
      }

      operation.subscribe({
        next: () => {
          this.snackbarService.success(`Contact request ${this.isEditMode ? 'updated' : 'created'} successfully!`);
          this.dialogRef.close(true); // Close dialog and indicate success
        },
        error: (err) => {
          console.error('Error saving contact request:', err);
          this.snackbarService.error(`Error saving contact request: ${err.message || 'Unknown error'}`);
        }
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
