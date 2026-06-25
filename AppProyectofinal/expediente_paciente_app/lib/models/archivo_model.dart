class ArchivoModel {
  final int id;
  final String descripcion;
  final String nombreArchivo;
  final String fechaSubida;
  final String urlArchivo;

  ArchivoModel({
    required this.id,
    required this.descripcion,
    required this.nombreArchivo,
    required this.fechaSubida,
    required this.urlArchivo,
  });

  factory ArchivoModel.fromJson(Map<String, dynamic> json) {
    return ArchivoModel(
      id: json['id'],
      descripcion: json['descripcion'] ?? '',
      nombreArchivo: json['nombreArchivo'] ?? '',
      fechaSubida: json['fechaSubida'] ?? '',
      urlArchivo: json['urlArchivo'] ?? '',
    );
  }
}