import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

export interface AuthRespuesta {
  token: string;
  email: string;
  nombreCompleto: string;
  rol: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5051/api/Auth'; 

  // Signal reactiva para saber en toda la web quién está conectado
  usuarioActual = signal<AuthRespuesta | null>(this.cargarUsuarioGuardado());

  login(email: string, password: string) {
    return this.http.post<AuthRespuesta>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(respuesta => {
        localStorage.setItem('usuario_munequita', JSON.stringify(respuesta));
        this.usuarioActual.set(respuesta);
      })
    );
  }

  register(datos: { nombreCompleto: string; email: string; password: string }) {
    return this.http.post(`${this.apiUrl}/register`, datos);
  }

  logout() {
    localStorage.removeItem('usuario_munequita');
    this.usuarioActual.set(null);
  }

  obtenerToken(): string | null {
    return this.usuarioActual()?.token ?? null;
  }

  esAdmin(): boolean {
    return this.usuarioActual()?.rol === 'Admin';
  }

  private cargarUsuarioGuardado(): AuthRespuesta | null {
    const data = localStorage.getItem('usuario_munequita');
    return data ? JSON.parse(data) : null;
  }

  actualizarDatosUsuario(nombreCompleto: string, email: string) {
    const actual = this.usuarioActual();
    if (!actual) return;

    const actualizado = { ...actual, nombreCompleto, email };
    this.usuarioActual.set(actualizado);
    localStorage.setItem('usuario', JSON.stringify(actualizado));
  }
}