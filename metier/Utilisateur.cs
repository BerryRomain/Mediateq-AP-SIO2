using MySql.Data.MySqlClient;
using System;

namespace Mediateq_AP_SIO2
{
    class Utilisateur
    {
        // Champs représentant les propriétés de l'utilisateur
        private string login;
        private string nom;
        private string prenom;
        private string serviceLibelle; // Utilisation d'une classe Service pour la référence
        private string motDePasseHache;

        // Constructeur de la classe Utilisateur
        public Utilisateur(string unLogin, string unNom, string unPrenom, string unServiceLibelle, string unMotDePasseHache)
        {
            login = unLogin;
            nom = unNom;
            prenom = unPrenom;
            serviceLibelle = unServiceLibelle;
            motDePasseHache = unMotDePasseHache;
        }

        // Propriétés pour accéder et modifier les champs privés
        public string Login { get => login; set => login = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string ServiceLibelle { get => serviceLibelle; set => serviceLibelle = value; }
        public string MotDePasseHache { get => motDePasseHache; set => motDePasseHache = value; }

        // Méthode pour authentifier un utilisateur
        public static Utilisateur AuthentifierUtilisateur(string login, string motDePasse)
        {
            Utilisateur utilisateur = null;
            MySqlConnection connexion = DAOFactory.creerConnection(); // Crée une connexion à la base de données

            try
            {
                DAOFactory.connecter(); // Ouvre la connexion à la base de données

                // Requête SQL pour récupérer les données de l'utilisateur et du service associé
                string query = "SELECT login, nom, prenom, mot_de_passe, service.libelle " +
                               "FROM Utilisateur " +
                               "JOIN service ON service.id = utilisateur.service " +
                               "WHERE login = '" + login + "' AND mot_de_passe = '" + motDePasse + "'";

                // Exécute la requête SQL pour récupérer les données de l'utilisateur
                using (MySqlDataReader reader = DAOFactory.execSQLRead(query))
                {
                    if (reader.Read())
                    {
                        // Récupère les données de l'utilisateur et du service associé
                        string serviceLibelle = reader["libelle"].ToString();

                        // Crée une instance de Utilisateur avec les données récupérées
                        utilisateur = new Utilisateur(
                            reader["login"].ToString(),
                            reader["nom"].ToString(),
                            reader["prenom"].ToString(),
                            serviceLibelle,
                            reader["mot_de_passe"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'authentification : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter(); // Ferme la connexion à la base de données
            }

            return utilisateur; // Retourne l'utilisateur authentifié ou null s'il n'existe pas
        }
    }
}
