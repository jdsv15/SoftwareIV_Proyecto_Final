import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

import '../models/archivo_model.dart';
import '../services/api_service.dart';

class ArchivosScreen extends StatefulWidget {
  final String cedula;

  const ArchivosScreen({
    super.key,
    required this.cedula,
  });

  @override
  State<ArchivosScreen> createState() => _ArchivosScreenState();
}

class _ArchivosScreenState extends State<ArchivosScreen> {
  final ApiService apiService = ApiService();

  static const platform = MethodChannel('expediente_paciente/open_url');

  late Future<List<ArchivoModel>> archivos;

  @override
  void initState() {
    super.initState();
    archivos = apiService.obtenerArchivos(widget.cedula);
  }

  Future<void> abrirArchivo(String url) async {
    try {
      await platform.invokeMethod('openUrl', {'url': url});
    } catch (_) {
      Clipboard.setData(ClipboardData(text: url));

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('No se pudo abrir. Enlace copiado al portapapeles.'),
        ),
      );
    }
  }

  void verDetalleArchivo(ArchivoModel archivo) {
    showDialog(
      context: context,
      builder: (_) => AlertDialog(
        title: Text(archivo.descripcion),
        content: SelectableText(
          'Archivo: ${archivo.nombreArchivo}\n'
          'Fecha: ${archivo.fechaSubida}\n\n'
          'URL:\n${archivo.urlArchivo}',
        ),
        actions: [
          TextButton(
            onPressed: () {
              abrirArchivo(archivo.urlArchivo);
              Navigator.pop(context);
            },
            child: const Text('Abrir archivo'),
          ),
          TextButton(
            onPressed: () {
              Clipboard.setData(ClipboardData(text: archivo.urlArchivo));
              Navigator.pop(context);

              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Enlace copiado.')),
              );
            },
            child: const Text('Copiar enlace'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cerrar'),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Exámenes de Laboratorio'),
      ),
      body: FutureBuilder<List<ArchivoModel>>(
        future: archivos,
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
              child: Text('No hay exámenes registrados.'),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(12),
            itemCount: lista.length,
            itemBuilder: (context, index) {
              final item = lista[index];

              return Card(
                child: ListTile(
                  leading: const Icon(Icons.picture_as_pdf),
                  title: Text(item.descripcion),
                  subtitle: Text(
                    '${item.nombreArchivo}\nFecha: ${item.fechaSubida}',
                  ),
                  trailing: const Icon(Icons.open_in_new),
                  onTap: () {
                    verDetalleArchivo(item);
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