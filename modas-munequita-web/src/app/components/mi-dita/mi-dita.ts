import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ClientesService, ClienteDetalle } from '../../services/clientes';

@Component({
  selector: 'app-mi-dita',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './mi-dita.html',
  styleUrl: './mi-dita.css'
})
export class MiDitaComponent implements OnInit {
  private clientesService = inject(ClientesService);

  datosDita = signal<ClienteDetalle | null>(null);
  cargando = signal(true);

  ngOnInit() {
    this.clientesService.obtenerMiDita().subscribe({
      next: (datos) => {
        this.datosDita.set(datos);
        this.cargando.set(false);
      },
      error: (err) => {
        console.error('Error al cargar Mi Dita', err);
        this.cargando.set(false);
      }
    });
  }
}