class TratamientoModel {
  final int id;
  final String nombre;
  final String fechaAsignacion;
  final String estado;

  TratamientoModel({
    required this.id,
    required this.nombre,
    required this.fechaAsignacion,
    required this.estado,
  });

  factory TratamientoModel.fromJson(Map<String, dynamic> json) {
    return TratamientoModel(
      id: json['id'],
      nombre: json['nombre'] ?? '',
      fechaAsignacion: json['fechaAsignacion'] ?? '',
      estado: json['estado'] ?? '',
    );
  }
}