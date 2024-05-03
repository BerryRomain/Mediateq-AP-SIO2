using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using Mediateq_AP_SIO2;
using Mediateq_AP_SIO2;
using MySql.Data.MySqlClient;

namespace Mediateq_AP_SIO2
{
    public partial class FrmMediateq : Form
    {
        #region Variables globales

        // Stocke une liste de catégories.
        static List<Categorie> lesCategories;

        // Stocke une liste de descripteurs.
        static List<Descripteur> lesDescripteurs;

        // Stocke une liste de revues.
        static List<Revue> lesRevues;

        // Stocke une liste de livres.
        static List<Livre> lesLivres;

        // Stocke une liste de DVD.
        static List<DVD> lesDVDs;

        // Stocke une liste d'abonnés.
        static List<Abonne> lesAbonnes;

        // Représente la connexion à la base de données.
        private MySqlConnection connexion;

        #endregion



        #region Procédures évènementielles

        // Constructeur de la fenêtre FrmMediateq.
        public FrmMediateq()
        {
            InitializeComponent();
        }

        // Chargement de la fenêtre FrmMediateq.
        private void FrmMediateq_Load(object sender, EventArgs e)
        {
            try
            {
                // Création de la connexion avec la base de données.
                DAOFactory.creerConnection();

                // Chargement des objets en mémoire : descripteurs et revues.
                lesDescripteurs = DAODocuments.getAllDescripteurs();
                lesRevues = DAOPresse.getAllRevues();

                // Suppression des onglets non nécessaires.
                tabOngletsApplication.TabPages.Remove(tabParutions);
                tabOngletsApplication.TabPages.Remove(tabTitres);
                tabOngletsApplication.TabPages.Remove(tabLivres);
                tabOngletsApplication.TabPages.Remove(tabDVD);
                tabOngletsApplication.TabPages.Remove(tabAbonnes);
            }
            catch (ExceptionSIO exc)
            {
                // Affichage d'une fenêtre de message en cas d'erreur.
                MessageBox.Show(exc.NiveauExc + " - " + exc.LibelleExc + " - " + exc.Message);
            }
        }

        #endregion

        #region Parutions
        //-----------------------------------------------------------
        // ONGLET "PARUTIONS"
        //------------------------------------------------------------
        // Événement déclenché lors de l'entrée dans l'onglet "Parutions".
        private void tabParutions_Enter(object sender, EventArgs e)
        {
            // Liaison des données de la liste des revues à la ComboBox cbxTitres.
            cbxTitres.DataSource = lesRevues;
            cbxTitres.DisplayMember = "titre";
        }

        // Événement déclenché lors de la sélection d'un élément dans la ComboBox cbxTitres.
        private void cbxTitres_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Liste de parutions correspondant au titre sélectionné.
            List<Parution> lesParutions;

            // Récupération du titre sélectionné dans la ComboBox.
            Revue titreSelectionne = (Revue)cbxTitres.SelectedItem;

            // Récupération de la liste des parutions associées au titre sélectionné.
            lesParutions = DAOPresse.getParutionByTitre(titreSelectionne);

            // Réinitialisation du DataGridView dgvParutions.
            dgvParutions.Rows.Clear();

            // Parcours de la collection des parutions et ajout des données dans le DataGridView.
            foreach (Parution parution in lesParutions)
            {
                dgvParutions.Rows.Add(parution.Numero, parution.DateParution, parution.Photo);
            }
        }

        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "TITRES"
        //------------------------------------------------------------
        // Événement déclenché lors de l'entrée dans l'onglet "Titres".
        private void tabTitres_Enter(object sender, EventArgs e)
        {
            // Liaison des données de la liste des descripteurs à la ComboBox cbxDomaines.
            cbxDomaines.DataSource = lesDescripteurs;
            cbxDomaines.DisplayMember = "libelle";
        }

        // Événement déclenché lors de la sélection d'un élément dans la ComboBox cbxDomaines.
        private void cbxDomaines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Objet Domaine sélectionné dans la ComboBox.
            Descripteur domaineSelectionne = (Descripteur)cbxDomaines.SelectedItem;

