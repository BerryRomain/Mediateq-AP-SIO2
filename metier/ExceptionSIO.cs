using System;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente une exception spécifique à l'application SIO.
    /// </summary>
    class ExceptionSIO : Exception
    {
        // Champ privé représentant le niveau de l'exception
        private int niveauExc;
        // Champ privé représentant le libellé de l'exception
        private string libelleExc;

        /// <summary>
        /// Constructeur de la classe ExceptionSIO.
        /// </summary>
        /// <param name="pNiveau">Le niveau de l'exception.</param>
        /// <param name="pLibelle">Le libellé de l'exception.</param>
        /// <param name="pMessage">Le message d'erreur de l'exception.</param>
        public ExceptionSIO(int pNiveau, string pLibelle, string pMessage) : base(pMessage)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            niveauExc = pNiveau;
            libelleExc = pLibelle;
        }

        // Propriétés pour accéder et modifier les champs privés
        public int NiveauExc { get => niveauExc; set => niveauExc = value; }
        public string LibelleExc { get => libelleExc; set => libelleExc = value; }
    }
}
