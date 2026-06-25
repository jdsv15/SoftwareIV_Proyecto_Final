import 'package:flutter/material.dart';

import 'padecimientos_screen.dart';
import 'tratamientos_screen.dart';
import 'medicamentos_screen.dart';
import 'archivos_screen.dart';
import 'historial_screen.dart';

class MenuScreen extends StatelessWidget {
  final String cedula;
  final String nombrePaciente;

  const MenuScreen({
    super.key,
    required this.cedula,
    required this.nombrePaciente,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Menú Principal'),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            children: [
              const SizedBox(height: 10),
              Text(
                'Paciente: $nombrePaciente',
                style: const TextStyle(
                  fontSize: 22,
                  fontWeight: FontWeight.bold,
                ),
              ),
              const SizedBox(height: 30),

              _boton(
                context,
                'Ver Padecimientos Asignados',
                Icons.health_and_safety,
                PadecimientosScreen(cedula: cedula),
              ),

              _boton(
                context,
                'Ver Tratamientos Asignados',
                Icons.medical_services,
                TratamientosScreen(cedula: cedula),
              ),

              _boton(
                context,
                'Ver Medicamentos Asignados',
                Icons.local_pharmacy,
                MedicamentosScreen(cedula: cedula),
              ),

              _boton(
                context,
                'Ver Exámenes de Laboratorio',
                Icons.description,
                ArchivosScreen(cedula: cedula),
              ),

              _boton(
                context,
                'Ver Historial Clínico',
                Icons.history,
                HistorialScreen(cedula: cedula),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _boton(
    BuildContext context,
    String texto,
    IconData icono,
    Widget pantalla,
  ) {
    return Container(
      margin: const EdgeInsets.only(bottom: 15),
      width: double.infinity,
      height: 65,
      child: ElevatedButton.icon(
        onPressed: () {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => pantalla),
          );
        },
        icon: Icon(icono),
        label: Text(
          texto,
          textAlign: TextAlign.center,
        ),
      ),
    );
  }
}