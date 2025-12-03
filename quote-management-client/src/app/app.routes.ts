import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { QuoteFormComponent } from './quotes/quote-form/quote-form.component';
import { QuoteListComponent } from './quotes/quote-list/quote-list.component';
import { authGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'quotes',
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'form', component: QuoteFormComponent },
      { path: 'list', component: QuoteListComponent },
    ],
  },
  { path: '**', redirectTo: '/login' },
];
