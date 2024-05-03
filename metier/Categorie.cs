namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente une catégorie de documents.
    /// </summary>
    class Categorie
    {
        // Champs privés représentant l'identifiant et le libellé de la catégorie
        private string id;
        private string libelle;

        /// <summary>
        /// Constructeur de la classe Categorie.
        /// </summary>
        /// <param name="id">L'identifiant de la catégorie.</param>
        /// <param name="libelle">Le libellé de la catégorie.</param>
        public Categorie(string id, string libelle)
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
