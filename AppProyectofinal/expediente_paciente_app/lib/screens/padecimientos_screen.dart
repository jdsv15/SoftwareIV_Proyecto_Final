import 'package:flutter/material.dart';

import '../models/padecimiento_model.dart';
import '../services/api_service.dart';

class PadecimientosScreen extends StatefulWidget {
  final String cedula;

  const PadecimientosScreen({
    super.key,
    required this.cedula,
  });

  @override
  State<PadecimientosScreen> createState() => _PadecimientosScreenState();
}

class _PadecimientosScreenState extends State<PadecimientosScreen> {
  final ApiService apiService = ApiService();
  late Future<List<PadecimientoModel>> padecimientos;

  @override
  void initState() {
    super.initState();
    padecimientos = apiService.obtenerPadecimientos(widget.cedula);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Padecimientos'),
      ),
      body: FutureBuilder<List<PadecimientoModel>>(
        future: padecimientos,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }

          if (snapshot.hasError) {
            return Center(
              child: Text('Error: ${snapshot.error}'),
            );
          }

          final lista = snapshot.data ?? [];

          if (lista.isEmpty) {
            return const Center(
              child: Text('No hay padecimientos asignados.'),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(12),
            itemCount: lista.length,
            itemBuilder: (context, index) {
              final item = lista[index];

              return Card(
                child: ListTile(
                  leading: const Icon(Icons.health_and_safety),
                  title: Text(item.nombre),
                  subtitle: Text(
                    'Fecha: ${item.fechaDiagnostico}\nEstado: ${item.estado}',
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