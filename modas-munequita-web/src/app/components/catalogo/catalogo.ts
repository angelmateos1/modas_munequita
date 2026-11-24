import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { PrendasService, Prenda, CrearPrenda } from '../../services/prendas';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-catalogo',
  standalone: true,
  imports: [FormsModule, CurrencyPipe],
  templateUrl: './catalogo.html',
  styleUrl: './catalogo.css'
})
export class CatalogoComponent implements OnInit {
  private prendasService = inject(PrendasService);
  authService = inject(AuthService);

  prendas = signal<Prenda[]>([]);
  mostrarFormulario = signal(false);

  nuevaPrenda: CrearPrenda = {
    nombre: '',
    descripcion: '',
    categoria: 'Vestidos',
    talla: 'M',
    precio: 0,
    stock: 1,
    imagenUrl: ''
  };

  ngOnInit() {
    this.cargarPrendas();
  }

  cargarPrendas() {
    this.prendasService.obtenerPrendas().subscribe({
      next: (datos) => this.prendas.set(datos),
      error: (err) => console.error('Error al cargar prendas', err)
    });
  }

  guardarPrenda() {
    this.prendasService.crearPrenda(this.nuevaPrenda).subscribe({
      next: (creada) => {
        this.prendas.update(lista => [...lista, creada]);
        this.mostrarFormulario.set(false);
        this.nuevaPrenda = {
          nombre: '',
          descripcion: '',
          categoria: 'Vestidos',
          talla: 'M',
          precio: 0,
          stock: 1,
          imagenUrl: ''
        };
      },
      error: (err) => console.error('Error al crear prenda', err)
    });
  }

  borrarPrenda(id: number) {
    this.prendasService.eliminarPrenda(id).subscribe({
      next: () => {
        this.prendas.update(lista => lista.filter(p => p.id !== id));
      },
      error: (err) => console.error('Error al eliminar prenda', err)
    });
  }
}