import 'package:flutter/material.dart';
import 'screens/home_screen.dart';

void main() {
  runApp(const ExpedientePacienteApp());
}

class ExpedientePacienteApp extends StatelessWidget {
  const ExpedientePacienteApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Expediente Paciente',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        primarySwatch: Colors.blue,
        useMaterial3: false,
      ),
      home: const HomeScreen(),
    );
  }
}