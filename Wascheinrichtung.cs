using System;
using System.IO;
using System.Threading;

using Beleg2021;
namespace Beleg2021
{
    public class Wascheinrichtung : MaschinenModul
    {
        /// <summary>
        /// Konstruktor des MaschinenModuls.
        /// </summary>
        /// <param name="id">
        /// Die ID des MaschinenModuls.
        /// </param>
        public Wascheinrichtung(string id)
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
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " wird in " + this.GetID() + " gewaschen.");
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
                Console.WriteLine("Das ZKG " + this._AktuellesTeil.GetID() + " wurde aus " + this.GetID() + " entnommen.");
                return this.LagereAus();
            }
            else
            {
                Console.WriteLine("Das MaschinenModul " + this.GetID() + " hat kein ZKG!");
                return null;
            }
        }

        /// <summary>
        /// Führt die Vorreinigung aus.
        /// </summary>
        public void Vorreinigen()
        {
            Console.WriteLine("Vorreinigung des ZKG " + this._AktuellesTeil.GetID() + " in " + this.GetID() + "...");
            Thread.Sleep(5000);
        }

        
        /// <summary>
        /// Führt die Hauptreinigung aus.
        /// </summary>
        public void Hauptreinigen()
        {
            Console.WriteLine("Hauptreinigung des ZKG " + this._AktuellesTeil.GetID() + " in " + this.GetID() + "...");
            Thread.Sleep(10000);
        }

        
        /// <summary>
        /// Führt die Hochdruckreinigung aus.
        /// </summary>
        public void Hochdruckreinigen()
        {
            Console.WriteLine("Hochdruckreinigung des ZKG " + this._AktuellesTeil.GetID() + " in " + this.GetID() + "...");
            Thread.Sleep(20000);
        }

        
        /// <summary>
        /// Führt die Nachreinigung aus.
        /// </summary>
        public void Nachreinigen()
        {
            Console.WriteLine("Nachreinigung des ZKG " + this._AktuellesTeil.GetID() + " in " + this.GetID() + "...");
            Thread.Sleep(10000);
        }
    }
}