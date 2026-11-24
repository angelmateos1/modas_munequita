import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { CatalogoComponent } from './components/catalogo/catalogo';

export const routes: Routes = [
  { path: '', component: CatalogoComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '' }
];