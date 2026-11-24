import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { CatalogoComponent } from './components/catalogo/catalogo';
import { PanelDitaComponent } from './components/panel-dita/panel-dita';

export const routes: Routes = [
  { path: '', component: CatalogoComponent },
  { path: 'login', component: LoginComponent },
  { path: 'admin/clientes', component: PanelDitaComponent },
  { path: '**', redirectTo: '' }
];