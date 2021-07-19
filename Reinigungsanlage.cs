using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Beleg2021
{
    /// <summary>
    /// 
    /// WICHTIG: Fuer Sie gibt es hier nichts zu tun!
    /// 
    /// Die Klasse Reinigungsanlage ist der Einstiegspunkt in Ihre Simulation einer Reinigungsanlage.
    ///  
    /// Die Hauptaufgabe dieser Klasse bzw. des Objektes dieser Klasse ist es, Ihren Konstruktor der 
    /// Anlagensteuerung aufzurufen und so Ihre Implementierung zu starten. Weiterhin werden hier die 
    /// Zeitsteuerungen der Roboter und der ZKG-Verschiebeprozesses vom Speicher auf TPein aufgerufen.
    /// </summary>
    public class Reinigungsanlage
    {

        private String _Name = "REINIGUNGSANLAGE Beleg2021";
        /// <summary>
        /// Rein informative Funktion.
        /// </summary>
        /// <returns>Den Namen IHRER Reinigungsanlage. Er wird von uns vorgegeben. Dieser Name ist fuer Sie nur informativ.</returns>
        public String GetName()
        {
            return _Name;
        }
        /// <summary>
        /// WICHTIG: Fuer Sie gibt es hier nichts zu tun!
        /// Hier wird ihr Code gestartet. 
        /// </summary>
        /// <param name="args">Keine</param>
        private static void Main(string[] args)
        {
            //Hier wird der Code aufgerufen den Sie als Student geschrieben haben.

            //WICHTIG: Fuer Sie gibt es hier nichts zu tun!
            //Sie sollten/duerfen die folgende Zeile nicht aendern. 
            try
            {
                if (_internal.SafeToCallMMConstructor(Assembly.GetExecutingAssembly())) _internal.StartStudentCode(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ihr Code ist noch zu unvollst�ndig oder es sind noch Ausnahmen enthalten, um sinnvoll gestartet zu werden.");
                Console.WriteLine("Die Nachricht der Exception war: " + ex.GetBaseException().GetType().ToString() + " --> " + ex.GetBaseException().Message);
            }
            finally { Console.ReadKey(); }
            //Hier passiert nichts, die Zeile sorgt nur daf�r, dass sich die Konsole nicht direkt schlie�t

        }

    }

}
