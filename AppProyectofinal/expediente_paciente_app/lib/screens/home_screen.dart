import 'package:flutter/material.dart';
import 'menu_screen.dart';
import '../services/api_service.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final TextEditingController cedulaController = TextEditingController();

  Future<void> ingresar() async {
    final cedula = cedulaController.text.trim();

    if (cedula.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Ingrese su cédula')),
      );
      return;
    }

    try {
      final paciente = await ApiService().obtenerInfoPaciente(cedula);

      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (_) => MenuScreen(
            cedula: cedula,
            nombrePaciente: paciente['nombre'] ?? '',
          ),
        ),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('No se encontró el paciente')),
      );
    }
  }

  @override
  void dispose() {
    cedulaController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Expediente Médico'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Icon(
              Icons.medical_information,
              size: 90,
              color: Colors.blue,
            ),
            const SizedBox(height: 25),
            const Text(
              'Consulta de Expediente',
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(height: 25),
            TextField(
              controller: cedulaController,
              keyboardType: TextInputType.text,
              decoration: const InputDecoration(
                labelText: 'Cédula del paciente',
                border: OutlineInputBorder(),
                prefixIcon: Icon(Icons.badge),
              ),
            ),
            const SizedBox(height: 20),
            SizedBox(
              width: double.infinity,
              height: 48,
              child: ElevatedButton(
                onPressed: ingresar,
                child: const Text('Consultar expediente'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}