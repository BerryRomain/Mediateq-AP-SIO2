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
    /// Classe fournissant des méthodes pour interagir avec la table "Livre" de la base de données.
    /// </summary>
    public class DAOLivre
    {
        /// <summary>
        /// Insère un nouveau livre dans la base de données.
        /// </summary>
        /// <param name="livre">L'objet Livre à insérer.</param>
        /// <param name="idPublic">L'ID du public associé au livre.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        internal static void InsererLivre(Livre livre, int idPublic, MySqlConnection connexion)
        {
            try
            {
                DAOFactory.connecter();

                // Vérifier si l'ID du document existe déjà
                string checkQuery = "SELECT COUNT(*) FROM Document WHERE id = @DocumentId";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connexion))
                {
                    checkCommand.Parameters.AddWithValue("@DocumentId", livre.IdDoc);
                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        MessageBox.Show("Un document avec cet ID existe déjà.", "Erreur d'insertion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Sortir de la méthode sans effectuer l'insertion
                    }
                }

                // Si l'ID n'existe pas déjà, procéder à l'insertion
                string query = "INSERT INTO Document(id, titre, image, commandeEnCours, idPublic) " +
                               "VALUES(@DocumentId, @Titre, @Image, 0, @IdPublic);" +
                               "INSERT INTO Livre(idDocument, ISBN, auteur, collection) " +
                               "VALUES(@DocumentId, @ISBN, @Auteur, @Collection);";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    // Paramètres d'insertion
                    command.Parameters.AddWithValue("@DocumentId", livre.IdDoc);
                    command.Parameters.AddWithValue("@ISBN", livre.ISBN1);
                    command.Parameters.AddWithValue("@Auteur", livre.Auteur);
                    command.Parameters.AddWithValue("@Collection", livre.LaCollection);
                    command.Parameters.AddWithValue("@Titre", livre.Titre);
                    command.Parameters.AddWithValue("@Image", livre.Image);
                    command.Parameters.AddWithValue("@IdPublic", idPublic);

                    // Exécuter la requête d'insertion
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Si des lignes ont été affectées, afficher le message de succès
                        MessageBox.Show("Le livre a été créé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Si aucune ligne n'a été affectée, afficher un message d'erreur
                        MessageBox.Show("Une erreur s'est produite lors de la création du livre.", "Erreur d'insertion", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Supprime un livre de la base de données.
        /// </summary>
        /// <param name="idDocument">L'ID du document à supprimer.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        internal static void SupprimerLivre(string idDocument, MySqlConnection connexion)
        {
            try
            {
                DAOFactory.connecter();

                string query = "DELETE FROM Livre WHERE idDocument = @DocumentId; " +
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
        /// Modifie les détails d'un livre dans la base de données.
        /// </summary>
        /// <param name="idDocument">L'ID du document à modifier.</param>
        /// <param name="titre">Le nouveau titre du livre.</param>
        /// <param name="image">Le nouveau lien de l'image du livre.</param>
        /// <param name="auteur">Le nouvel auteur du livre.</param>
        /// <param name="isbn">Le nouvel ISBN du livre.</param>
        /// <param name="collection">La nouvelle collection du livre.</param>
        /// <param name="idPublic">L'ID du public associé au livre.</param>
        /// <param name="connexion">La connexion à la base de données.</param>
        /// <returns>Le nombre de lignes affectées par la requête de modification.</returns>
        public static int ModifierLivre(string idDocument, string titre, string image, string auteur, string isbn, string collection, int idPublic, MySqlConnection connexion)
        {
            try
            {
                string query = "UPDATE document INNER JOIN livre ON document.id = livre.idDocument " +
                       "SET document.titre = @Titre, document.image = @Image, livre.auteur = @Auteur, " +
                       "livre.ISBN = @ISBN, livre.collection = @collection, document.idPublic = @IdPublic " +
                       "WHERE livre.idDocument = @IdDocument";

                using (MySqlCommand command = new MySqlCommand(query, connexion))
                {
                    command.Parameters.AddWithValue("@IdDocument", idDocument);
                    command.Parameters.AddWithValue("@Titre", titre);
                    command.Parameters.AddWithValue("@Image", image);
                    command.Parameters.AddWithValue("@Auteur", auteur);
                    command.Parameters.AddWithValue("@ISBN", isbn);
                    command.Parameters.AddWithValue("@collection", collection);
                    command.Parameters.AddWithValue("@IdPublic", idPublic);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la modification du livre : " + ex.Message);
                throw;
            }
        }
    }
}
