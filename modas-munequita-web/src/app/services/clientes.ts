import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

export interface PagoDita {
  id: number;
  abonado: number;
  fechaPago: string;
  concepto: string;
}

export interface ClienteResumen {
  id: string;
  nombreCompleto: string;
  email: string;
  telefono: string;
  deudaActual: number;
  diaCobroDita: number;
  notasAdmin: string;
  esClienteActivo: boolean;
}

export interface ClienteDetalle extends ClienteResumen {
  historialPagos: PagoDita[];
}

export interface ActualizarCliente {
  nombreCompleto: string;
  telefono: string;
  deudaActual: number;
  diaCobroDita: number;
  notasAdmin: string;
  esClienteActivo: boolean;
}

export interface RegistrarPago {
  abonado: number;
  concepto: string;
}

@Injectable({
  providedIn: 'root'
})
export class ClientesService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5051/api/admin/clientes';

  obtenerClientes(diaCobro?: number) {
    let params = new HttpParams();
    if (diaCobro) {
      params = params.set('diaCobro', diaCobro.toString());
    }
    return this.http.get<ClienteResumen[]>(this.apiUrl, { params });
  }

  obtenerDetalle(id: string) {
    return this.http.get<ClienteDetalle>(`${this.apiUrl}/${id}`);
  }

  actualizarCliente(id: string, datos: ActualizarCliente) {
    return this.http.put(`${this.apiUrl}/${id}`, datos);
  }

  registrarPago(id: string, pago: RegistrarPago) {
    return this.http.post<PagoDita>(`${this.apiUrl}/${id}/pagos`, pago);
  }
}