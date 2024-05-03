using System;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente une revue dans le système de gestion de la médiathèque.
    /// </summary>
    public class Revue
    {
        // Champs privés représentant les attributs d'une revue
        private string id;
        private string titre;
        private char empruntable;
        private string periodicite;
        private DateTime dateFinAbonnement;
        private int delaiMiseADispo;
        private string idDescripteur;

        /// <summary>
        /// Constructeur de la classe Revue.
        /// </summary>
        /// <param name="id">L'identifiant de la revue.</param>
        /// <param name="titre">Le titre de la revue.</param>
        /// <param name="empruntable">Indique si la revue est empruntable ou non.</param>
        /// <param name="periodicite">La fréquence de parution de la revue.</param>
        /// <param name="dateFinAbonnement">La date de fin d'abonnement à la revue.</param>
        /// <param name="delaiMiseADispo">Le délai de mise à disposition de la revue.</param>
        /// <param name="idDescripteur">L'identifiant du descripteur associé à la revue.</param>
        public Revue(string id, string titre, char empruntable, string periodicite, DateTime dateFinAbonnement, int delaiMiseADispo, string idDescripteur)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            this.id = id;
            this.titre = titre;
            this.empruntable = empruntable;
            this.periodicite = periodicite;
            this.dateFinAbonnement = dateFinAbonnement;
            this.delaiMiseADispo = delaiMiseADispo;
            this.idDescripteur = idDescripteur;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public char Empruntable { get => empruntable; set => empruntable = value; }
        public string Periodicite { get => periodicite; set => periodicite = value; }
        public DateTime DateFinAbonnement { get => dateFinAbonnement; set => dateFinAbonnement = value; }
        public int DelaiMiseADispo { get => delaiMiseADispo; set => delaiMiseADispo = value; }
        public string IdDescripteur { get => idDescripteur; set => idDescripteur = value; }
    }
}
