using Google.Protobuf;
using Mediateq_AP_SIO2;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Collections.Specialized.BitVector32;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Représente une connexion utilisateur avec les informations nécessaires pour l'authentification.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Nom de l'utilisateur.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur.
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur.
        /// </summary>
        public string MotDePasse { get; set; }

        /// <summary>
        /// Libellé du service associé à l'utilisateur.
        /// </summary>
        public string ServiceLibelle { get; set; }

        /// <summary>
        /// Constructeur de la classe Connection.
        /// </summary>
        /// <param name="nom">Nom de l'utilisateur.</param>
        /// <param name="prenom">Prénom de l'utilisateur.</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur.</param>
        /// <param name="serviceLibelle">Libellé du service associé à l'utilisateur.</param>
        public Connection(string nom, string prenom, string motDePasse, string serviceLibelle)
        {
            Nom = nom;
            Prenom = prenom;
            MotDePasse = motDePasse;
            ServiceLibelle = serviceLibelle;
        }

        /// <summary>
        /// Authentifie un utilisateur en vérifiant son login et son mot de passe dans la base de données.
        /// </summary>
        /// <param name="login">Login de l'utilisateur à authentifier.</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur à authentifier.</param>
        /// <returns>Un objet Connection si l'authentification réussit, sinon null.</returns>
        public static Connection AuthentifierUtilisateur(string login, string motDePasse)
        {
            MySqlConnection connexion = null;

            try
            {
                // Connexion à la base de données si elle n'est pas déjà ouverte
                if (connexion == null || connexion.State != ConnectionState.Open)
                {
                    DAOFactory.connecter();
                    connexion = DAOFactory.GetConnexion();
                }

                // Requête SQL pour récupérer les informations de l'utilisateur
                string query = "SELECT u.Nom, u.Prenom, u.mot_de_passe, s.Libelle AS ServiceLibelle " +
                               "FROM Utilisateur u " +
                               "INNER JOIN Service s ON u.service = s.Id " +
                               "WHERE u.Login = @Login AND u.mot_de_passe = @MotDePasse";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@MotDePasse", motDePasse);

                    // Exécution de la requête SQL et récupération du résultat
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Vérification si l'utilisateur a été trouvé
                        if (reader.Read())
                        {
                            // Récupération des informations de l'utilisateur
                            string nom = reader["Nom"].ToString();
                            string prenom = reader["Prenom"].ToString();
                            string serviceLibelle = reader["ServiceLibelle"].ToString();

                            // Création et retour de l'objet Connection avec les informations récupérées
                            return new Connection(nom, prenom, motDePasse, serviceLibelle);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                // Déconnexion de la base de données si elle a été ouverte dans cette méthode
                if (connexion != null && connexion.State == ConnectionState.Open)
                {
                    DAOFactory.deconnecter();
                }
            }

            // Si l'utilisateur n'a pas été trouvé ou si l'authentification a échoué, retournez null
            return null;
        }
    }
}
