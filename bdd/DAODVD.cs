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
    /// Classe fournissant des méthodes pour interagir avec la table "DVD" de la base de données.
    /// </summary>
    public class DAODVD
    {
        /// <summary>
        /// Insère un nouveau DVD dans la base de données.
        /// </summary>
        /// <param name="dvd">L'objet DVD à insérer.</param>
        /// <param name="idPublic">L'ID du public associé au DVD.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        internal static void InsererDVD(DVD dvd, int idPublic, MySqlConnection connexion)
        {
            try
            {
                DAOFactory.connecter();

                // Requête SQL pour insérer un nouveau DVD
                string query = "INSERT INTO Document(id, titre, image, commandeEnCours, idPublic) " +
                               "VALUES(@DocumentId, @Titre, @Image, 0, @IdPublic);" +
                               "INSERT INTO DVD(idDocument, synopsis, realisateur, duree) " +
                               "VALUES(@DocumentId, @Synopsis, @Realisateur, @Duree);";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    if (connexion != null && connexion.State == ConnectionState.Open)
                    {
                        // Paramètres d'insertion
                        command.Parameters.AddWithValue("@DocumentId", dvd.IdDoc);
                        command.Parameters.AddWithValue("@Titre", dvd.Titre);
                        command.Parameters.AddWithValue("@Image", dvd.Image);
                        command.Parameters.AddWithValue("@IdPublic", idPublic);
                        command.Parameters.AddWithValue("@Synopsis", dvd.Synopsis);
                        command.Parameters.AddWithValue("@Realisateur", dvd.Realisateur);
                        command.Parameters.AddWithValue("@Duree", dvd.Duree);

                        // Exécuter la requête d'insertion
                        command.ExecuteNonQuery();

                        // Afficher un message de succès
                        MessageBox.Show("Le DVD a été créé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Console.WriteLine("La connexion n'est pas correctement initialisée ou fermée.");
                    }
                }
            }
            catch (Exception e)
            {
                // Afficher le message d'erreur
                MessageBox.Show("Une erreur s'est produite lors de la création du DVD : " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }

        /// <summary>
        /// Supprime un DVD de la base de données.
        /// </summary>
        /// <param name="idDocument">L'ID du document à supprimer.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        internal static void SupprimerDVD(string idDocument, MySqlConnection connexion)
        {
            try
            {
                DAOFactory.connecter();

                // Requête SQL pour supprimer un DVD
                string query = "DELETE FROM DVD WHERE idDocument = @DocumentId; " +
                               "DELETE FROM Document WHERE id = @DocumentId;";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    if (connexion != null && connexion.State == ConnectionState.Open)
                    {
                        command.Parameters.AddWithValue("@DocumentId", idDocument);

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
        /// Vérifie l'existence d'un document dans la base de données.
        /// </summary>
        /// <param name="idDocument">L'ID du document à vérifier.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        /// <returns>True si le document existe, sinon False.</returns>
        public static bool VerifierExistenceDocument(string idDocument, MySqlConnection connexion)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Document WHERE id = @DocumentId";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    command.Parameters.AddWithValue("@DocumentId", idDocument);

                    object result = command.ExecuteScalar();

                    int count = Convert.ToInt32(result);

                    return count > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifie les détails d'un DVD dans la base de données.
        /// </summary>
        /// <param name="idDocument">L'ID du document à modifier.</param>
        /// <param name="titre">Le nouveau titre du DVD.</param>
        /// <param name="image">Le nouveau chemin de l'image du DVD.</param>
        /// <param name="synopsis">Le nouveau synopsis du DVD.</param>
        /// <param name="duree">La nouvelle durée du DVD.</param>
        /// <param name="realisateur">Le nouveau réalisateur du DVD.</param>
        /// <param name="idPublic">Le nouvel ID du public associé au DVD.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        /// <returns>Le nombre de lignes affectées par la modification.</returns>
        public static int ModifierDVD(string idDocument, string titre, string image, string synopsis, string duree, string realisateur, int idPublic, MySqlConnection connexion)
        {
            try
            {
                string query = "UPDATE document INNER JOIN dvd ON document.id = dvd.idDocument " +
                               "SET document.titre = @Titre, document.image = @Image, dvd.synopsis = @Synopsis, " +
                               "dvd.duree = @Duree, dvd.realisateur = @Realisateur, document.idPublic = @IdPublic " +
                               "WHERE dvd.idDocument = @IdDocument";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    command.Parameters.AddWithValue("@IdDocument", idDocument);
                    command.Parameters.AddWithValue("@Titre", titre);
                    command.Parameters.AddWithValue("@Image", image);
                    command.Parameters.AddWithValue("@Synopsis", synopsis);
                    command.Parameters.AddWithValue("@Duree", duree);
                    command.Parameters.AddWithValue("@Realisateur", realisateur);
                    command.Parameters.AddWithValue("@IdPublic", idPublic);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la modification du DVD : " + ex.Message);
                throw;
            }
        }
    }
}
