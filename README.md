# NeuralNetCUBE
TLDR: [Ausprobieren und selbst spielen!](http://www-stud.uni-due.de/~scjokepp/NeuralNetCUBE/) oder [Anschauen, dass es möglich ist!](https://raw.githubusercontent.com/Banana-Blade/NeuralNetCUBE/master/SuccessVideo.mp4)
---
### Projekt für das Modul "Neuronale Netze" im WS18/19 geschrieben in C# mit Unity.

Das zugrunde liegende Spiel basiert auf einem Online-Tutorial von Brackeys, die benutzten Herleitungen und Formeln auf dem Text "Wie Künstliche Neuronale Netze lernen: Ein Blick in die Black Box der Backpropagation Netzwerke" von Prof. Dr. Walter Oberhofer und Dipl.-Kfm. Thomas Zimmerer von der Universität Regensburg.

***

*Interessante Skripte/Files für das Neuronale Netz:*
  * [NeuralNetwork.cs](NeuralNetCUBEProject/Assets/Scripts/NeuralNetwork.cs): Ein dreischichtiges Neuronales Netz mit Feedforward, Backpropagation und Möglichkeit auf Batchlernen - zum tiefgreifenderen Verständnis der Grundlagen mittels einfacher Matrizenrechnung anhand der Formeln der Uni Regensburg (Biases von mir ergänzt) "von Hand" geschrieben, d.h. ohne Nutzung von Bibliotheken wie tensorflow 
  * [Matrix.cs](NeuralNetCUBEProject/Assets/Scripts/Matrix.cs): Für typische Matrizenoperatoren sowie für die Aktivierungsfunktion samt Ableitung
  * [PlayerMovement.cs](NeuralNetCUBEProject/Assets/Scripts/PlayerMovement.cs): Zur Generierung von Daten während der Nutzer spielt - in jedem Frame werden gespeichert: Position des Spielers (links/rechts), Geschwindigkeit des Spielers (in Richtung links/rechts), Entfernung zur nächsten Reihe von Hinternissen, An-/Abwesenheit bzw. Position der nächsten Hindernisse, Tastendruck des Nutzers (links/rechts)
  * [NNMovements.cs](NeuralNetCUBEProject/Assets/Scripts/NNMovement.cs): Zum Erfassen der Situation zur Laufzeit und Entscheidungsfindung durch Feedforward
  * [EquilibrationScreenManager.cs](NeuralNetCUBEProject/Assets/Scripts/EquilibrationScreenManager.cs): Zum Ausführen der Äquilibration (hier zufallsbasiertes Löschen bzw. Duplizieren)
  * [TrainingScreenManager.cs](NeuralNetCUBEProject/Assets/Scripts/TrainingScreenManager.cs): Zum Starten des Trainings, Berechnen des Fehlers und zur Evaluation der Ergebnisse anhand der Trainingsdaten samt nützlichem Ladebalken ;-)
  
***

Am Laptop oder Desktop PC kann man das Projekt [hier](http://www-stud.uni-due.de/~scjokepp/NeuralNetCUBE/) ausprobieren. (Leider ist es nicht für Smartphones oder Tablets geeignet, da darauf die Darstellung im Browser zur Zeit nicht von Unity via WebGL unterstützt wird.)

***

#### Vorsicht: Es folgen Spoiler!

Wenn das Netz nicht zu lernen scheint (gerade bei Overfitting auf einer einzelnen Klasse, während alle anderen nicht berücksichtigt werden), kann man mit dem kleinen Button "Reset NN" in der oberen rechten Ecke die Gewichte wieder auf zufällige Werte im anfangs definierten Bereich setzen. So muss man nicht sämtliche Daten von Neuem generieren, falls man sich in einem lokalen Minimum "verfängt". Wie mir beim Schreiben dieses Textes auffällt, hätte ich hier eventuell ein Menü einbauen sollen, sodass man auch die anderen festen Parameter des Netzes verändern und sofort ein neues Netz generieren kann (was aber dem widersprochen hätte, dass ich eventuell weitere optionale Inputs später hinzufügen wollte, die die Daten dann ohnehin unbrauchbar gemacht hätten).

##### Tricks:
- Anzahl verdeckter Neuronen klein halten für schnelleres Training (dafür evtl. begrenztes Modell?)
- viele Daten generieren (mind. 3 Durchläufe bis 100% spielen)
- Daten äquilibrieren vor dem Training, durch hochschieben von "links" und "rechts" auf Höhe von "kein Button"
- viele Epochen bei kleiner Batch-Größe (nicht 1) trainieren
- Lernrate klein starten und sukzessive sogar noch senken

---

#### Beweise, dass es möglich ist, und andere Videos/Bilder:
- [Spiel gewonnen!](https://raw.githubusercontent.com/Banana-Blade/NeuralNetCUBE/master/SuccessVideo.mp4)
- [Rechtsverkehr gelehrt](https://raw.githubusercontent.com/Banana-Blade/NeuralNetCUBE/master/Right-hand%20driving.mp4)
- [Menü-Durchlauf und Pendeln](https://raw.githubusercontent.com/Banana-Blade/NeuralNetCUBE/master/WalkthroughVideoWithPendulum.mp4)
- [Heureka - Screenshot der Unity Umgebung, als mein Netz anfing, vernünftig zu lernen](https://raw.githubusercontent.com/Banana-Blade/NeuralNetCUBE/master/ScreenshotUnity.jpg)

---

#### Eventuell bei Zeiten noch implementieren:
- weitere optionale Inputs (oder!)
- "Reset NN" führt zum Menü zur Erstellung des Neuronalen Netzes ohne Verlust der Daten
