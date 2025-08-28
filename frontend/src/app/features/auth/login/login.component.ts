import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SnackbarService } from '../../../core/services/snackbar/snackbar.service'; // Import SnackbarService
import { AuthService } from '../../../core/services/auth/auth.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackbarService: SnackbarService // Inject SnackbarService
  ) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loading = true;
      const { username, password } = this.loginForm.value;
      this.authService.login({ username, password }).subscribe({
        next: (token) => {
          console.log('Login successful, token:', token);
          this.snackbarService.login('Login successful!'); // Show success snackbar
          this.router.navigate(['/contact-requests']);
          this.loading = false;
        },
        error: (err) => {
          console.error('Login failed:', err);
          this.snackbarService.error('Login failed. Please check your credentials.'); // Show error snackbar
          this.loading = false;
        }
      });
    }
  }
}
