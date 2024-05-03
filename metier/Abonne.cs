using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente un abonné à la médiathèque.
    /// </summary>
    public class Abonne
    {
        // Champs privés représentant les informations de l'abonné
        private string nom;
        private string prenom;
        private string adresse;
        private string telephone;
        private string adresse_mail;
        private string mot_de_passe;
        private DateTime date_de_naissance;
        private DateTime date_debut_abonnement;
        private DateTime date_fin_abonnement;

        /// <summary>
        /// Constructeur de la classe Abonne.
        /// </summary>
        /// <param name="unNom">Le nom de l'abonné.</param>
        /// <param name="unPrenom">Le prénom de l'abonné.</param>
        /// <param name="uneAdresse">L'adresse de l'abonné.</param>
        /// <param name="unTelephone">Le numéro de téléphone de l'abonné.</param>
        /// <param name="uneAdresseMail">L'adresse e-mail de l'abonné.</param>
        /// <param name="unMotDePasseHache">Le mot de passe hashé de l'abonné.</param>
        /// <param name="uneDateNaissance">La date de naissance de l'abonné.</param>
        /// <param name="uneDateDebutAbonnement">La date de début d'abonnement de l'abonné.</param>
        /// <param name="uneDateFinAbonnement">La date de fin d'abonnement de l'abonné.</param>
        public Abonne(string unNom, string unPrenom, string uneAdresse, string unTelephone, string uneAdresseMail, string unMotDePasseHache, DateTime uneDateNaissance, DateTime uneDateDebutAbonnement, DateTime uneDateFinAbonnement)
        {
            // Initialisation des champs avec les valeurs passées en paramètres
            nom = unNom;
            prenom = unPrenom;
            adresse = uneAdresse;
            telephone = unTelephone;
            adresse_mail = uneAdresseMail;
            mot_de_passe = unMotDePasseHache;
            date_de_naissance = uneDateNaissance;
            date_debut_abonnement = uneDateDebutAbonnement;
            date_fin_abonnement = uneDateFinAbonnement;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Adresse { get => adresse; set => adresse = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public string AdresseMail { get => adresse_mail; set => adresse_mail = value; }
        public string MotDePasseHache { get => mot_de_passe; set => mot_de_passe = value; }
        public DateTime DateNaissance { get => date_de_naissance; set => date_de_naissance = value; }
        public DateTime DateDebutAbonnement { get => date_debut_abonnement; set => date_debut_abonnement = value; }
        public DateTime DateFinAbonnement { get => date_fin_abonnement; set => date_fin_abonnement = value; }
    }

    /// <summary>
    /// Gère la collection des abonnés à la médiathèque.
    /// </summary>
    public class GestionAbonnes
    {
        // Collection d'abonnés
        private List<Abonne> lesAbonnes = new List<Abonne>();

        /// <summary>
        /// Ajoute une liste d'abonnés à la collection.
        /// </summary>
        /// <param name="nouveauxAbonnes">La liste des nouveaux abonnés à ajouter.</param>
        public void AjouterAbonnes(List<Abonne> nouveauxAbonnes)
        {
            lesAbonnes.AddRange(nouveauxAbonnes);
        }

        /// <summary>
        /// Récupère la liste des abonnés.
        /// </summary>
        /// <returns>La liste des abonnés.</returns>
        public List<Abonne> GetLesAbonnes()
        {
            return lesAbonnes;
        }
    }
}
