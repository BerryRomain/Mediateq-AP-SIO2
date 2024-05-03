using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente un descripteur associé à un document.
    /// </summary>
    class Descripteur
    {
        // Champs privés représentant l'identifiant et le libellé du descripteur
        private string id;
        private string libelle;

        /// <summary>
        /// Constructeur de la classe Descripteur.
        /// </summary>
        /// <param name="id">L'identifiant du descripteur.</param>
        /// <param name="libelle">Le libellé du descripteur.</param>
        public Descripteur(string id, string libelle)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            this.id = id;
            this.libelle = libelle;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string Id { get => id; set => id = value; }
        public string Libelle { get => libelle; set => libelle = value; }
    }
}
