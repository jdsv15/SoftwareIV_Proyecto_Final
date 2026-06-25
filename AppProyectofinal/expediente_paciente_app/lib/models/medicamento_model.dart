class MedicamentoModel {
  final int id;
  final String nombre;
  final String fechaAsignacion;
  final String estado;

  MedicamentoModel({
    required this.id,
    required this.nombre,
    required this.fechaAsignacion,
    required this.estado,
  });

  factory MedicamentoModel.fromJson(Map<String, dynamic> json) {
    return MedicamentoModel(
      id: json['id'],
      nombre: json['nombre'] ?? '',
      fechaAsignacion: json['fechaAsignacion'] ?? '',
      estado: json['estado'] ?? '',
    );
  }
}