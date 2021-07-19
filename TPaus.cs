using System;
using System.IO;

using Beleg2021;
namespace Beleg2021
{
    public class TPaus : MaschinenModul
    {
        /// <summary>
        /// Konstruktor des MaschinenModuls.
        /// </summary>
        /// <param name="id">
        /// Die ID des MaschinenModuls.
        /// </param>
        public TPaus(string id)
        {
            this._ID = id;
        }

        /// <summary>
        /// Übernimmt ein ZKG, gibt dessen Bearbeitungszustände aus und übergibt es sofort wieder.
        /// </summary>
        /// <param name="zkg">
        /// Das zu übernehmende ZKG.
        /// </param>
        public override bool UebernimmZKG(ZKG zkg)
        {
            if (GetStatus() == Status.BEREIT && zkg != null)
            {
                this.LagereEin(zkg);
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " ist auf " + this.GetID() + " angekommen und kann abgeholt werden.");
                Console.WriteLine("Bearbeitungszustände von " + this._AktuellesTeil.GetID() + ": " + String.Join(", ", this._AktuellesTeil.GetRFIDchip()));
                this.UebergibZKG();
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
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " wurde von " + this.GetID() + " abgeholt.");
                return this.LagereAus();
            }
            else
            {
                Console.WriteLine("Das MaschinenModul " + this.GetID() + " hat kein ZKG!");
                return null;
            }
        }
    }
}