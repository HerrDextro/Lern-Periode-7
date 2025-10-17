# Lern-Periode-7
Janky Graphic engine in console cursed creation thing
*25.10 bis 20.12*

Grob-Planung
Ich werde mit Alexander Straub ein Projekt machen. 

25.10.2024
- [x] Class und Funktion um Console Lines zu schreiben 
- [x] Funktion um individuelle Pixel zu setzen

Heute haben wir ein Framework gemacht, um mit dem █-Character in der Konsole einzelne Pixel zu setzen. Das Ziel ist es, eine Art Graphics Engine zu machen, die Daten von einem 2-D-Array nehmen kann (Position x, y und Farbe in string) und sie dann wiedergeben kann. Wir möchten, dass die Linien in der Konsole einzeln geupdated werden, damit es nicht bei jedem Update kurz schwarz wird. Schlussendlich soll dieses Graphics Engine dazu dienen, Konsolenspiele zu machen.

![WhatsApp Image 2024-10-25 at 11 54 29_193456d3](https://github.com/user-attachments/assets/43b10365-5701-49b1-8741-d234af51c088)

![WhatsApp Bild 2024-10-25 um 15 16 54_31aa9c01](https://github.com/user-attachments/assets/37c5155d-e5b5-4095-9eb4-40d2001cfa32)




1.11.2024
- [x] Funktion um Linien individuell zu refreshen
- [X] Load images from .txt file (in █ Format)
- [X] Load custom textures (Farbwerte für die Bilder, die man reinloadet)
- [x] Read user Key input (perhaps in second console window)
- [x] Create documentation
- [x] Main Menu mit Spielauswahl

Heute haben wir das Framework praktisch fertig gemacht. Man kann Bilder, Texturen von .txt Dateien importieren, alle Keys nutzen (theoretisch, wir nutzen jetzt nur WASD oder Pfeile) und wir sind jetzt bereit, ein paar einfache Games zu erstellen, um zu testen. Wir haben auch eine Dokumentation gemacht, damit wir nicht bei der Erstellung neuer Spiele irren. Wir haben das Main Menu, mit einer Auswahl von 2 Spiele (Space Invaders und Adventure) gemacht, und space invaders sehr simpel etwas renderen lassen, damit wir den "Spielaufstart" testen könnten.

08.11.2024
- [X] Alex: komische graue Linie bei Main start fixen (kann Stunden brauchen, oder Minuten)
- [x] Alex: Space invaders: runGame
- [X] Space Invaders: allyUFO class + shooting (keine Ahnung wie das funktioniert, daher kein konkretes AP)
- [x] Neo: Pong (class slider)
- [x] Pong: Fensterrand Collision
- [x] Pong: Pralleffect
- [x] Pong: class ball

☝️ Vergessen Sie nicht, bis einen ersten Code auf github hochzuladen

...

15.11.2024
- [x] Neo: Fix VS file properties to have misc files in compile mode (das braucht der ganze Morgen lange)
- [x] Neo: Doku SPainter
- [ ] Alex: Implement Music
- [ ] Alex: Death Screen
- [ ] Alex: Difficulty Increases
- [ ] (Alex): Start Coding New Game

22.11.2024
- [x] Alex: Formen fallen und Rotieren
- [x] Alex: Formen erkennen reihe
- [x] Alex: Formen löschen sich
- [x] Alex: Formen fallen nach löschung runter
- [x] Neo: hoffen, dass etwas funktioniert
- [x] Neo: Pong bis zu paddle und ball fertig

Alex hat heute das ganze Tetris gemacht, es ist spielbar und man kann sterben. Das Tetris hat 7 Formen, welche rotiert werden können. 
Neo hat heute endlich die Arbeitspakete von letzte Wochen geschafft, und ein fast fertiges single player Pong gemacht. Wir haben auch eine bessere Dokumentation für der SmartPainter erstellt, damit andere die vielleicht ein Game darauf machen wollen nicht so viel schmerz und träne haben.
Neo hat ein bisschen später die erste Version von Pong fertig gemacht. Es hat noch ein paar Bugs, und der Code ist noch nicht sauber und poliert. 

29.11.20247
- [x] Neo: Pong Hitbox
- [x] Neo: Game over screen
- [x] Neo: StartUp Anim Program
- [x] Neo: StartUpAnim textures (flag, text, logo)
- [X] Alex: Tetris music track thread
- [x] Alex: Add more animations
- [x] Alex: Refine game-hub
- [x] Alex: SPainter update & patches (nicht commmitted)

Neo: Heute habe ich Pong vollständig funktionierend gemacht. Ich habe Hitboxes hinzugefügt und einen Game-Over-Screen erstellt. Dann habe ich die Start-Up-Animation erstellt und sie in mein Branch im Game-Format (wie die anderen, Pong und Tetris) zum Testen hochgeladen, damit man beim Starten des Programms nicht immer sechs Sekunden warten muss. Ich habe die Texturen für die Animationen erstellt und auch ein bisschen getestet.

Alex: Ich habe heute all meine Ziele erreicht und noch mehr gemacht, indem ich den SmartPainter mit mehreren neuen Funktionen und einigen Patches aktualisiert habe. Ich habe auch angefangen, "Cursors zu readen". Ich habe auch Tetris ein Audiotrack gegeben auf ein separater Thread, und alle Probleme damit gefixt. Ich habe das Game Hub (aktuelles Start screen) auf Audio gegeben und leicht verbessert.

06.11.202
- [x] Neo: "Stupid" constructor for paddle
- [x] Neo: Modular constructors (textures, left righ boolean, ball size, field size)
- [ ] Neo: StartUpAnim Animation function: red snow to square
- [X] Alex: Cursor Reader testprogramm
- [X] Alex: Cursor Reader im SPainter setzen
- [X] Alex: Cursor Reader im DevPaint implementieren
- [x] Alex: Cursor-Reader & Key-Reader in eigene Klasse
- [ ] Alex: Alle Probleme beheben, die dadurch entstehen

Neo: Heute habe ich das Pong Programm schöner gemacht indem ich constructors mit alle nötige Parameter programmiert habe, und ich habe das Programm für unseres Programmierprojekt verbessert. (Ich werde noch heute regex für email inputs machen)

Alex: Heute habe ich das Cursor Tracking fertiggemacht, und noch mehrere andere Inputs für das Graphic engine gemacht. Ich habe die readkeys und das Cursortracking in ein neues Programm namens SReader (SmartReader) gemacht und auch noch der middle Mouse Button und Scroll Wheen zum Input gemacht. Danach habe ich schnell ein Parcour game gemacht, wo man mit ein Jetpack von Plattform zu Plattform fliegen kann.

13.11.2024
- [x] Neo: dynamic hitbox variables (auto adjusting, pong)
- [x] Alex: Parkour (Jetpack Joyride x Amongus)


Heute haben wir die zwei GitHub Branches vollständig gemerged, meherere kleine Bugs gefixt und eine neue Dokumentation erstellt. Es hat lange gebraucht, um zu entscheiden was zu behalten und was nicht, und dann ancher die nötige packages wieder zu installieren, weil die irgendwie weg waren. Auch müssten die Programme, die noch für die alten Programme (outdated) geschrieben waren, umschreiben. 

20.11.2024
- [x] Neo: Startup Animation vollständig fertig
- [ ] Startup statt manuell aktivieren sofort am Anfang laufen lassen
- [x] Pong: add ESC after dead
- [x] Alex: Krankenhaus überleben (that feeling when knee surgery is tomorrow)

Zusammenfassung 20.12.2024
Heute habe ich das Pong eine escape Funktion gegeben, der Code bereinigt und daas StartUp animation verbessert. Wie im GIF unten sieht man die Funktion, die ich zum StartUp animation hinzugefügt habe. Es vergrössert ein Punk bis zu Schweizerflagge. Ich habe dann das StartUp animation statt als game manuell aktivierbar so implementiert, dass es sofort beim StartUp lauft.

## Reflexion
Ich schreibe diese Reflexion über sowohl Alex als auch mich, obwohl ich nicht Alex bin. Wir haben viel über dieses Projekt geredet, und daher kann ich hier auch effektiv über seine Erfahrungen schreiben.

Wir haben durch dieses Projekt sehr viel gelernt. Obwohl es offensichtlich einen großen Unterschied in unseren Programmierkenntnissen gibt, konnte es uns beide herausfordern. Ich habe vor allem viel über objektorientiertes Programmieren gelernt. Ich war bereits mit Objekten und Funktionen vertraut, hatte sie aber nicht vollständig verstanden. Jetzt weiß ich, wofür Konstruktoren sind, wie man Parameter über mehrere Objektinstanzen hinweg übergibt und wie man Programme sinnvoll in Klassen und Dateien unterteilt. Außerdem habe ich gelernt, wie man eine gute Dateistruktur für ein solches Projekt aufsetzt.

Für Alex war dieses Programm ebenfalls eine gute Lernmöglichkeit. Er konnte sich immer weiter herausfordern, indem er jedes Mal neue Features hinzufügte, wie Multithreading, Sound, Cursor-Tracking usw. Er hat sich auch in C# verbessert, da er jetzt viele neue Methoden kennengelernt hat und sie nicht mehr selbst programmieren muss.

Um das Cursor-Tracking zu implementieren, musste er sehr komplizierte Dinge tun, wie den Kernel zu importieren und dann Berechnungen durchzuführen, die bestimmen, wo sich der Cursor relativ zum offenen Fenster befindet.

Insgesamt hat dieses Programm uns beide herausgefordert und auf ein höheres Niveau gebracht.


## REVISIT: 22.08.2025
Die Arbeit am Graphic renderer geht weiter! Dieses mal machen wir multiplayer games, welche über Trello (als backend🥀🤡) online gespielt werden können. Es funktioniert so, indem wir mit dem Trello-API in Trello Karten erstellen, welche alle Daten für das Game speichern. Wir können es so dann auch abrufen. Alex hat schon Das SPainter dafür vorbereitet, und für Neo kann die Arbeit am Schach Frontend beginnen.

29.08.2025
- [x] GameStart Funktion. (Wenn im SPainter die Start Funktion abgerufen wird dann werden alle UID's und Objekte erstellt. 
- [ ] Schachbrett Texture. (Um zu testen und zu entwicklen brauchen wir dieses ganz am Anfang. Wähle ein passendes Farbschema, damit die Stücken sichtbar sind auf ihre eigene Felder. ZB: Stücken sind S/W, Brett ist Grün/Khaki)
- [ ] Schachstück Texturen. (Es sollten alle vorhanden sein)

! Kleine Änderung, da das DevPaint im Moment broken ist !

- [x] Board wird mit Stücken initialisiert. (Breite das "Piece.cs" aus indem man eine liste oder so von allen möglichen Stücken macht, und initialiser diese in die richtige positionen im Chessboard. Das wäre das Attribut Boardstate im Game object (GameState.cs) Mach für das Initalisieren auch eine Funktion im GameState
- [x] mach (mit GPT warscheinlich) ein Player turn change (also wenn W move gemacht hat, dann geht es zu B)
- [ ] Hintergrund Textboxen implementieren
- [X] Trello API requests planen

# Zusammenfassung 29.08.2025
Ich habe ein StartGame und die wichtigsten Classes jetzt erstellt. Da ich mit Multiplayer games noch ganz unerfahren bin wusste ich trotz Diagramm nicht genau wie ich mein program gestalten sollte, ich glaube ZB dass ein "Room list" hinzugefügt werden muss für Matchmaking (Dann kann ein anderer Spieler welche SPainter verwendet ein Room auswählen. 

![WhatsApp Bild 2025-08-29 um 08.48.36_ec8926f8.jpg](https://github.com/HerrDextro/Lern-Periode-7/blob/745ec94678e17bdef7bf1f3a8afb1f44c6ee0a3d/WhatsApp%20Bild%202025-08-29%20um%2008.48.36_ec8926f8.jpg)

Ich konnte heute den Hintergrund Texttracer implementieren. Er ist noch nicht komplett fertig jedoch kann man schon grundsätzlichen text eingeben. Ein nächster schritt ist es die fehlenden Tasten hinzuzufügen um alle Keys abzurufen können.

![WhatsApp Bild 2025-08-29 um 10 32 13_31622181](https://github.com/user-attachments/assets/f7e5882e-e80b-450a-95d8-fe9d7456364e)

- [X] Alex: Alle Zeichen können eingetippt werden
- [X] Alex: SPainter hat rgb farben
- [ ] Alex: Dev Paint hat text fähigkeiten
- [X] Alex: PAP zu API request erstellen

# Zusammenfassung 05.09.2025
Heute konnte ich nur wenige meiner ursprünglichen Ziele erreichen. Ich konnte jedoch das Problem lösen, sodass ich jetzt jeden Textcharakter auf meiner Tastatur eingeben kann. Aufgrund einer Idee von Vincent wurde ich jedoch abgelenkt und habe RGB-Farben in SPainter implementiert. Zudem fand ich heraus, dass ich bei der Entwicklung von DevPaint einfach irgendetwas falsch gemacht habe. Ich habe mich entschieden, eine neue Malapplikation zu entwickeln, die ich hoffentlich unter der Woche aus Spaß fertigstellen werde. Sonst ist auch kein Problem, aber ich werde vermutlich nächste Woche mit der Umsetzung des Trello-Backends anfangen, da ich jetzt alle dafür nötigen Features habe.

![WhatsApp Bild 2025-09-05 um 08 31 17_558a7b8c](https://github.com/user-attachments/assets/99a5b925-d9bf-4970-be47-73c68e26e1ed)

- [X] Alex: Trello Board Erstellen
- [X] Akex: Nötige requests manuell mit Postman planen
- [ ] Alex: Create game UI teil erstellt
- [X] Alex: Create game erstellt game als karte

# Zusammenfassung 12.09.2025
(Neo) Heute habe ich das ganze Chess logic gemacht in ein Konsolenprogramm. Ich habe für das Legal move checking und sonstige Chess mechanics Gera.Chess verwendet, um es zu vereinfachen. Auch habe ich die Chess Klassen vereinfacht damit sie lesbarer sind und es einfacher ist damit zu arbeiten.
(Alex) Ich konnte heute mit der Umsetzung der StartGame Funktion anfangen. Hierfür habe API requests von C# aus gemacht. Zudem musste ich noch einige bugs in Hyperpaint fixen, was jetzt aber funktioniert.

17.09.2025
- [ ] Neo: UI Schachboard mit JSON als texture statt csb (wie bei Devpaint)
- [ ] Neo: Änderung der Piece Klasse mit texture
- [ ] Alex: UI für Join/Create game implementieren
- [ ] Alex: Piece nicht mit OwnerID sondern farbe speichern
- [ ] Alex: Create Game wartet auf 2. Spieler
- [ ] Alex: Umsetzung des hauptteils nach PAP beginnen


# Zusammenfassung 19.09.2025
(Alex) Ich konnte heute einen unerwarteten Bug reparieren der rot nicht rot machte. Zudem konnte ich ein UI für das Join/Create game erstellen. Danach konnte ich machen, dass man einem Spiel von einem 2. Gerät beitreten kann.
(Neo) Ich habe heute die Renderchess Methode das Board rendern lassen, und auch das Programm teilweise umgeändert damit es GERA.chess verwenden kann. Ich habe die ParseFen Methode angefangen, aber dieses ist super kompliziert und ich möchte es nicht mit GPT machen. Auch habe ich eine Klasse erstellt für baordObj, damit es die erlaubte moves und sonstige möglichkeiten speichern kann.


 - [X] Alex: Main Game Loop holt spielstand ab
 - [ ] Alex: Neos funktion wird ausgeführt
 - [X] Alex: Spielstand wird auf Trello gepusht
 - [x] Neo: FenParsing to boardObj method must be finished
 - [x] Neo: pass boardObj.boardState to RenderChess
 - [x] Neo: Stateless MakeMove method


https://github.com/user-attachments/assets/f2387d79-01f0-47c7-a019-e1ce9c8e30e7


# Zusammenfassung 17.10.2025
(Neo) Ich habe heute das RenderChess() fertig gemacht und eine Methode erstellt die es erlaubt Squares vom Chessboard mit Maus zu selektieren. 
(Alex) Heute konnte ich nicht so viel machen, da Neo noch nicht mit seinem Teil fertig war. Jeoch konnte ich in der Zwischenzeit seit dem letzten mal noch einiges Neues implementieren. Über die Ferien schrieb ich eine komplett neue Version von SPainter umd weil mir langeweilig war implementierte ich Kreise mit AntiAliasing heute.
