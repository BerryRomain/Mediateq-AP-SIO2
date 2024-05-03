namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente un livre dans la médiathèque.
    /// </summary>
    class Livre : Document
    {
        // Champs privés représentant l'ISBN, l'auteur et la collection du livre
        private string ISBN;
        private string auteur;
        private string laCollection;

        /// <summary>
        /// Constructeur de la classe Livre.
        /// </summary>
        /// <param name="unId">L'identifiant du livre.</param>
        /// <param name="unTitre">Le titre du livre.</param>
        /// <param name="unISBN">L'ISBN du livre.</param>
        /// <param name="unAuteur">L'auteur du livre.</param>
        /// <param name="uneCollection">La collection du livre.</param>
        /// <param name="uneImage">Le chemin vers l'image associée au livre.</param>
        public Livre(string unId, string unTitre, string unISBN, string unAuteur, string uneCollection, string uneImage) : base(unId, unTitre, uneImage)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            ISBN1 = unISBN;
            Auteur = unAuteur;
            LaCollection = uneCollection;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string ISBN1 { get => ISBN; set => ISBN = value; }
        public string Auteur { get => auteur; set => auteur = value; }
        public string LaCollection { get => laCollection; set => laCollection = value; }
    }
}
