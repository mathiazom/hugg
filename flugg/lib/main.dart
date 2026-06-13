import 'package:flutter/material.dart';
import 'package:hugg/widgets/isbn_scanner.dart';

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

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('🐍'), centerTitle: false),
      floatingActionButton: currentPageIndex == 2
          ? FloatingActionButton(
              onPressed: () => {
                Navigator.of(context).push(
                  MaterialPageRoute<void>(
                    builder: (context) => const IsbnScanner(),
                  ),
                ),
              },
              backgroundColor: Colors.white,
              child: Icon(
                Icons.barcode_reader,
                size: 28,
                color: Color(0xFF66BB6A),
              ),
            )
          : null,
      body: Container(
        padding: EdgeInsets.symmetric(vertical: 10, horizontal: 20),
        child: [
          Text(
          "Reading Now",
          style: TextStyle(
            fontSize: 32,
            fontWeight: FontWeight.bold,
            color: Color(0xFF424242),
          ),
        ),
        Text(
          "Upcoming",
          style: TextStyle(
            fontSize: 32,
            fontWeight: FontWeight.bold,
            color: Color(0xFF424242),
          ),
        ),
        Text(
          "My Books",
          style: TextStyle(
            fontSize: 32,
            fontWeight: FontWeight.bold,
            color: Color(0xFF424242),
          ),
        )
        ][currentPageIndex],
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
