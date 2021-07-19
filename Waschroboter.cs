using System;
using System.IO;

using Beleg2021;
namespace Beleg2021
{
    public class Waschroboter : Roboter
    {   
        /// <summary>Variablen zur Bestimmung der benötigten Waschgänge</summary>
        private bool _Vorreinigen, _Hauptreinigen, _Hochdruckreinigen, _Nachreinigen;

        /// <summary>
        /// Konstruktor des MaschinenModuls.
        /// </summary>
        /// <param name="id">
        /// Die ID des MaschinenModuls.
        /// </param>
        public Waschroboter(string id)
        {
            this._ID = id;
            this._Vorreinigen = false;
            this._Hauptreinigen = false;
            this._Hochdruckreinigen = false;
            this._Nachreinigen = false;
        }

        /// <summary>
        /// Übernimmt ein ZKG. Gibt true zurück falls das ZKG erfolgreich übernommen wurde, false sonst.
        /// </summary>
        /// <param name="zkg">
        /// Das zu übernehmende ZKG.
        /// </param>
        public override bool UebernimmZKG(ZKG zkg)
        {
            if (GetStatus() == Status.BEREIT && zkg != null)
            {
                this.LagereEin(zkg);
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " wird von " + this.GetID() + " gewaschen.");
                return true;
            }
            else 
            {
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " konnte von " + this.GetID() + " nicht übernommen werden!");
                return false;
            }
        }

        /// <summary>
        /// Übergibt das aktuelle ZKG. Gibt null zurück, falls kein ZKG vorhanden ist.
        /// </summary>
        public override ZKG UebergibZKG()
        {
            if (GetStatus() == Status.BELEGT && _AktuellesTeil != null)
            {
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " wurde von " + this.GetID() + " abgelegt.");
                return this.LagereAus();
            }
            else
            {
                Console.WriteLine("Das MaschinenModul " + this.GetID() + " hat kein ZKG!");
                return null;
            }
        }

        /// <summary>
        /// Liest RFID chip des übergebenen ZKGs und setzt entsprechend die benötigten Waschgänge.
        /// </summary>
        protected override void LiesRFIDvonZKG(ZKG zkg)
        {
            bool[] bs = zkg.GetRFIDchip();

            if (bs[0] && bs[1] && bs[2] && bs[3])
            {
                _Vorreinigen = _Hauptreinigen = true;
            }
            if (bs[4] && bs[5] && bs[7])
            {
                _Nachreinigen = true;
            }
            if (bs[6])
            {
                _Hochdruckreinigen = true;
            }
        }

        /// <summary>
        /// Hauptprozess des Waschroboters, welcher ein zu reinigendes ZKG von einem belegten Ablageplatz nimmt und,
        /// entsprechend der Arbeitsschritthistorie des ZKGs, in verschiedenen Waschgängen in der Wascheinrichtung reinigt.
        /// </summary>
        protected override void RoboterHauptprozess()
        {
            Ablageplatz ablageplatz1 = (Ablageplatz) AbstrakteAnlagensteuerung.GetMaschinenModulByName("Ablageplatz1");
            Ablageplatz ablageplatz2 = (Ablageplatz) AbstrakteAnlagensteuerung.GetMaschinenModulByName("Ablageplatz2");

            // Nimm zu reinigendes ZKG von Ablageplatz. Falls keines vorhanden beende.
            if (!IstBereit(ablageplatz1) && !ablageplatz1.GetReinigungszustand())
            {
                this.UebernimmZKG(ablageplatz1.UebergibZKG());
            } else if (!IstBereit(ablageplatz2) && !ablageplatz2.GetReinigungszustand()) {
                this.UebernimmZKG(ablageplatz2.UebergibZKG());
            } else {
                return;
            }

            // RFID des ZKG auslesen.
            this.LiesRFIDvonZKG(this._AktuellesTeil);

            // Nötige Waschgänge durchführen.
            Wascheinrichtung wascheinrichtung = (Wascheinrichtung) AbstrakteAnlagensteuerung.GetMaschinenModulByName("Wascheinrichtung");
            wascheinrichtung.UebernimmZKG(this._AktuellesTeil);
            if (this._Vorreinigen)
            {
                wascheinrichtung.Vorreinigen();
            }
            if (this._Hauptreinigen)
            {
                wascheinrichtung.Hauptreinigen();
            }
            if (this._Hochdruckreinigen)
            {
                wascheinrichtung.Hochdruckreinigen();
            }
            if (this._Nachreinigen)
            {
                wascheinrichtung.Nachreinigen();
            }

            wascheinrichtung.UebergibZKG();
            this._Vorreinigen = this._Hauptreinigen = this._Hochdruckreinigen = this._Nachreinigen = false;

            // ZKG auf freiem Ablageplatz ablegen und dessen Reinigungszustand setzen.
            if (IstBereit(ablageplatz1))
            {
                ablageplatz1.UebernimmZKG(this.UebergibZKG());
                ablageplatz1.SetReinigungszustand(true);
            } else if (IstBereit(ablageplatz2)) {
                ablageplatz2.UebernimmZKG(this.UebergibZKG());
                ablageplatz2.SetReinigungszustand(true);
            }
        }

        
        /// <summary>
        /// Gibt zurück, ob übergebenes MaschinenModul bereit ist.
        /// </summary>
        private bool IstBereit(MaschinenModul mm)
        {
            return mm.GetStatus() == Status.BEREIT;
        }
    }
}