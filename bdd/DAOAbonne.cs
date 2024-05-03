using Mediateq_AP_SIO2;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe fournissant des méthodes pour interagir avec la table "Abonne" de la base de données.
    /// </summary>
    public class DAOAbonne
    {
        /// <summary>
        /// Insère un nouvel abonné dans la base de données.
        /// </summary>
        /// <param name="abonne">L'objet Abonne à insérer.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        public static void InsererAbonne(Abonne abonne, MySqlConnection connexion)
        {
            try
            {
                DAOFactory.connecter();

                // Requête SQL pour insérer un nouvel abonné
                string query = "INSERT INTO Abonne (nom, prenom, adresse, telephone, adresse_mail, mot_de_passe, date_de_naissance, date_debut_abonnement, date_fin_abonnement) " +
                               "VALUES (@Nom, @Prenom, @Adresse, @Telephone, @AdresseMail, @MotDePasse, @DateNaissance, @DateDebutAbonnement, @DateFinAbonnement)";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    if (connexion != null && connexion.State == ConnectionState.Open)
                    {
                        // Paramètres de la commande SQL
                        command.Parameters.AddWithValue("@Nom", abonne.Nom);
                        command.Parameters.AddWithValue("@Prenom", abonne.Prenom);
                        command.Parameters.AddWithValue("@Adresse", abonne.Adresse);
                        command.Parameters.AddWithValue("@Telephone", abonne.Telephone);
                        command.Parameters.AddWithValue("@AdresseMail", abonne.AdresseMail);
                        command.Parameters.AddWithValue("@MotDePasse", abonne.MotDePasseHache);
                        command.Parameters.AddWithValue("@DateNaissance", abonne.DateNaissance);
                        command.Parameters.AddWithValue("@DateDebutAbonnement", abonne.DateDebutAbonnement);
                        command.Parameters.AddWithValue("@DateFinAbonnement", abonne.DateFinAbonnement);

                        // Exécute la commande SQL
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        // Gérer le cas où la connexion est nulle ou fermée
                        Console.WriteLine("La connexion n'est pas correctement initialisée ou fermée.");
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
                DAOFactory.deconnecter();
            }
        }

        /// <summary>
        /// Récupère tous les abonnés de la base de données.
        /// </summary>
        /// <returns>Une liste d'objets Abonne.</returns>
        public static List<Abonne> GetTousLesAbonnes()
        {
            List<Abonne> lesAbonnes = new List<Abonne>();
            try
            {
                DAOFactory.connecter();

                // Requête SQL pour sélectionner tous les abonnés
                string query = "SELECT * FROM Abonne";

                MySqlDataReader reader = DAOFactory.execSQLRead(query);

                while (reader.Read())
                {
                    Abonne abonne = new Abonne(
                        reader["nom"].ToString(),
                        reader["prenom"].ToString(),
                        reader["adresse"].ToString(),
                        reader["telephone"].ToString(),
                        reader["adresse_mail"].ToString(),
                        reader["mot_de_passe"].ToString(),
                        Convert.ToDateTime(reader["date_de_naissance"]),
                        Convert.ToDateTime(reader["date_debut_abonnement"]),
                        Convert.ToDateTime(reader["date_fin_abonnement"])
                    );

                    // Ajoute l'objet Abonne à la liste
                    lesAbonnes.Add(abonne);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la récupération des abonnés : " + e.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }

            return lesAbonnes;
        }

        /// <summary>
        /// Modifie un abonné dans la base de données.
        /// </summary>
        /// <param name="nom">Le nouveau nom de l'abonné.</param>
        /// <param name="prenom">Le nouveau prénom de l'abonné.</param>
        /// <param name="adresse">La nouvelle adresse de l'abonné.</param>
        /// <param name="telephone">Le nouveau numéro de téléphone de l'abonné.</param>
        /// <param name="adresse_mail">La nouvelle adresse e-mail de l'abonné.</param>
        /// <param name="mot_de_passe">Le nouveau mot de passe de l'abonné.</param>
        /// <param name="date_de_naissance">La nouvelle date de naissance de l'abonné.</param>
        /// <param name="date_debut_abonnement">La nouvelle date de début d'abonnement de l'abonné.</param>
        /// <param name="date_fin_abonnement">La nouvelle date de fin d'abonnement de l'abonné.</param>
        /// <returns>True si la modification a réussi, sinon False.</returns>
        public static Boolean ModifierAbonne(string nom, string prenom, string adresse, string telephone, string adresse_mail, string mot_de_passe, DateTime date_de_naissance, DateTime date_debut_abonnement, DateTime date_fin_abonnement)
        {
            try
            {
                DAOFactory.connecter();

                // Requête SQL pour mettre à jour un abonné
                string updateAbo = "UPDATE Abonne SET nom = '" + nom + "', prenom = '" + prenom + "', adresse = '" + adresse + "', telephone = '" + telephone + "', adresse_mail = '" + adresse_mail + "', mot_de_passe = '" + mot_de_passe + "', date_de_naissance = '" + date_de_naissance.ToString("yyyy-MM-dd") + "', date_debut_abonnement = '" + date_debut_abonnement.ToString("yyyy-MM-dd") + "', date_fin_abonnement = '" + date_fin_abonnement.ToString("yyyy-MM-dd") + "' WHERE adresse_mail = '" + adresse_mail + "'";

                // Exécute la requête de mise à jour
                DAOFactory.execSQLWrite(updateAbo);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }
    }
}






