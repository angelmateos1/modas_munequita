import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { RegistroComponent } from './components/registro/registro';
import { CatalogoComponent } from './components/catalogo/catalogo';
import { PanelDitaComponent } from './components/panel-dita/panel-dita';
import { MiDitaComponent } from './components/mi-dita/mi-dita';
import { MiPerfilComponent } from './components/mi-perfil/mi-perfil';
import { adminGuard } from './guards/admin-guard';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', component: CatalogoComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registro', component: RegistroComponent },
  {
    path: 'mi-dita',
    component: MiDitaComponent,
    canActivate: [authGuard]
  },
  {
    path: 'mi-perfil',
    component: MiPerfilComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/clientes',
    component: PanelDitaComponent,
    canActivate: [adminGuard]
  },
  { path: '**', redirectTo: '' }
];