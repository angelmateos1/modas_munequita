import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Prenda {
  id: number;
  nombre: string;
  descripcion: string;
  categoria: string;
  talla: string;
  precio: number;
  stock: number;
  imagenUrl: string;
}

export type CrearPrenda = Omit<Prenda, 'id'>;

@Injectable({
  providedIn: 'root'
})
export class PrendasService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5051/api/prendas';

  obtenerPrendas() {
    return this.http.get<Prenda[]>(this.apiUrl);
  }

  crearPrenda(prenda: CrearPrenda) {
    return this.http.post<Prenda>(this.apiUrl, prenda);
  }

  eliminarPrenda(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}