import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, DatePipe } from '@angular/common';
import {
  ClientesService,
  ClienteResumen,
  ClienteDetalle,
  RegistrarPago
} from '../../services/clientes';

@Component({
  selector: 'app-panel-dita',
  standalone: true,
  imports: [FormsModule, CurrencyPipe, DatePipe],
  templateUrl: './panel-dita.html',
  styleUrl: './panel-dita.css'
})
export class PanelDitaComponent implements OnInit {
  private clientesService = inject(ClientesService);

  clientes = signal<ClienteResumen[]>([]);
  clienteSeleccionado = signal<ClienteDetalle | null>(null);
  filtroDia: number | null = null;
  mensaje = signal('');

  nuevoPago: RegistrarPago = {
    abonado: 0,
    concepto: 'Abono mensual'
  };

  ngOnInit() {
    this.cargarClientes();
  }

  cargarClientes() {
    this.clientesService.obtenerClientes(this.filtroDia ?? undefined).subscribe({
      next: (datos) => this.clientes.set(datos),
      error: (err) => console.error('Error al cargar clientes', err)
    });
  }

  filtrarHoy() {
    this.filtroDia = new Date().getDate();
    this.cargarClientes();
  }

  limpiarFiltro() {
    this.filtroDia = null;
    this.cargarClientes();
  }

  seleccionarCliente(id: string) {
    this.mensaje.set('');
    this.clientesService.obtenerDetalle(id).subscribe({
      next: (detalle) => this.clienteSeleccionado.set(detalle),
      error: (err) => console.error('Error al cargar detalle', err)
    });
  }

  guardarCambiosCliente() {
    const cliente = this.clienteSeleccionado();
    if (!cliente) return;

    this.clientesService.actualizarCliente(cliente.id, {
      nombreCompleto: cliente.nombreCompleto,
      telefono: cliente.telefono,
      deudaActual: cliente.deudaActual,
      diaCobroDita: cliente.diaCobroDita,
      notasAdmin: cliente.notasAdmin,
      esClienteActivo: cliente.esClienteActivo
    }).subscribe({
      next: () => {
        this.mensaje.set('Datos actualizados correctamente.');
        this.cargarClientes();
      },
      error: (err) => console.error('Error al actualizar cliente', err)
    });
  }

  registrarAbono() {
    const cliente = this.clienteSeleccionado();
    if (!cliente || this.nuevoPago.abonado <= 0) return;

    this.clientesService.registrarPago(cliente.id, this.nuevoPago).subscribe({
      next: () => {
        this.nuevoPago = { abonado: 0, concepto: 'Abono mensual' };
        this.mensaje.set('Abono registrado y descontado de la deuda.');
        this.seleccionarCliente(cliente.id);
        this.cargarClientes();
      },
      error: (err) => console.error('Error al registrar pago', err)
    });
  }
}