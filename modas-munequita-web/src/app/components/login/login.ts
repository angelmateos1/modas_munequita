import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  password = '';
  error = signal('');

  iniciarSesion() {
    this.error.set('');
    this.authService.login(this.email, this.password).subscribe({
      next: () => this.router.navigate(['/']),
      error: () => this.error.set('Correo o contraseña incorrectos.')
    });
  }
}