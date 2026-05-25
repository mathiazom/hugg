import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';


void main() {
  runApp(
    const MaterialApp(title: 'hugg', home: MobileScannerSimple()),
  );
}

class MobileScannerSimple extends StatefulWidget {
  const MobileScannerSimple({super.key});

  @override
  State<MobileScannerSimple> createState() => _MobileScannerSimpleState();
}

class _MobileScannerSimpleState extends State<MobileScannerSimple> {
  Barcode? barcode;

  @override
  Widget build(BuildContext context) {

    Widget _barcodePreview(Barcode? value) {
      if (value == null) {
        return const Text(
          'Scan something!',
          overflow: TextOverflow.fade,
          style: TextStyle(color: Colors.white),
        );
      }

      return Text(
        value.displayValue ?? 'No display value.',
        overflow: TextOverflow.fade,
        style: const TextStyle(color: Colors.white),
      );
    }

    void _handleBarcode(BarcodeCapture barcodes) {
      if (mounted) {
        setState(() {
          barcode = barcodes.barcodes.firstOrNull;
        });
      }
    }

    return Scaffold(
      appBar: AppBar(title: const Text('🐍 hugg')),
      backgroundColor: Colors.black,
      body: Stack(
        children: [
          MobileScanner(onDetect: _handleBarcode, tapToFocus: true),
          Align(
            alignment: Alignment.bottomCenter,
            child: Container(
              alignment: Alignment.bottomCenter,
              height: 100,
              color: const Color.fromRGBO(0, 0, 0, 0.4),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  Expanded(child: Center(child: _barcodePreview(barcode))),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
