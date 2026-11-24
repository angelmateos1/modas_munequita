import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { RegistroComponent } from './components/registro/registro';
import { CatalogoComponent } from './components/catalogo/catalogo';
import { PanelDitaComponent } from './components/panel-dita/panel-dita';
import { adminGuard } from './guards/admin-guard';

export const routes: Routes = [
  { path: '', component: CatalogoComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registro', component: RegistroComponent },
  {
    path: 'admin/clientes',
    component: PanelDitaComponent,
    canActivate: [adminGuard]
  },
  { path: '**', redirectTo: '' }
];