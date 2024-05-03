namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente un document de la médiathèque.
    /// </summary>
    class Document
    {
        // Champs privés représentant l'identifiant, le titre, l'image et la catégorie du document
        private string idDoc;
        private string titre;
        private string image;
        private string laCategorie;

        /// <summary>
        /// Constructeur de la classe Document.
        /// </summary>
        /// <param name="unId">L'identifiant du document.</param>
        /// <param name="unTitre">Le titre du document.</param>
        /// <param name="uneImage">Le chemin vers l'image associée au document.</param>
        public Document(string unId, string unTitre, string uneImage)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            idDoc = unId;
            titre = unTitre;
            image = uneImage;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string IdDoc { get => idDoc; set => idDoc = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Image { get => image; set => image = value; }
        public string LaCategorie { get => laCategorie; set => laCategorie = value; }
    }
}
