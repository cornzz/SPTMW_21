using System;
using System.IO;
//using System.Linq;
using System.Collections.Generic;
using Beleg2021;
namespace Beleg2021
{
    public class Anlagensteuerung : AbstrakteAnlagensteuerung
    {
        /// <summary>
        /// Der von uns vorgegebene Initialisierer für die ZKGs.
        /// Nutzen Sie diesen gerne als Vorlage für die Implementierung des Module Initialisierer.
        /// </summary>
        /// <param name="Pfad">Pfad zur entsprechenden CSV Datei. Der String muss wie folgt formatiert sein: 
        /// @"../../../ZKG.csv", 
        /// das @ ist dabei notwendig! die .. erlauben einen relativen Verweis auf die .csv so dass diese vorhanden ist, 
        /// egal in welchem Ordner sie das Projekt ablegen. Den Projektordner sollten Sie auf keinen Fall in seiner Struktur ändern!
        /// </param>
        private void InitialisiereZKGs(String Pfad)
        {
            //So oder so aehnlich koennen sie Ihren Code annotieren, damit der Verlauf der Aufrufe auf der Console sichtbar wird.
            Console.WriteLine("InitialisiereZKGs betreten");
            // Der Pfad zur angegebenen Datei ZKG.csv ../../../ bedeutet drei Verzeichnisse nach oben, also in das Hauptverzeichnis ihres Projektes.
            StreamReader reader = new StreamReader(Pfad); // @"../../../ZKG.csv"
            // Liest zeilenweise bis zum Ende der Datei
            while (!reader.EndOfStream)
            {
                // speichert die aktuelle Zeile auf als String line zwischen
                string line = reader.ReadLine();
                // nimmt die Zeile in line und teilt sie an jedem Vorkommen von ;   _values ist ein Array von String
                string[] values = line.Split(';');
                // Das erste [0] Element des Arrays ist die linke Spalte der CSV-Datei, also die ID des ZKG
                string ZKG_ID = values[0];
                // erzeugt einen leeren Array um die Bearbeitungsschritte aufnehmen zu können.
                bool[] rfid = new bool[9];
                // Füllt die ersten 8 (nur die sind in der CSV-Datei gegeben) Elemente des Arrays mit den Werten aus der CSV 
                for (int i = 0; i < 8; i++)
                {
                    rfid[i] = values[i + 1] == "true" ? true : false;
                }
                // Hier wird schließlich die Instanz des ZKG erzeugt und an die List _ZKGObjektVerwaltung angefügt
                _ZKGObjektVerwaltung.Add(new ZKG(ZKG_ID, rfid));
            }
            Console.WriteLine("InitialisiereZKGs verlassen");
        }

        /// <summary>
        /// Hier implementieren Sie ähnlich zur Methode drüber den Parser für die Maschienenmodule. 
        /// Ähnlich heißt explizit nicht, dass Sie den Code von oben 1:1 kopieren können, das wird nicht funktionieren!
        /// </summary>
        /// <param name="Pfad">
        /// Pfad zur entsprechenden CSV Datei. Der String muss wie folgt formatiert sein: 
        /// @"../../../Module.csv", 
        /// das @ ist dabei notwendig! die .. erlauben einen relativen Verweis auf die .csv so das diese vorhanden ist, 
        /// egal in welchem Ordner sie das Projekt ablegen. Den Projektordner sollten Sie auf keinen Fall in seiner Struktur ändern!
        /// </param>
        private void InitialisiereMaschinenModule(String pfad)
        {
            Console.WriteLine("InitialisiereMaschinenModule betreten");
            StreamReader reader = new StreamReader(pfad);
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(';');
                string klasse = values[0];
                string name = values[1];
                string id = values[2];

                Type type = Type.GetType("Beleg2021." + klasse, true);
                MaschinenModul modul = (MaschinenModul) Activator.CreateInstance(type, id);
                _MaschinenModulObjektVerwaltung.Add(name, modul);
            }

            Console.WriteLine("InitialisiereMaschinenModule verlassen");
        }

        /// <summary>
        /// Verschiebt ein ZKG aus der ZKGObjectVerwaltung nach TPein, falls TPein nicht belegt ist.
        /// </summary>
        /// <param name="ZKGObjektVerwaltung">
        /// Die ZKGObjektVerwaltung.
        /// </param>
        public override void VerschiebeZKGausZKGObjektVerwaltungNachTPEIN(List<ZKG> ZKGObjektVerwaltung)
        {
            TPein tpein = (TPein) GetMaschinenModulByName("TPein");
            if (tpein.GetStatus() == Status.BELEGT) {
                // Console.WriteLine("TPein ist belegt, es wurde kein ZKG verschoben!");
            } else if (ZKGObjektVerwaltung.Count == 0) {
                Console.WriteLine("Keine ZKGs in der ZKG Objekt Verwaltung übrig!");
            } else {
                ZKG zkg = ZKGObjektVerwaltung[0];
                Console.WriteLine("Das ZKG " + zkg.GetID() + " wird auf TPein verschoben.");
                if (tpein.UebernimmZKG(zkg))
                {
                    ZKGObjektVerwaltung.RemoveAt(0);
                }
            }
		}
	   
	    /// <summary>
        /// Initialisiert die Anlage.
        /// </summary>
        public Anlagensteuerung()
        {
            string pfad = @"../../../";
            InitialisiereZKGs(pfad + "ZKG.csv");
            InitialisiereMaschinenModule(pfad + "Module.csv");
        }
    }
}

