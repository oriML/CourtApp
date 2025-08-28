import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { AuthService } from '../../../core/services/auth/auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [
        LoginComponent,
        ReactiveFormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        BrowserAnimationsModule // Required for Material components
      ],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty fields', () => {
    expect(component.loginForm.get('username')?.value).toEqual('');
    expect(component.loginForm.get('password')?.value).toEqual('');
  });

  it('should make the username field required', () => {
    const username = component.loginForm.get('username');
    username?.setValue('');
    expect(username?.valid).toBeFalse();
    expect(username?.hasError('required')).toBeTrue();
  });

  it('should validate username format', () => {
    const username = component.loginForm.get('username');
    username?.setValue('invalid-username');
    expect(username?.valid).toBeFalse();
    expect(username?.hasError('username')).toBeTrue();

    username?.setValue('valid@example.com');
    expect(username?.valid).toBeTrue();
    expect(username?.hasError('username')).toBeFalse();
  });

  it('should make the password field required', () => {
    const password = component.loginForm.get('password');
    password?.setValue('');
    expect(password?.valid).toBeFalse();
    expect(password?.hasError('required')).toBeTrue();
  });

  it('should validate password minimum length', () => {
    const password = component.loginForm.get('password');
    password?.setValue('123');
    expect(password?.valid).toBeFalse();
    expect(password?.hasError('minlength')).toBeTrue();

    password?.setValue('123456');
    expect(password?.valid).toBeTrue();
    expect(password?.hasError('minlength')).toBeFalse();
  });

  it('should call authService.login and navigate on successful login', () => {
    authServiceSpy.login.and.returnValue(of({ token: 'dummy_token' }));

    component.loginForm.setValue({
      username: 'test@example.com',
      password: 'password123'
    });

    component.onSubmit();

    expect(authServiceSpy.login).toHaveBeenCalledWith({ username: 'test@example.com', password: 'password123' });
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/contact-requests']);
  });

  it('should not navigate on failed login', () => {
    authServiceSpy.login.and.returnValue(throwError(() => new Error('Login failed')));

    component.loginForm.setValue({
      username: 'test@example.com',
      password: 'password123'
    });

    component.onSubmit();

    expect(authServiceSpy.login).toHaveBeenCalled();
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  });

  it('should disable submit button when form is invalid', () => {
    component.loginForm.setValue({
      username: 'invalid',
      password: '123'
    });
    fixture.detectChanges();
    const submitButton = fixture.nativeElement.querySelector('button[type="submit"]');
    expect(submitButton.disabled).toBeTrue();
  });

  it('should enable submit button when form is valid', () => {
    component.loginForm.setValue({
      username: 'valid@example.com',
      password: 'password123456'
    });
    fixture.detectChanges();
    const submitButton = fixture.nativeElement.querySelector('button[type="submit"]');
    expect(submitButton.disabled).toBeFalse();
  });
});