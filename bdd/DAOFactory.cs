using System;
using MySql.Data.MySqlClient;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe fournissant des méthodes pour interagir avec la base de données.
    /// </summary>
    public class DAOFactory
    {
        private static MySqlConnection connexion;

        /// <summary>
        /// Crée et retourne une instance de connexion à la base de données.
        /// </summary>
        /// <returns>Une instance de MySqlConnection.</returns>
        public static MySqlConnection creerConnection()
        {
            string serverIp = "localhost";
            string username = "root";
            string password = "";
            string databaseName = "mediateq";

            string dbConnectionString = string.Format("server={0};uid={1};pwd={2};database={3};", serverIp, username, password, databaseName);

            connexion = null;

            try
            {
                connexion = new MySqlConnection(dbConnectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur connexion BDD", e.Message);
            }

            return connexion;
        }

        /// <summary>
        /// Établit une connexion à la base de données.
        /// </summary>
        public static void connecter()
        {
            try
            {
                connexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Ferme la connexion à la base de données.
        /// </summary>
        public static void deconnecter()
        {
            connexion.Close();
        }

        /// <summary>
        /// Exécute une requête de lecture et retourne un Datareader.
        /// </summary>
        /// <param name="requete">La requête SQL à exécuter.</param>
        /// <param name="parameters">Les paramètres de la requête.</param>
        /// <returns>Un Datareader contenant les résultats de la requête.</returns>
        public static MySqlDataReader execSQLRead(string requete, params object[] parameters)
        {
            MySqlCommand command;
            MySqlDataAdapter adapter;
            command = new MySqlCommand();
            command.CommandText = requete;
            command.Connection = connexion;

            // Ajouter les paramètres à la commande
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue($"@Param{i}", parameters[i]);
            }

            adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;

            MySqlDataReader dataReader;
            dataReader = command.ExecuteReader();

            return dataReader;
        }

        /// <summary>
        /// Exécute une requête d'écriture (Insert ou Update).
        /// </summary>
        /// <param name="requete">La requête SQL à exécuter.</param>
        public static void execSQLWrite(string requete)
        {
            MySqlCommand command;
            command = new MySqlCommand();
            command.CommandText = requete;
            command.Connection = connexion;

            try
            {
                connecter(); // Ouvrir la connexion avant d'exécuter la commande
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                deconnecter(); // Fermer la connexion après l'exécution de la commande
            }
        }

        /// <summary>
        /// Obtient l'instance actuelle de connexion à la base de données.
        /// </summary>
        /// <returns>L'instance actuelle de MySqlConnection.</returns>
        public static MySqlConnection GetConnexion()
        {
            return connexion;
        }
    }
}
