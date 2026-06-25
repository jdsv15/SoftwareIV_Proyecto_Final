import 'dart:convert';

import 'package:http/http.dart' as http;

import '../models/padecimiento_model.dart';
import '../models/tratamiento_model.dart';
import '../models/medicamento_model.dart';
import '../models/archivo_model.dart';
import '../models/historial_model.dart';

class ApiService {
  
static const String baseUrl =
    'http://proyectofinalsoftware.runasp.net/api/mobile/paciente';
    

  Future<List<PadecimientoModel>> obtenerPadecimientos(
      String cedula) async {

    final response =
        await http.get(Uri.parse('$baseUrl/$cedula/padecimientos'));

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);

      return data
          .map((item) => PadecimientoModel.fromJson(item))
          .toList();
    }

    throw Exception('Error al cargar padecimientos');
  }

  Future<List<TratamientoModel>> obtenerTratamientos(
      String cedula) async {

    final response =
        await http.get(Uri.parse('$baseUrl/$cedula/tratamientos'));

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);

      return data
          .map((item) => TratamientoModel.fromJson(item))
          .toList();
    }

    throw Exception('Error al cargar tratamientos');
  }

  Future<List<MedicamentoModel>> obtenerMedicamentos(
      String cedula) async {

    final response =
        await http.get(Uri.parse('$baseUrl/$cedula/medicamentos'));

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);

      return data
          .map((item) => MedicamentoModel.fromJson(item))
          .toList();
    }

    throw Exception('Error al cargar medicamentos');
  }

  Future<List<ArchivoModel>> obtenerArchivos(
      String cedula) async {

    final response =
        await http.get(Uri.parse('$baseUrl/$cedula/archivos'));

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);

      return data
          .map((item) => ArchivoModel.fromJson(item))
          .toList();
    }

    throw Exception('Error al cargar archivos');
  }

  Future<List<HistorialModel>> obtenerHistorial(
      String cedula) async {

    final response =
        await http.get(Uri.parse('$baseUrl/$cedula/historial'));

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);

      return data
          .map((item) => HistorialModel.fromJson(item))
          .toList();
    }

    throw Exception('Error al cargar historial clínico');
  }

  Future<Map<String, dynamic>> obtenerDetalleNota(int idNota) async {
  final response = await http.get(
    Uri.parse('$baseUrl/historial/detalle/$idNota'),
  );

  if (response.statusCode == 200) {
    return jsonDecode(response.body);
  }

  throw Exception('Error al cargar el detalle de la nota');
}

Future<Map<String, dynamic>> obtenerInfoPaciente(String cedula) async {
  final response = await http.get(
    Uri.parse('$baseUrl/$cedula/info'),
  );

  if (response.statusCode == 200) {
    return jsonDecode(response.body);
  }

  throw Exception('Paciente no encontrado');
}
}