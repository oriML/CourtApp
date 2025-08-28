import { inject } from '@angular/core';
import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingService } from './loading.service';
import { SnackbarService } from '../snackbar/snackbar.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  const snackbarService = inject(SnackbarService);

  loadingService.show();

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      snackbarService.error(error.message);
      return throwError(() => error);
    }),
    finalize(() => {
      loadingService.hide();
    })
  );
};
