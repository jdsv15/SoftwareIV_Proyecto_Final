import 'package:flutter/material.dart';

import '../services/api_service.dart';

class DetalleNotaScreen extends StatefulWidget {
  final int idNota;

  const DetalleNotaScreen({
    super.key,
    required this.idNota,
  });

  @override
  State<DetalleNotaScreen> createState() => _DetalleNotaScreenState();
}

class _DetalleNotaScreenState extends State<DetalleNotaScreen> {
  final ApiService apiService = ApiService();
  late Future<Map<String, dynamic>> detalleNota;

  @override
  void initState() {
    super.initState();
    detalleNota = apiService.obtenerDetalleNota(widget.idNota);
  }

  String limpiarHtml(String texto) {
    return texto.replaceAll(RegExp(r'<[^>]*>'), '');
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Detalle de Nota'),
      ),
      body: FutureBuilder<Map<String, dynamic>>(
        future: detalleNota,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }

          if (snapshot.hasError) {
            return Center(child: Text('Error: ${snapshot.error}'));
          }

          final nota = snapshot.data;

          if (nota == null) {
            return const Center(child: Text('No se encontró la nota.'));
          }

          return Padding(
            padding: const EdgeInsets.all(16),
            child: Card(
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      nota['fechaHora'] ?? '',
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 18,
                      ),
                    ),
                    const Divider(),
                    Text(
                      limpiarHtml(nota['notas'] ?? ''),
                      style: const TextStyle(fontSize: 16),
                    ),
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}