class PadecimientoModel {
  final int id;
  final String nombre;
  final String fechaDiagnostico;
  final String estado;

  PadecimientoModel({
    required this.id,
    required this.nombre,
    required this.fechaDiagnostico,
    required this.estado,
  });

  factory PadecimientoModel.fromJson(Map<String, dynamic> json) {
    return PadecimientoModel(
      id: json['id'],
      nombre: json['nombre'] ?? '',
      fechaDiagnostico: json['fechaDiagnostico'] ?? '',
      estado: json['estado'] ?? '',
    );
  }
}