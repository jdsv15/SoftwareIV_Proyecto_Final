import 'package:flutter/material.dart';

import '../models/tratamiento_model.dart';
import '../services/api_service.dart';

class TratamientosScreen extends StatefulWidget {
  final String cedula;

  const TratamientosScreen({
    super.key,
    required this.cedula,
  });

  @override
  State<TratamientosScreen> createState() => _TratamientosScreenState();
}

class _TratamientosScreenState extends State<TratamientosScreen> {
  final ApiService apiService = ApiService();
  late Future<List<TratamientoModel>> tratamientos;

  @override
  void initState() {
    super.initState();
    tratamientos = apiService.obtenerTratamientos(widget.cedula);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Tratamientos'),
      ),
      body: FutureBuilder<List<TratamientoModel>>(
        future: tratamientos,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }

          if (snapshot.hasError) {
            return Center(child: Text('Error: ${snapshot.error}'));
          }

          final lista = snapshot.data ?? [];

          if (lista.isEmpty) {
            return const Center(
              child: Text('No hay tratamientos asignados.'),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(12),
            itemCount: lista.length,
            itemBuilder: (context, index) {
              final item = lista[index];

              return Card(
                child: ListTile(
                  leading: const Icon(Icons.medical_services),
                  title: Text(item.nombre),
                  subtitle: Text(
                    'Fecha: ${item.fechaAsignacion}\nEstado: ${item.estado}',
                  ),
                ),
              );
            },
          );
        },
      ),
    );
  }
}