            // Réinitialisation du DataGridView dgvTitres.
            dgvTitres.Rows.Clear();

            // Parcours de la collection des revues et ajout des données dans le DataGridView.
            foreach (Revue revue in lesRevues)
            {
                // Vérification si le descripteur de la revue correspond au domaine sélectionné.
                if (revue.IdDescripteur == domaineSelectionne.Id)
                {
                    // Ajout des données de la revue dans le DataGridView.
                    dgvTitres.Rows.Add(revue.Id, revue.Titre, revue.Empruntable, revue.DateFinAbonnement, revue.DelaiMiseADispo);
                }
            }
        }
        #endregion


        #region Livres
        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        // Événement déclenché lors de l'entrée dans l'onglet "Livres".
        private void tabLivres_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire (catégories, descripteurs, livres)
            lesCategories = DAODocuments.getAllCategories();
            lesDescripteurs = DAODocuments.getAllDescripteurs();
            lesLivres = DAODocuments.getAllLivres();
        }

        // Bouton de recherche de livre par numéro de document.
        private void btnRechercher_Click(object sender, EventArgs e)
        {
            // Réinitialisation des labels d'affichage des informations du livre.
            lblNumero.Text = "";
            lblTitre.Text = "";
            lblAuteur.Text = "";
            lblCollection.Text = "";
            lblISBN.Text = "";
            lblImage.Text = "";

            // Recherche du livre correspondant au numéro de document saisi.
            // Si le livre est trouvé, ses informations sont affichées dans les labels.
            // Sinon, affiche un message d'erreur.
            bool trouve = false;
            foreach (Livre livre in lesLivres)
            {
                if (livre.IdDoc == txbNumDoc.Text)
                {
                    lblNumero.Text = livre.IdDoc;
                    lblTitre.Text = livre.Titre;
                    lblAuteur.Text = livre.Auteur;
                    lblCollection.Text = livre.LaCollection;
                    lblISBN.Text = livre.ISBN1;
                    lblImage.Text = livre.Image;
                    trouve = true;
                }
            }
            if (!trouve)
                MessageBox.Show("Document non trouvé dans les livres");
        }

        // Événement déclenché lors de la modification du texte dans le champ de saisie du titre.
        private void txbTitre_TextChanged(object sender, EventArgs e)
        {
            // Réinitialisation du DataGridView dgvLivres.
            dgvLivres.Rows.Clear();

            // Parcours de la liste des livres.
            // Si le titre du livre contient la saisie effectuée, il est ajouté au DataGridView.
            foreach (Livre livre in lesLivres)
            {
                // Conversion des saisies et des titres en minuscules pour une comparaison insensible à la casse.
                string saisieMinuscules = txbTitre.Text.ToLower();
                string titreMinuscules = livre.Titre.ToLower();

                // Vérification si le titre du livre contient la saisie.
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dgvLivres.Rows.Add(livre.IdDoc, livre.Titre, livre.Auteur, livre.ISBN1, livre.LaCollection);
                }
            }
        }

        // Bouton de création d'un nouveau livre.
        private void btn_CreationLivre_Click(object sender, EventArgs e)
        {
            // Récupération des informations saisies pour créer un nouveau livre.
            string idLivres = txtB_creation_IdLivres.Text;
            string titre = txtB_creation_Livre.Text;
            string auteur = txtB_creation_Auteur.Text;
            string codeISBN = txtB_creation_CodeISBN.Text;
            string collection = txtB_creation_Collection.Text;
            string image = txtB_creation_ImageLivres.Text;

            // Récupération du libellé du public sélectionné dans la ComboBox.
            string libellePublic = cbB_choixPublicVise.SelectedItem?.ToString();

            bool erreur = false; // Variable pour suivre si une erreur s'est produite.

            // Vérification de la complétude des champs et de la validité du public sélectionné.
            if (string.IsNullOrEmpty(idLivres) || string.IsNullOrEmpty(titre) || string.IsNullOrEmpty(auteur) ||
                string.IsNullOrEmpty(codeISBN) || string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(image) ||
                string.IsNullOrEmpty(libellePublic))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires ou sélectionner un public valide.", "Champs incomplets",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPublic = cbB_choixPublicVise.SelectedIndex + 1;

            if (idPublic == -1)
            {
                MessageBox.Show("Le public sélectionné n'est pas valide.", "Public invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Création d'une instance de Livre avec les informations saisies.
            Livre nouveauLivre = new Livre(idLivres, titre, codeISBN, auteur, collection, image);

            // Ouverture de la connexion à la base de données.
            using (MySqlConnection connexion = DAOFactory.GetConnexion())
            {
                connexion.Open();

                try
                {
                    // Insertion du nouveau livre dans la base de données.
                    DAOLivre.InsererLivre(nouveauLivre, idPublic, connexion);
                }
                catch (Exception ex)
                {
                    // Affichage du message d'erreur en cas d'échec de l'insertion.
                    MessageBox.Show("Une erreur s'est produite lors de la création du livre : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    erreur = true; // Définition de la variable erreur à true.
                }

                // Si aucune erreur ne s'est produite, affichage d'un message de succès et réinitialisation des champs.
                if (!erreur)
                {
                    MessageBox.Show("Le livre a été créé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Réinitialisation des champs après l'insertion réussie.
                    txtB_creation_IdLivres.Clear();
                    txtB_creation_Livre.Clear();
                    txtB_creation_Auteur.Clear();
                    txtB_creation_CodeISBN.Clear();
                    txtB_creation_Collection.Clear();
                    txtB_creation_ImageLivres.Clear();
                }
            }
        }

        // Bouton de suppression d'un livre.
        private void btn_suppressionLivre_Click(object sender, EventArgs e)
        {
            // Récupération de l'ID du document à supprimer.
            string idDocument = txtB_suppressionLivre.Text;

            // Vérification de la saisie de l'ID.
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Veuillez entrer l'ID du document à supprimer.", "Champ obligatoire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Ouverture de la connexion à la base de données.
                using (MySqlConnection connexion = DAOFactory.GetConnexion())
                {
                    connexion.Open();

                    // Vérification de l'existence du document à supprimer.
                    bool idExiste = DAOLivre.VerifierExistenceDocument(idDocument, connexion);

                    // Si l'ID du document n'existe pas, affichage d'un message d'erreur.
                    if (!idExiste)
                    {
                        MessageBox.Show("L'ID du document n'existe pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtB_suppressionLivre.Clear();
                        return;
                    }

                    // Suppression du livre.
                    DAOLivre.SupprimerLivre(idDocument, connexion);

                    // Affichage d'un message de succès.
                    MessageBox.Show("Le livre a été supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Réinitialisation du champ de saisie de l'ID du document.
                    txtB_suppressionLivre.Clear();
                }
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur en cas d'échec de la suppression.
                MessageBox.Show("Une erreur s'est produite lors de la suppression du livre : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtB_suppressionLivre.Clear();
            }
        }

        // Bouton de recherche d'informations sur un livre.
        private void btn_rechercherLivre_Click(object sender, EventArgs e)
        {
            // Récupération de l'ID du livre à rechercher.
            string idDocument = txtB_modif_IdLivre.Text;

            // Vérification de la saisie de l'ID.
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Veuillez saisir un ID de livre.", "Champ obligatoire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Ouverture de la connexion à la base de données.
                using (MySqlConnection connection = DAOFactory.GetConnexion())
                {
                    connection.Open();

                    // Requête SQL pour récupérer les informations sur le livre.
                    string query = "SELECT document.titre, document.image, livre.auteur, livre.ISBN, livre.collection, public.libelle " +
                                   "FROM livre " +
                                   "JOIN document ON livre.idDocument = document.id " +
                                   "JOIN public ON document.idPublic = public.id " +
                                   "WHERE livre.idDocument = @idDocument";

                    // Exécution de la requête SQL.
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Ajout du paramètre ID du document à la commande SQL.
                        command.Parameters.AddWithValue("@idDocument", idDocument);

                        // Lecture des résultats de la requête.
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // Si des résultats sont trouvés, affichage des informations dans les champs correspondants.
                            // Sinon, affichage d'un message d'information.
                            if (reader.Read())
                            {
                                txtB_modif_Titres.Text = reader["titre"].ToString();
                                txtB_modif_Auteur.Text = reader["auteur"].ToString();
                                txtB_modif_CodeISBN.Text = reader["ISBN"].ToString();
                                txtB_modif_Collection.Text = reader["collection"].ToString();
                                txtB_modif_LienImage.Text = reader["image"].ToString();
                                cbB_modifPublicVise.SelectedItem = reader["libelle"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Aucun enregistrement trouvé pour l'ID de livre spécifié.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur en cas d'échec de la recherche.
                MessageBox.Show("Une erreur s'est produite lors de la recherche du livre : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Bouton de suppression du contenu des champs de recherche de livre.
        private void btn_suppressionContenu_Click(object sender, EventArgs e)
        {
            // Réinitialisation des champs de recherche de livre.
            txtB_modif_IdLivre.Clear();
            txtB_modif_Titres.Clear();
            txtB_modif_Auteur.Clear();
            txtB_modif_CodeISBN.Clear();
            txtB_modif_Collection.Clear();
            txtB_modif_LienImage.Clear();
            cbB_modifPublicVise.SelectedIndex = -1;
        }

        // Bouton de modification d'un livre.
        private void btn_modifLivre_Click(object sender, EventArgs e)
        {
            // Vérification de la complétude des champs de modification.
            if (string.IsNullOrEmpty(txtB_modif_Titres.Text) || string.IsNullOrEmpty(txtB_modif_Auteur.Text) ||
                string.IsNullOrEmpty(txtB_modif_CodeISBN.Text) || string.IsNullOrEmpty(txtB_modif_Collection.Text) ||
                string.IsNullOrEmpty(txtB_modif_LienImage.Text) || cbB_modifPublicVise.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs pour effectuer la modification.", "Champs vides",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupération des informations de modification.
            string idDocument = txtB_modif_IdLivre.Text;
            string titre = txtB_modif_Titres.Text;
            string image = txtB_modif_LienImage.Text;
            string auteur = txtB_modif_Auteur.Text;
            string isbn = txtB_modif_CodeISBN.Text;
            string collection = txtB_modif_Collection.Text;
            int idPublic = cbB_modifPublicVise.SelectedIndex + 1;

            try
            {
                // Ouverture de la connexion à la base de données.
                using (MySqlConnection connexion = DAOFactory.GetConnexion())
                {
                    connexion.Open();

                    // Modification du livre dans la base de données.
                    int rowsAffected = DAOLivre.ModifierLivre(idDocument, titre, image, auteur, isbn, collection, idPublic, connexion);

                    // Affichage d'un message de succès ou d'avertissement selon le nombre de lignes affectées.
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Les modifications ont été appliquées avec succès.", "Modification réussie",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Aucune modification n'a été appliquée. Vérifiez l'ID du document spécifié.",
                                        "Aucune modification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur en cas d'échec de la modification.
                MessageBox.Show("Une erreur s'est produite lors de la modification : " + ex.Message, "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region DVD
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        // Chargement des données dans l'onglet DVD lors de son activation
        private void tabDVD_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire
            lesCategories = DAODocuments.getAllCategories();
            lesDescripteurs = DAODocuments.getAllDescripteurs();
            lesDVDs = DAODocuments.getAllDVD();
        }

        // Recherche d'un DVD par son numéro de document
        private void btnRechercherDVD_Click(object sender, EventArgs e)
        {
            // Réinitialisation des labels
            labelNumeroDVD.Text = "";
            labelTitreDVD.Text = "";
            labelSynopsisDVD.Text = "";
            labelDureeDVD.Text = "";
            labelRealisateurDVD.Text = "";
            labelImageDVD.Text = "";

            // Recherche du DVD correspondant au numéro de document saisi
            // Affichage des détails s'il est trouvé, sinon affichage d'un message d'erreur
            bool trouve = false;
            foreach (DVD dvd in lesDVDs)
            {
                if (dvd.IdDoc == txtBSaisirNumeroDoc.Text)
                {
                    labelNumeroDVD.Text = dvd.IdDoc;
                    labelTitreDVD.Text = dvd.Titre;
                    labelSynopsisDVD.Text = dvd.Synopsis;
                    labelDureeDVD.Text = dvd.Duree;
                    labelRealisateurDVD.Text = dvd.Realisateur;
                    labelImageDVD.Text = dvd.Image;
                    trouve = true;
                }
            }
            if (!trouve)
                MessageBox.Show("Document non trouvé dans les DVD");
        }

        // Recherche de DVD par titre en temps réel lors de la saisie dans le champ de recherche
        private void txtBRechercheTitre_TextChanged(object sender, EventArgs e)
        {
            // Effacement des lignes existantes dans le DataGridView
            dgvDVD.Rows.Clear();

            // Recherche des DVDs dont le titre correspond à la saisie
            foreach (DVD dvd in lesDVDs)
            {
                // Conversion des saisies et des titres en minuscules pour une recherche insensible à la casse
                string saisieMinuscules = txtBRechercheTitre.Text.ToLower();
                string titreMinuscules = dvd.Titre.ToLower();

                // Si le titre du DVD contient la saisie, il est ajouté au DataGridView
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dgvDVD.Rows.Add(dvd.IdDoc, dvd.Titre, dvd.Realisateur, dvd.Synopsis, dvd.Duree);
                }
            }
        }

        // Création d'un nouveau DVD
        private void btn_creationDVD_Click(object sender, EventArgs e)
        {
            // Récupération des valeurs des champs
            string numeroDoc = txtB_creation_numeroDoc.Text;
            string titre = txtB_creation_Titre.Text;
            string synopsis = txtB_creation_Synopsis.Text;
            string duree = txtB_creation_Duree.Text;
            string realisateur = txtB_creation_Realisateur.Text;
            string lienImage = txtB_creation_image.Text;

            // Récupération du libellé du public visé
            string libellePublic = cbB_choixPublic.SelectedItem?.ToString();

            // Vérification si tous les champs obligatoires sont remplis
            if (string.IsNullOrEmpty(numeroDoc) || string.IsNullOrEmpty(titre) || string.IsNullOrEmpty(synopsis) ||
                string.IsNullOrEmpty(duree) || string.IsNullOrEmpty(realisateur) || string.IsNullOrEmpty(lienImage) ||
                string.IsNullOrEmpty(libellePublic))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires ou sélectionner un public valide", "Champs incomplets", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPublic = cbB_choixPublic.SelectedIndex + 1;

            // Vérification de la validité du public sélectionné
            if (idPublic == -1)
            {
                MessageBox.Show("Le public sélectionné n'est pas valide.", "Public invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Création d'un objet DVD avec les valeurs récupérées
            DVD nouveauDVD = new DVD(
                numeroDoc,
                titre,
                synopsis,
                realisateur,
                duree,
                lienImage
            );

            // Insertion du DVD dans la base de données
            using (MySqlConnection connexion = DAOFactory.GetConnexion())
            {
                connexion.Open();
                DAODVD.InsererDVD(nouveauDVD, idPublic, connexion);
                // Affichage d'un message de succès
                MessageBox.Show("Le DVD a été créé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Effacement des champs après l'insertion réussie
                txtB_creation_numeroDoc.Clear();
                txtB_creation_Titre.Clear();
                txtB_creation_Synopsis.Clear();
                txtB_creation_Duree.Clear();
                txtB_creation_Realisateur.Clear();
                txtB_creation_image.Clear();
            }
        }

        // Suppression d'un DVD par son numéro de document
        private void btn_SuppressionDVD_Click(object sender, EventArgs e)
        {
            string idDocument = txtB_SuppressionDVD.Text;

            // Vérification si le champ de saisie est vide
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Veuillez entrer l'ID du document à supprimer.", "Champ obligatoire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Ouverture de la connexion à la base de données
                using (MySqlConnection connexion = DAOFactory.GetConnexion())
                {
                    connexion.Open();
                    // Vérification de l'existence du document à supprimer
                    bool idExiste = DAODVD.VerifierExistenceDocument(idDocument, connexion);
                    if (!idExiste)
                    {
                        MessageBox.Show("L'ID du document n'existe pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtB_SuppressionDVD.Clear();
                        return;
                    }
                    // Suppression du DVD correspondant à l'ID du document
                    DAODVD.SupprimerDVD(idDocument, connexion);
                    // Affichage d'un message de succès
                    MessageBox.Show("Le DVD a été supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtB_SuppressionDVD.Clear();
                }
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur en cas d'échec de suppression
                MessageBox.Show("Une erreur s'est produite lors de la suppression du DVD : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtB_SuppressionDVD.Clear();
            }
        }

        // Recherche d'un DVD pour modification par son numéro de document
        private void btn_Rechercher_modif_Click(object sender, EventArgs e)
        {
            // Vérification si le champ de saisie est vide
            if (!string.IsNullOrEmpty(txtB_modif_numeroDoc.Text))
            {
                using (MySqlConnection connection = DAOFactory.creerConnection())
                {
                    DAOFactory.connecter();
                    // Requête pour récupérer les informations du DVD à modifier
                    string query = "SELECT document.titre, document.image, dvd.synopsis, dvd.duree, dvd.realisateur, document.idPublic, public.libelle " +
                                    "FROM dvd " +
                                    "JOIN document ON dvd.idDocument = document.id " +
                                    "JOIN public ON document.idPublic = public.id " +
                                    "WHERE dvd.idDocument = @idDocument";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idDocument", txtB_modif_numeroDoc.Text);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Affichage des informations récupérées dans les champs correspondants
                                txtB_modif_Titre.Text = reader["titre"].ToString();
                                txtB_modif_Synopsis.Text = reader["synopsis"].ToString();
                                txtB_modif_Duree.Text = reader["duree"].ToString();
                                txtB_modif_Realisateur.Text = reader["realisateur"].ToString();
                                txtB_modif_Image.Text = reader["image"].ToString();
                                cbB_modifPublic.SelectedItem = reader["libelle"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Aucun enregistrement trouvé pour le numéro de document spécifié.");
                            }
                        }
                    }
                    DAOFactory.deconnecter();
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir un numéro de document.");
            }
        }

        // Modification d'un DVD
        private void btn_modifDVD_Click(object sender, EventArgs e)
        {
            // Vérification si les champs sont vides
            if (string.IsNullOrEmpty(txtB_modif_Titre.Text) || string.IsNullOrEmpty(txtB_modif_Synopsis.Text) ||
                string.IsNullOrEmpty(txtB_modif_Duree.Text) || string.IsNullOrEmpty(txtB_modif_Realisateur.Text) ||
                string.IsNullOrEmpty(txtB_modif_Image.Text) || cbB_modifPublic.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs pour effectuer la modification.", "Champs vides",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupération des valeurs des champs de modification
            string idDocument = txtB_modif_numeroDoc.Text;
            string titre = txtB_modif_Titre.Text;
            string image = txtB_modif_Image.Text;
            string synopsis = txtB_modif_Synopsis.Text;
            string duree = txtB_modif_Duree.Text;
            string realisateur = txtB_modif_Realisateur.Text;
            int idPublic = cbB_modifPublic.SelectedIndex + 1;

            try
            {
                // Ouverture de la connexion à la base de données
                using (MySqlConnection connexion = DAOFactory.GetConnexion())
                {
                    connexion.Open();
                    // Modification du DVD dans la base de données
                    int rowsAffected = DAODVD.ModifierDVD(idDocument, titre, image, synopsis, duree, realisateur, idPublic, connexion);
                    // Affichage d'un message de succès ou d'avertissement selon le nombre de lignes affectées
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Les modifications ont été appliquées avec succès.", "Modification réussie",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Aucune modification n'a été appliquée. Vérifiez l'ID du document spécifié.",
                                        "Aucune modification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur en cas d'échec de la modification
                MessageBox.Show("Une erreur s'est produite lors de la modification : " + ex.Message, "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Réinitialisation des champs de modification
        private void btn_supprContenu_Click(object sender, EventArgs e)
        {
            txtB_modif_numeroDoc.Clear();
            txtB_modif_Titre.Clear();
            txtB_modif_Synopsis.Clear();
            txtB_modif_Duree.Clear();
            txtB_modif_Realisateur.Clear();
            txtB_modif_Image.Clear();
            cbB_modifPublic.SelectedIndex = -1;
        }
        #endregion


        private void labelRealisateur_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }

        private void label16_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }

        private bool estConnecte = false;
        private void btn_connexion_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur le bouton de connexion.

            string login = txtB_login.Text;
            string motDePasse = txtB_mdp.Text;

            // Authentifier l'utilisateur
            Connection utilisateur = Connection.AuthentifierUtilisateur(login, motDePasse);

            if (utilisateur != null)
            {
                // L'authentification a réussi
                AfficherMessage("Connexion réussie. Bienvenue, " + utilisateur.Nom);

                label_nom_login.Text = utilisateur.Nom;
                label_prenom_login.Text = utilisateur.Prenom;
                label_service_login.Text = utilisateur.ServiceLibelle;

                btn_deconnexion.Enabled = true;

                estConnecte = true;

                GérerAccesPages(utilisateur);
            }
            else
            {
                // L'authentification a échoué
                AfficherMessage("Erreur d'authentification. Vérifiez votre login et votre mot de passe.");
            }
        }

        private void btn_deconnexion_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur le bouton de déconnexion.

            txtB_login.Text = "";
            txtB_mdp.Text = "";

            label_nom_login.Text = "";
            label_prenom_login.Text = "";
            label_service_login.Text = "";

            // Mettre à jour l'état de connexion
            estConnecte = false;

            btn_deconnexion.Enabled = false;

            GérerAccesPages(null);

            // Afficher un message de déconnexion
            AfficherMessage("Déconnexion réussie.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cette méthode est appelée lors du chargement du formulaire.
            // Elle masque les pages initialement.
            GérerAccesPages(null);
        }

        private void GérerAccesPages(Connection utilisateur)
        {
            // Cette méthode gère l'accès aux pages en fonction du type d'utilisateur connecté.

            if (utilisateur != null)
            {
                switch (utilisateur.ServiceLibelle)
                {
                    case "Administrateur":
                        // Afficher toutes les pages pour l'administrateur
                        tabOngletsApplication.TabPages.Add(tabParutions);
                        tabOngletsApplication.TabPages.Add(tabLivres);
                        tabOngletsApplication.TabPages.Add(tabDVD);
                        tabOngletsApplication.TabPages.Add(tabAbonnes);
                        tabOngletsApplication.TabPages.Add(tabTitres);
                        break;
                    case "Administratif":
                        // Afficher les pages pour l'administratif
                        tabOngletsApplication.TabPages.Add(tabAbonnes);
                        tabOngletsApplication.TabPages.Add(tabDVD);
                        tabOngletsApplication.TabPages.Add(tabLivres);
                        tabOngletsApplication.TabPages.Remove(tabParutions);
                        tabOngletsApplication.TabPages.Remove(tabTitres);
                        break;
                    case "Prêts":
                        // Afficher les pages pour les prêts
                        tabOngletsApplication.TabPages.Add(tabTitres);
                        tabOngletsApplication.TabPages.Remove(tabParutions);
                        tabOngletsApplication.TabPages.Remove(tabLivres);
                        tabOngletsApplication.TabPages.Remove(tabDVD);
                        tabOngletsApplication.TabPages.Remove(tabAbonnes);
                        break;
                    case "Culture":
                        // Afficher les pages pour la culture
                        tabOngletsApplication.TabPages.Add(tabParutions);
                        tabOngletsApplication.TabPages.Remove(tabLivres);
                        tabOngletsApplication.TabPages.Remove(tabDVD);
                        tabOngletsApplication.TabPages.Remove(tabAbonnes);
                        tabOngletsApplication.TabPages.Remove(tabTitres);
                        break;
                }
            }
            else
            {
                // Si l'utilisateur n'est pas connecté ou si les informations sur l'utilisateur ne sont pas disponibles,
                // toutes les pages sont masquées sauf la page de connexion.
                tabOngletsApplication.TabPages.Remove(tabLivres);
                tabOngletsApplication.TabPages.Remove(tabDVD);
                tabOngletsApplication.TabPages.Remove(tabAbonnes);
                tabOngletsApplication.TabPages.Remove(tabTitres);
                tabOngletsApplication.TabPages.Remove(tabParutions);
            }
        }

        private void AfficherMessage(string message)
        {
            // Cette méthode affiche un message dans une MessageBox, une étiquette, ou tout autre élément de l'IHM.
            MessageBox.Show(message);
        }

        private void label20_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }

        private void label25_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }

        private void btn_creerAbo_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur le bouton de création d'un abonné.

            try
            {
                string nom = txtB_saisirNom.Text;
                string prenom = txtB_saisirPrenom.Text;
                string adresse = txtB_saisirAdresse.Text;
                string telephone = txtB_saisirTelephone.Text;
                string adresse_mail = txtB_saisirMail.Text;
                string mot_de_passe = txtB_saisirMDP.Text;

                DateTime dateNaissance = dtp_naissance.Value;
                DateTime dateDebutAbonnement = dtp_debutAbonnement.Value;
                DateTime dateFinAbonnement = dtp_FinAbonnement.Value;

                if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(adresse) ||
                    string.IsNullOrEmpty(telephone) || string.IsNullOrEmpty(adresse_mail) || string.IsNullOrEmpty(mot_de_passe))
                {
                    MessageBox.Show("Veuillez remplir tous les champs obligatoires", "Champs incomplets", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Créer un objet Abonne
                Abonne nouvelAbonne = new Abonne(
                    nom,
                    prenom,
                    adresse,
                    telephone,
                    adresse_mail,
                    mot_de_passe,
                    dateNaissance,
                    dateDebutAbonnement,
                    dateFinAbonnement
                );

                using (MySqlConnection connexion = DAOFactory.GetConnexion())
                {
                    connexion.Open();
                    DAOAbonne.InsererAbonne(nouvelAbonne, connexion);
                }

                dgv_gestionAbonne.Rows.Add(nom, prenom, adresse, telephone, adresse_mail, mot_de_passe, dateNaissance, dateDebutAbonnement, dateFinAbonnement);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'abonné : " + ex.Message);
            }
        }

        private void dgv_gestionAbonne_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une cellule du DataGridView, mais elle est actuellement vide.
        }

        private void btn_choix_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur le bouton de choix, mais elle est actuellement vide.
        }

        private Abonne TrouverAbonneParAdresseMail(string adresseMail)
        {
            // Cette méthode recherche un abonné en fonction de son adresse e-mail dans la base de données.

            try
            {
                using (MySqlConnection connexion = DAOFactory.GetConnexion())
                {
                    connexion.Open();
                    string query = "SELECT * FROM Abonne WHERE adresse_mail = @AdresseMail";

                    using (MySqlCommand command = new MySqlCommand(query, connexion))
                    {
                        command.Parameters.AddWithValue("@AdresseMail", adresseMail);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Abonne(
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la recherche de l'abonné : " + ex.Message);
                throw;
            }

            return null;
        }

        private void btn_modifierAbo_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur le bouton de modification d'un abonné, mais elle est actuellement vide.
        }

        private GestionAbonnes gestionAbonnes = new GestionAbonnes();

        private void txtB_NomRechercher_TextChanged(object sender, EventArgs e)
        {
            // Cette méthode gère le changement de texte dans la zone de recherche par nom d'abonné, mais elle est actuellement vide.
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            // Cette méthode est appelée lorsque l'utilisateur entre dans l'onglet correspondant.
            // Elle charge tous les abonnés depuis la base de données.
            lesAbonnes = DAOAbonne.GetTousLesAbonnes();
        }

        private void label36_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }

        private void labelRealisateurDVD_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }

        private void label48_Click(object sender, EventArgs e)
        {
            // Cette méthode gère l'événement de clic sur une étiquette, mais elle est actuellement vide.
        }
    }
}
