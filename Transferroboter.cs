using System;
using System.IO;

using Beleg2021;
namespace Beleg2021
{
    public class Transferroboter : Roboter
    {
        /// <summary>Variable zur Bestimmung ob aktuelles ZKG ein Testteil ist.</summary>
        private bool _TestTeil;
        
        /// <summary>
        /// Konstruktor des MaschinenModuls.
        /// </summary>
        /// <param name="id">
        /// Die ID des MaschinenModuls.
        /// </param>
        public Transferroboter(string id)
        {
            this._ID = id;
            this._TestTeil = false;
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
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " wurde von " + this.GetID() + " aufgenommen.");
                return true;
            }
            else 
            {
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " konnte von " + this.GetID() + " nicht aufgenommen werden!");
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
        /// Ließt RFID chip des übergebenen ZKGs und setzt entsprechend _TestTeil.
        /// </summary>
        protected override void LiesRFIDvonZKG(ZKG zkg)
        {
            bool[] bs = zkg.GetRFIDchip();

            this._TestTeil = bs[0] && bs[1] && !bs[2] && !bs[3] && !bs[4] && !bs[5] && !bs[6] && !bs[7];
        }

        /// <summary>
        /// Hauptprozess des Transferroboters, welcher für den Transfer von ZKGs zwischen TPein, 
        /// Ablageplätzen und TPaus zuständig ist
        /// </summary>
        protected override void RoboterHauptprozess()
        {
            Waschroboter waschroboter = (Waschroboter) AbstrakteAnlagensteuerung.GetMaschinenModulByName("Waschroboter");
            Ablageplatz ablageplatz1 = (Ablageplatz) AbstrakteAnlagensteuerung.GetMaschinenModulByName("Ablageplatz1");
            Ablageplatz ablageplatz2 = (Ablageplatz) AbstrakteAnlagensteuerung.GetMaschinenModulByName("Ablageplatz2");
            TPein tpein = (TPein) AbstrakteAnlagensteuerung.GetMaschinenModulByName("TPein");
            TPaus tpaus = (TPaus) AbstrakteAnlagensteuerung.GetMaschinenModulByName("TPaus");

            // Überprüfe ob einer der Ablageplätze frei gemacht werden kann, lege entsprechendes ZKG auf TPaus ab.
            if (!IstBereit(ablageplatz1) && ablageplatz1.GetReinigungszustand())
            {
                this.UebernimmZKG(ablageplatz1.UebergibZKG());
                this._AktuellesTeil.SetRFIDchip();
                tpaus.UebernimmZKG(this.UebergibZKG());
            }
            if (!IstBereit(ablageplatz2) && ablageplatz2.GetReinigungszustand())
            {
                this.UebernimmZKG(ablageplatz2.UebergibZKG());
                this._AktuellesTeil.SetRFIDchip();
                tpaus.UebernimmZKG(this.UebergibZKG());
            }

            // Falls waschroboter aktiv ist und einer der beiden Ablageplätze belegt ist kann nichts weiter gemacht werden.
            if (!IstBereit(waschroboter) && (!IstBereit(ablageplatz1) || !IstBereit(ablageplatz2))) 
            {
                return;
            }

            // Falls ZKG auf tpein bereit liegt und ein Ablageplatz frei ist, nimm ein ZKG auf.
            if (!IstBereit(tpein) && (IstBereit(ablageplatz1) || IstBereit(ablageplatz2)))
            {
                this.UebernimmZKG(tpein.UebergibZKG());
                this.LiesRFIDvonZKG(this._AktuellesTeil);
                // Falls aktuelles ZKG ein Testteil ist, lege auf TPaus ab, ansonsten transferiere ZKG auf Ablageplatz.
                if (this._TestTeil)
                {
                    Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " ist ein Testteil. Lege auf TPaus ab...");
                    tpaus.UebernimmZKG(this.UebergibZKG());
                } else {
                    if (IstBereit(ablageplatz1))
                    {
                        ablageplatz1.UebernimmZKG(this.UebergibZKG());
                        ablageplatz1.SetReinigungszustand(false);
                    } else {
                        ablageplatz2.UebernimmZKG(this.UebergibZKG());
                        ablageplatz2.SetReinigungszustand(false);
                    }
                }
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