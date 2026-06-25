import 'package:flutter/material.dart';

import '../models/medicamento_model.dart';
import '../services/api_service.dart';

class MedicamentosScreen extends StatefulWidget {
  final String cedula;

  const MedicamentosScreen({
    super.key,
    required this.cedula,
  });

  @override
  State<MedicamentosScreen> createState() => _MedicamentosScreenState();
}

class _MedicamentosScreenState extends State<MedicamentosScreen> {
  final ApiService apiService = ApiService();
  late Future<List<MedicamentoModel>> medicamentos;

  @override
  void initState() {
    super.initState();
    medicamentos = apiService.obtenerMedicamentos(widget.cedula);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Medicamentos'),
      ),
      body: FutureBuilder<List<MedicamentoModel>>(
        future: medicamentos,
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
              child: Text('No hay medicamentos asignados.'),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(12),
            itemCount: lista.length,
            itemBuilder: (context, index) {
              final item = lista[index];

              return Card(
                child: ListTile(
                  leading: const Icon(Icons.local_pharmacy),
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