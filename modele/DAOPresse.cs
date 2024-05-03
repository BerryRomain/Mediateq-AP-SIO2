using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Mediateq_AP_SIO2;

namespace Mediateq_AP_SIO2
{
    class DAOPresse
    {
        // Méthode pour récupérer toutes les revues
        public static List<Revue> getAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select * from revue";

            DAOFactory.connecter(); // Connexion à la base de données

            MySqlDataReader reader = DAOFactory.execSQLRead(req); // Exécution de la requête SQL de lecture

            // Parcours des résultats de la requête
            while (reader.Read())
            {
                // Création d'une instance de Revue avec les données récupérées
                Revue revue = new Revue(
                    reader[0].ToString(), // ID de la revue
                    reader[1].ToString(), // Titre de la revue
                    char.Parse(reader[2].ToString()), // Empruntable
                    reader[3].ToString(), // Périodicité
                    DateTime.Parse(reader[5].ToString()), // Date de fin d'abonnement
                    int.Parse(reader[4].ToString()), // Délai de mise à disposition
                    reader[6].ToString() // ID du descripteur
                );

                // Ajout de la revue à la liste des revues
                lesRevues.Add(revue);
            }

            DAOFactory.deconnecter(); // Déconnexion de la base de données

            return lesRevues; // Retourne la liste des revues
        }

        // Méthode pour récupérer les parutions d'une revue spécifique
        public static List<Parution> getParutionByTitre(Revue pTitre)
        {
            List<Parution> lesParutions = new List<Parution>();
            string req = "Select * from parution where idRevue = " + pTitre.Id;

            DAOFactory.connecter(); // Connexion à la base de données

            MySqlDataReader reader = DAOFactory.execSQLRead(req); // Exécution de la requête SQL de lecture

            // Parcours des résultats de la requête
            while (reader.Read())
            {
                // Création d'une instance de Parution avec les données récupérées
                Parution parution = new Parution(
                    int.Parse(reader[1].ToString()), // Numéro de la parution
                    DateTime.Parse(reader[2].ToString()), // Date de parution
                    reader[3].ToString(), // Photo
                    pTitre.Id // ID de la revue associée à la parution
                );

                // Ajout de la parution à la liste des parutions
                lesParutions.Add(parution);
            }

            DAOFactory.deconnecter(); // Déconnexion de la base de données

            return lesParutions; // Retourne la liste des parutions
        }
    }
}
