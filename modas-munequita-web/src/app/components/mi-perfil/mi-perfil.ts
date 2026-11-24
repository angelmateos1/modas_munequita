import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ClientesService, ActualizarPerfil } from '../../services/clientes';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-mi-perfil',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './mi-perfil.html',
  styleUrl: './mi-perfil.css'
})
export class MiPerfilComponent implements OnInit {
  private clientesService = inject(ClientesService);
  private authService = inject(AuthService);

  perfil: ActualizarPerfil = {
    nombreCompleto: '',
    email: '',
    telefono: '',
    passwordActual: '',
    nuevaPassword: ''
  };

  cargando = signal(true);
  mensajeOk = signal('');
  mensajeError = signal('');

  ngOnInit() {
    this.clientesService.obtenerPerfil().subscribe({
      next: (datos) => {
        this.perfil.nombreCompleto = datos.nombreCompleto;
        this.perfil.email = datos.email;
        this.perfil.telefono = datos.telefono;
        this.cargando.set(false);
      },
      error: () => {
        this.mensajeError.set('No se pudieron cargar tus datos.');
        this.cargando.set(false);
      }
    });
  }

  guardarPerfil() {
    this.mensajeOk.set('');
    this.mensajeError.set('');

    this.clientesService.actualizarPerfil(this.perfil).subscribe({
      next: (actualizado) => {
        this.authService.actualizarDatosUsuario(actualizado.nombreCompleto, actualizado.email);
        this.perfil.passwordActual = '';
        this.perfil.nuevaPassword = '';
        this.mensajeOk.set('¡Tus datos se han actualizado correctamente!');
      },
      error: (err) => {
        this.mensajeError.set(err.error?.mensaje || 'Error al guardar los cambios.');
      }
    });
  }
}