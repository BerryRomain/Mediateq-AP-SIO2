namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente un DVD dans la médiathèque.
    /// </summary>
    class DVD : Document
    {
        // Champs privés représentant le synopsis, le réalisateur et la durée du DVD
        private string synopsis;
        private string realisateur;
        private string duree;

        /// <summary>
        /// Constructeur de la classe DVD.
        /// </summary>
        /// <param name="unId">L'identifiant du DVD.</param>
        /// <param name="unTitre">Le titre du DVD.</param>
        /// <param name="unSynopsis">Le synopsis du DVD.</param>
        /// <param name="unRealisateur">Le réalisateur du DVD.</param>
        /// <param name="uneDuree">La durée du DVD.</param>
        /// <param name="uneImage">Le chemin vers l'image associée au DVD.</param>
        public DVD(string unId, string unTitre, string unSynopsis, string unRealisateur, string uneDuree, string uneImage) : base(unId, unTitre, uneImage)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            Synopsis = unSynopsis;
            Realisateur = unRealisateur;
            Duree = uneDuree;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string Synopsis { get => synopsis; set => synopsis = value; }
        public string Realisateur { get => realisateur; set => realisateur = value; }
        public string Duree { get => duree; set => duree = value; }
    }
}
