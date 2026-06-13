import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';
import 'package:http/http.dart' as http;

class IsbnScanner extends StatefulWidget {
  const IsbnScanner({super.key});

  @override
  State<IsbnScanner> createState() => _IsbnScannerState();
}

class _IsbnScannerState extends State<IsbnScanner> {
  Barcode? barcode;

  Future<Album>? futureAlbum;

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    Widget barcodePreview(Barcode? value) {
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

    void handleBarcode(BarcodeCapture barcodes) {
      var code = barcodes.barcodes.firstOrNull;
      if (mounted) {
        setState(() {
          barcode = code;
        });
        var displayValue = code?.displayValue;
        if (displayValue != null && futureAlbum == null) {
          futureAlbum = fetchAlbum(displayValue);
          showDialog<void>(
            context: context,
            builder: (BuildContext context) {
              return AlertDialog(
                title: Text("🔍 ISBN Search"),
                content: FutureBuilder<Album>(
                  future: futureAlbum,
                  builder: (context, snapshot) {
                    if (snapshot.hasData) {
                      return Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisSize: MainAxisSize.min,
                        spacing: 10,
                        children: [
                          Text("> Is this your book?", style: TextStyle(
                              fontSize: 16,
                              color: Color.fromARGB(255, 140, 140, 140),
                            )),
                          Text(
                            snapshot.data!.title,
                            style: TextStyle(
                              fontSize: 24,
                              fontWeight: FontWeight.bold,
                              color: Color(0xFF424242),
                            ),
                          ),
                          Text("ISBN: ${snapshot.data!.isbn}"),
                          Image(
                              image: NetworkImage(snapshot.data!.thumbnailUrl),
                            ),
                        ],
                      );
                    } else if (snapshot.hasError) {
                      return Text('${snapshot.error}');
                    }

                    // By default, show a loading spinner.
                    return SizedBox(height: 50, width: 50, child: const CircularProgressIndicator());
                  },
                ),
              );
            },
          ).then((val) {
            futureAlbum = null;
          });
        }
      }
    }

    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          onPressed: () => {Navigator.of(context).pop()},
          icon: Icon(Icons.arrow_back),
        ),
      ),
      body: Stack(
        children: [
          MobileScanner(onDetect: handleBarcode, tapToFocus: true),
          Align(
            alignment: Alignment.bottomCenter,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  alignment: Alignment.bottomCenter,
                  height: 100,
                  color: const Color.fromRGBO(0, 0, 0, 0.4),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      Expanded(child: Center(child: barcodePreview(barcode))),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class Album {
  final String thumbnailUrl;
  final String isbn;
  final String title;

  const Album({
    required this.thumbnailUrl,
    required this.isbn,
    required this.title,
  });

  factory Album.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'thumbnailUrl': String thumbnailUrl,
        'isbn': String isbn,
        'title': String title,
      } =>
        Album(thumbnailUrl: thumbnailUrl, isbn: isbn, title: title),
      _ => throw const FormatException('Failed to load album.'),
    };
  }
}

Future<Album> fetchAlbum(String isbn) async {
  final response = await http.get(
    Uri.parse('http://localhost:5260/books-api?isbn=$isbn'),
    headers: {'Accept': 'application/json'},
  );

  if (response.statusCode == 200) {
    // If the server did return a 200 OK response,
    // then parse the JSON.
    return Album.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
  } else {
    // If the server did not return a 200 OK response,
    // then throw an exception.
    throw Exception('Failed to load album');
  }
}
