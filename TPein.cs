using System;
using System.IO;

using Beleg2021;
namespace Beleg2021
{
    public class TPein : MaschinenModul
    {
        /// <summary>
        /// Konstruktor des MaschinenModuls.
        /// </summary>
        /// <param name="id">
        /// Die ID des MaschinenModuls.
        /// </param>
        public TPein(string id)
        {
            this._ID = id;
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
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " ist auf " + this.GetID() + " angekommen und kann abgeholt werden.");
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