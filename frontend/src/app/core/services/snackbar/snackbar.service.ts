import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { SnackbarComponent } from '../../../shared/ui/snackbar/snackbar.component';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  constructor(private snackBar: MatSnackBar, private breakpointObserver: BreakpointObserver) { }

  private openSnackbar(message: string, icon: string, type: string): void {
    const config: MatSnackBarConfig = {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: [`snackbar-${type}`],
      data: { message, icon, type }
    };

    this.breakpointObserver.observe([Breakpoints.HandsetPortrait, Breakpoints.HandsetLandscape])
      .subscribe(result => {
        if (result.matches) {
          config.horizontalPosition = 'center';
          config.verticalPosition = 'bottom';
        } else {
          config.horizontalPosition = 'right';
          config.verticalPosition = 'bottom';
        }
        this.snackBar.openFromComponent(SnackbarComponent, config);
      });
  }

  success(message: string): void {
    this.openSnackbar(message, 'check_circle', 'success');
  }

  error(message: string): void {
    this.openSnackbar(message, 'error', 'error');
  }

  info(message: string): void {
    this.openSnackbar(message, 'info', 'info');
  }

  warning(message: string): void {
    this.openSnackbar(message, 'warning', 'warning');
  }

  login(message: string): void {
    this.openSnackbar(message, 'login', 'login');
  }

  add(message: string): void {
    this.openSnackbar(message, 'add_circle', 'add');
  }

  delete(message: string): void {
    this.openSnackbar(message, 'delete', 'delete');
  }
}