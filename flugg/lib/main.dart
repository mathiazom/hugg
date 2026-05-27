import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';

void main() {
  runApp(MaterialApp(title: 'hugg', home: Home()));
}

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<StatefulWidget> createState() => _Home();
}

class _Home extends State<Home> {
  int currentPageIndex = 0;

  final titles = <String>["Reading Now", "Upcoming", "My Books"];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('🐍'),
        centerTitle: false,
      ),
      floatingActionButton: currentPageIndex == 2 ? FloatingActionButton(onPressed: () => {
        Navigator.of(context).push(
          MaterialPageRoute<void>(builder: (context) => const MobileScannerSimple())
        )
      }, backgroundColor: Colors.white, child: Icon(Icons.barcode_reader, size: 28, color: Color(0xFF66BB6A),),) : null,
      body: Container(
        padding: EdgeInsets.symmetric(vertical: 10, horizontal: 20),
        child: Text(
          titles.elementAt(currentPageIndex),
          style: TextStyle(
            fontSize: 32,
            fontWeight: FontWeight.bold,
            color: Color(0xFF424242),
          ),
        ),
      ),
      bottomNavigationBar: NavigationBar(
        indicatorColor: Color(0xFFC8E6C9),
        selectedIndex: currentPageIndex,
        onDestinationSelected: (int index) {
          setState(() {
            currentPageIndex = index;
          });
        },
        destinations: const <Widget>[
          NavigationDestination(
            icon: Icon(Icons.book, size: 28),
            selectedIcon: Icon(Icons.book, size: 28, color: Color(0xFF66BB6A)),
            label: "Reading Now",
          ),
          NavigationDestination(
            icon: Icon(Icons.upcoming, size: 28),
            selectedIcon: Icon(
              Icons.upcoming,
              size: 28,
              color: Color(0xFF66BB6A),
            ),
            label: "Upcoming",
          ),
          NavigationDestination(
            icon: Icon(Icons.library_books, size: 28),
            selectedIcon: Icon(
              Icons.library_books,
              size: 28,
              color: Color(0xFF66BB6A),
            ),
            label: "My Books",
          ),
        ],
      ),
    );
  }
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
      appBar: AppBar(leading: IconButton(onPressed: () => {
        Navigator.of(context).pop()
      }, icon: Icon(Icons.arrow_back))),
      body: Stack(children: [
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
      ],),
    );
  }
}
