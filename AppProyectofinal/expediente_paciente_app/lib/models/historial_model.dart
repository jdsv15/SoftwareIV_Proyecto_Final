class HistorialModel {
  final int id;
  final String fechaHora;
  final String resumen;

  HistorialModel({
    required this.id,
    required this.fechaHora,
    required this.resumen,
  });

  factory HistorialModel.fromJson(Map<String, dynamic> json) {
    return HistorialModel(
      id: json['id'],
      fechaHora: json['fechaHora'] ?? '',
      resumen: json['resumen'] ?? '',
    );
  }
}