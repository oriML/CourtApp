import { Routes } from "@angular/router";
import { authGuard } from "./app/core/guards/auth.guard";
import { LoginComponent } from "./app/features/auth/login/login.component";
import { ContactRequestsListComponent } from "./app/features/contact-requests/list/list.component";
import { MonthlyReportComponent } from "./app/features/reports/monthly/monthly.component";

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'contact-requests',
    component: ContactRequestsListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'reports/monthly',
    component: MonthlyReportComponent,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
];
