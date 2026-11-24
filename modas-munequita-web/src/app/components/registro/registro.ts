import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './registro.html',
  styleUrl: './registro.css'
})
export class RegistroComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  nombreCompleto = '';
  email = '';
  password = '';
  error = signal('');
  exito = signal('');

  registrarse() {
    this.error.set('');
    this.exito.set('');

    this.authService.register({
      nombreCompleto: this.nombreCompleto,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.exito.set('¡Cuenta creada! Redirigiendo al inicio de sesión...');
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err) => {
        this.error.set(err.error?.mensaje || 'No se pudo completar el registro. Revisa la contraseña (mínimo 6 caracteres, mayúscula, número y símbolo).');
      }
    });
  }
}