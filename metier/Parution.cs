using System;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente une parution d'une revue.
    /// </summary>
    class Parution
    {
        // Champs privés représentant le numéro de la parution, sa date, la photo associée et l'identifiant de la revue
        private int numero;
        private DateTime dateParution;
        private string photo;
        private string idRevue;

        /// <summary>
        /// Constructeur de la classe Parution.
        /// </summary>
        /// <param name="numero">Le numéro de la parution.</param>
        /// <param name="dateParution">La date de parution de la revue.</param>
        /// <param name="photo">Le chemin de la photo associée à la parution.</param>
        /// <param name="idRevue">L'identifiant de la revue.</param>
        public Parution(int numero, DateTime dateParution, string photo, string idRevue)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            this.numero = numero;
            this.dateParution = dateParution;
            this.photo = photo;
            this.idRevue = idRevue;
        }

        // Propriétés pour accéder et modifier les champs privés
        public int Numero { get => numero; set => numero = value; }
        public DateTime DateParution { get => dateParution; set => dateParution = value; }
        public string Photo { get => photo; set => photo = value; }
        public string IdRevue { get => idRevue; set => idRevue = value; }
    }
}
