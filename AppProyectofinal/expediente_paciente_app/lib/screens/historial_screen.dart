import 'package:flutter/material.dart';

import '../models/historial_model.dart';
import '../services/api_service.dart';
import 'detalle_nota_screen.dart';

class HistorialScreen extends StatefulWidget {
  final String cedula;

  const HistorialScreen({
    super.key,
    required this.cedula,
  });

  @override
  State<HistorialScreen> createState() => _HistorialScreenState();
}

class _HistorialScreenState extends State<HistorialScreen> {
  final ApiService apiService = ApiService();
  late Future<List<HistorialModel>> historial;

  @override
  void initState() {
    super.initState();
    historial = apiService.obtenerHistorial(widget.cedula);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Historial Clínico'),
      ),
      body: FutureBuilder<List<HistorialModel>>(
        future: historial,
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
              child: Text('No hay notas clínicas registradas.'),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(12),
            itemCount: lista.length,
            itemBuilder: (context, index) {
              final item = lista[index];

              return Card(
                child: ListTile(
                  leading: const Icon(Icons.history),
                  title: Text(item.fechaHora),
                  subtitle: Text(item.resumen),
                  trailing: const Icon(Icons.arrow_forward_ios),
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (_) => DetalleNotaScreen(idNota: item.id),
                      ),
                    );
                  },
                ),
              );
            },
          );
        },
      ),
    );
  }
}