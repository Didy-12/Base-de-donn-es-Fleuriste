// See https://aka.ms/new-console-template for more information
using MySql.Data.MySqlClient;
using System;
using MySqlX.XDevAPI.Relational;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Net.Sockets;
using MySqlX.XDevAPI;
using System.Drawing;
using System.Data;

namespace projet
{
    class InterfaceClient
    {
        #region // ouverture de la base de données 

     static void Main(string[] args)
        {
            string connectionString = "SERVER=localhost; DATABASE=fleuriste;UID=root;PASSWORD=root;SslMode=none";
            MySqlConnection connexion = new MySqlConnection(connectionString);
            connexion.Open();
            try
            {
                Console.WriteLine("Connexion à la base de données reussie");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Impossible de se connecter à la base de données : " + ex.Message);
            }
            #endregion
            // Menu principal
            Console.WriteLine("1: inscription");
            Console.WriteLine("2: connexion");
            string y = Console.ReadLine();
            if (y == "1")
            {
                Console.WriteLine("inserer id_client");
                string id_client = Console.ReadLine();
                Console.WriteLine("inserer email");
                string email = Console.ReadLine();
                Console.WriteLine("inserer mdp");
                string mdp = Console.ReadLine();
                Console.WriteLine("inserer nom");
                string nom = Console.ReadLine();
                Console.WriteLine("inserer prenom");
                string prenom = Console.ReadLine();
                Console.WriteLine("inserer num_tel");
                string num_tel = Console.ReadLine();
                Console.WriteLine("inserer adresse_facture");
                string adresse_facture = Console.ReadLine();
                Console.WriteLine("inserer date_inscription");
                string date_inscription = Console.ReadLine();
                Console.WriteLine("inserer statut_client");
                string statut_client = Console.ReadLine();
                Console.WriteLine("inserer carte_credit");
                string carte_credit = Console.ReadLine();


                string commandeInscription = "INSERT INTO client (id_client,email,mdp,nom,prenom,num_tel,adresse_facture, date_inscription, statut_client, carte_credit) VALUES (@id_client, @email, @mdp, @nom, @prenom, @num_tel, @adresse_facture, @date_inscription, @statut_client, @carte_credit);";
                MySqlCommand command3 = new MySqlCommand(commandeInscription, connexion);
                command3.Parameters.AddWithValue("@id_client", id_client);
                command3.Parameters.AddWithValue("@mdp", mdp);
                command3.Parameters.AddWithValue("@email", email);
                command3.Parameters.AddWithValue("@nom", nom);
                command3.Parameters.AddWithValue("@prenom", prenom);
                command3.Parameters.AddWithValue("@num_tel", num_tel);
                command3.Parameters.AddWithValue("@adresse_facture", adresse_facture);
                command3.Parameters.AddWithValue("@date_inscription", date_inscription);
                command3.Parameters.AddWithValue("@statut_client", statut_client);
                command3.Parameters.AddWithValue("@carte_credit", carte_credit);
                MySqlDataReader reader3 = command3.ExecuteReader();

                Console.WriteLine("merci de vous être inscrit");
            }
            if (y == "2")
            {
                Console.WriteLine("Qui êtes vous ?");
                Console.WriteLine("1: Un client qui souhaite passer commande");
                Console.WriteLine("2: Un employé qui souhaites avoir accès aux commandes");
                Console.WriteLine("3: Un directeur qui souhaite voir les statistiques");
                string t = Console.ReadLine();
                if (t == "1")
                {
                    Console.WriteLine("Connectez vous avec votre email et mot de passe");
                    string email = Console.ReadLine();
                    Console.WriteLine("inserer mdp");
                    string mdp = Console.ReadLine();

                    // Vérifier si le client existe dans la base de données
                    MySqlCommand command4 = connexion.CreateCommand();
                    command4.CommandText = "SELECT COUNT(*) FROM client WHERE email = @email AND mdp = @mdp";
                    command4.Parameters.AddWithValue("@email", email);
                    command4.Parameters.AddWithValue("@mdp", mdp);
                    MySqlDataReader reader = command4.ExecuteReader();
                    //int count = Convert.ToInt32(command4.ExecuteScalar())
                    int count = -1;
                    while (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                    reader.Close();
                    Console.WriteLine(count);
                    if (count == 0)
                        Console.WriteLine("ce n'est pas le bon mot de passe");
                    else if (0 < count)
                    {
                        Console.WriteLine("Bienvenue " + email);
                        Console.WriteLine("Choisissez votre bouquet : ");
                        Console.WriteLine("1: Bouquet Standard");
                        Console.WriteLine("2: Bouquet Personnalisé");
                        string choixBouquet = Console.ReadLine();

                        if (choixBouquet == "1")
                        {
                            Console.WriteLine("Vous avez choisi le Bouquet Standard.");
                            try
                            {
                                // Récupérer les noms de bouquet standard depuis la table "standard"
                                MySqlCommand command = new MySqlCommand("SELECT nom_bouquet_standard FROM standard", connexion);
                                MySqlDataReader reader2 = command.ExecuteReader();

                                // Afficher chaque nom de bouquet avec un numéro associé
                                int i = 1;
                                while (reader2.Read())
                                {
                                    Console.WriteLine(i + ". " + reader2.GetString(0));
                                    i++;
                                }

                                reader2.Close();

                                // Demander à l'utilisateur de sélectionner un bouquet grâce à son numéro
                                Console.Write("Veuillez sélectionner un bouquet en entrant son numéro : ");
                                int selectedBouquetNumber = int.Parse(Console.ReadLine());

                                // Récupérer le prix du bouquet sélectionné
                                command = new MySqlCommand("SELECT prix_bouquet_standard FROM standard WHERE id_standard = @id", connexion);
                                command.Parameters.AddWithValue("@id", "STD0" + selectedBouquetNumber.ToString());
                                float bouquetPrice = (float)command.ExecuteScalar();

                                // Récupérer le statut du client (ici on suppose qu'on a l'id du client)
                                string clientId = "CLIENT_ID";
                                command = new MySqlCommand("SELECT statut_client FROM client WHERE id_client = @id", connexion);
                                command.Parameters.AddWithValue("@id", clientId);
                                string clientStatus = (string)command.ExecuteScalar();

                                // Appliquer la réduction de prix selon le statut du client
                                if (clientStatus == "OR")
                                {
                                    bouquetPrice *= 0.85f;
                                }
                                else if (clientStatus == "Bronze")
                                {
                                    bouquetPrice *= 0.95f;
                                }

                                // Afficher le choix de l'utilisateur ainsi que le prix final du bouquet
                                Console.WriteLine("Vous avez choisi le bouquet \"" + reader.GetString(selectedBouquetNumber - 1) + "\" au prix de " + bouquetPrice.ToString("0.00") + "€.");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            // ajout de la commande dans la table commande
                            // Lecture de l'ID de la dernière commande insérée
                            MySqlCommand command12 = new MySqlCommand("SELECT MAX(id_commande) FROM commande", connexion);
                            string lastOrderId = (string)command12.ExecuteScalar();

                            // Calcul du nouvel ID de commande
                            int newOrderId = Int32.Parse(lastOrderId.Substring(1)) + 1;
                            string newOrderIdString = "C" + newOrderId.ToString("D4");

                            // Lecture des informations de la nouvelle commande saisies par le client
                            Console.Write("Entrez le numéro de commande : ");
                            string numeroCommande = Console.ReadLine();

                            Console.Write("Entrez la date de commande (format : YYYY-MM-DD) : ");
                            DateTime dateCommande = DateTime.Parse(Console.ReadLine());

                            Console.Write("Entrez l'adresse de livraison : ");
                            string adresseLivraison = Console.ReadLine();

                            Console.Write("Entrez la date de livraison (format : YYYY-MM-DD) : ");
                            DateTime dateLivraison = DateTime.Parse(Console.ReadLine());

                            // Vérification de la contrainte
                            TimeSpan delaiLivraison = dateLivraison - dateCommande;
                            if (delaiLivraison.TotalDays < 3)
                            {
                                Console.WriteLine("Attention, le délai de livraison est inférieur à 3 jours. Il est possible que la commande soit en retard.");
                            }

                            Console.Write("Entrez un message (ou saisir 'aucun' si vous ne souhaitez pas de message) : ");
                            string message = Console.ReadLine();
                            if (message.ToLower() == "aucun")
                            {
                                message = "";
                            }

                            // Statut de commande par défaut : VINV
                            string statutCommande = "VINV";

                            Console.Write("Saisir votre ID client : ");
                            string idClient = Console.ReadLine();

                            Console.Write("Saisir le nom de la boutique où vous souhaitez commander : ");
                            string nomBoutique = Console.ReadLine();

                            // Création de la commande à insérer dans la table "commande"
                            MySqlCommand command2 = new MySqlCommand("INSERT INTO commande (id_commande, numero_commande, date_commande, adresse_livraison, date_livraison, message, statut_commande, id_client, nom_boutique) VALUES (@idCommande, @numeroCommande, @dateCommande, @adresseLivraison, @dateLivraison, @message, @statutCommande, @idClient, @nomBoutique)", connexion);
                            command2.Parameters.AddWithValue("@idCommande", newOrderIdString);
                            command2.Parameters.AddWithValue("@numeroCommande", numeroCommande);
                            command2.Parameters.AddWithValue("@dateCommande", dateCommande);
                            command2.Parameters.AddWithValue("@adresseLivraison", adresseLivraison);
                            command2.Parameters.AddWithValue("@dateLivraison", dateLivraison);
                            command2.Parameters.AddWithValue("@message", message);
                            command2.Parameters.AddWithValue("@statutCommande", statutCommande);
                            command2.Parameters.AddWithValue("@idClient", idClient);
                            command2.Parameters.AddWithValue("@nomBoutique", nomBoutique);
                            command2.ExecuteNonQuery();

                            Console.WriteLine("La commande a été ajoutée avec succès !");
                        }
                        else if (choixBouquet == "2")
                        {
                            Console.WriteLine("Vous avez choisi le Bouquet Personnalisé.");
                            Console.WriteLine("Voulez vous qu'un employer créer votre bouquet ? :");
                            Console.WriteLine("1: oui");
                            Console.WriteLine("2: non");
                            string c = Console.ReadLine();
                            if (c == "1")
                            {
                                Console.WriteLine("Vous avez choisi qu'un employé compose votre bouquet");
                                Console.WriteLine("donner une description générale de l’arrangement floral voulu :");
                                string description = Console.ReadLine();
                                Console.WriteLine("donner le prix maximum :");
                                float prixMax = float.Parse(Console.ReadLine());
                                /*string commandeNote = "INSERT INTO commande (id_client,email,mdp,nom,prenom,num_tel,adresse_facture, date_inscription, statut_client, carte_credit) VALUES (@id_client, @email, @mdp, @nom, @prenom, @num_tel, @adresse_facture, @date_inscription, @statut_client, @carte_credit);";
                                MySqlCommand command7 = new MySqlCommand(commandeNote, connexion);
                                command7.Parameters.AddWithValue("@id_client", id_client);
                                command7.Parameters.AddWithValue("@mdp", mdp);
                                command7.Parameters.AddWithValue("@email", email);
                                command7.Parameters.AddWithValue("@nom", nom);
                                command7.Parameters.AddWithValue("@prenom", prenom);
                                MySqlDataReader reader7 = command7.ExecuteReader();
                                connexion.Close();
                                Console.WriteLine("merci pour votre commande de fleur");*/
                            }
                            if (c == "2")
                            {
                                Console.WriteLine("Vous avez choisi de composer votre bouquet");
                                Console.WriteLine("Desirez-vous des fleurs ?");
                                Console.WriteLine("1: Oui");
                                Console.WriteLine("2: Non");
                                string d = Console.ReadLine();
                                if (d == "1")
                                {
                                    Console.WriteLine("Vous avez choisi de prendre des fleurs");

                                    // Affichage des noms de fleurs avec leur numéro associé
                                    Console.WriteLine("Voici les fleurs disponibles :");

                                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                                    {
                                        connection.Open();
                                        MySqlCommand command10 = connection.CreateCommand();
                                        command10.CommandText = "SELECT * FROM fleur";
                                        MySqlDataReader reader2 = command10.ExecuteReader();

                                        while (reader2.Read())
                                        {
                                            string currentRowAsString = "";
                                            for (int i = 0; i < reader2.FieldCount; i++)
                                            {
                                                string valueAsString = reader2.GetValue(i).ToString();
                                                currentRowAsString += valueAsString + ", ";
                                            }
                                            Console.WriteLine(currentRowAsString);
                                        }

                                        connection.Close();
                                    }

                                    // Demander le budget de l'utilisateur
                                    Console.WriteLine("Quel est votre budget ?");
                                    float budget = float.Parse(Console.ReadLine());

                                    // Boucle de sélection des fleurs et quantités
                                    bool continuer = true;
                                    while (continuer)
                                    {
                                        // Demander à l'utilisateur de saisir le numéro de la fleur qu'il souhaite
                                        Console.WriteLine("Saisissez le numéro de la fleur que vous voulez acheter (0 pour terminer) :");
                                        int numeroFleur = int.Parse(Console.ReadLine());

                                        if (numeroFleur == 0)
                                        {
                                            continuer = false;
                                            break;
                                        }

                                        // Demander à l'utilisateur de saisir la quantité de la fleur qu'il souhaite
                                        Console.WriteLine("Saisissez la quantité de fleurs que vous voulez acheter :");
                                        int quantite = int.Parse(Console.ReadLine());

                                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                                        {
                                            connection.Open();

                                            // Récupérer les informations de la fleur sélectionnée dans la base de données
                                            string query = "SELECT * FROM fleur WHERE id_fleur = @id_fleur";
                                            MySqlCommand command3 = new MySqlCommand(query, connection);
                                            command3.Parameters.AddWithValue("@id_fleur", numeroFleur);
                                            MySqlDataReader reader3 = command3.ExecuteReader();

                                            string nomFleur = "";
                                            int quantiteFleur = 0;
                                            float prixFleur = 0;
                                            int stockFleur = 0;

                                            if (reader3.Read())
                                            {
                                                nomFleur = reader3.GetString(0);
                                                quantiteFleur = reader3.GetInt32(1);
                                                prixFleur = reader3.GetFloat(2);
                                                stockFleur = reader3.GetInt32(3);
                                            }

                                            reader3.Close();

                                            // Vérifier si la quantité demandée est disponible en stock
                                            if (quantite <= stockFleur)
                                            {
                                                // Calculer le prix total de la sélection
                                                float prixTotal1 = quantite * prixFleur;

                                                // Vérifier si le budget de l'utilisateur est suffisant
                                                if (budget >= prixTotal1)
                                                {
                                                    // Mettre à jour la quantité en stock de la fleur sélectionnée dans la base de données
                                                    query = "UPDATE fleur SET stock_fleur = @stock_fleur WHERE id_fleur = @id_fleur";
                                                    MySqlCommand command11 = new MySqlCommand(query, connection);
                                                    command11.Parameters.AddWithValue("@nom_fleur", nomFleur);
                                                    command11.Parameters.AddWithValue("@stock_fleur", stockFleur - quantite);
                                                    command11.ExecuteNonQuery();

                                                    // Soustraire le prix total de la sélection au budget
                                                    budget -= prixTotal1;

                                                    Console.WriteLine($"Vous avez acheté {quantite} {nomFleur}(s) pour {prixTotal1} euros.");
                                                    Console.WriteLine($"Il vous reste {budget} euros dans votre budget.");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Désolé, votre budget est insuffisant pour acheter cette quantité de fleurs.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Désolé, il n'y a que {stockFleur} {nomFleur}(s) en stock.");
                                            }
                                        }

                                        Console.WriteLine("Merci d'avoir utilisé notre service !");
                                    }
                                }
                                // Récupération du statut du client
                                /* Console.Write("Entrez votre statut de client (OR ou Bronze) : ");
                                     string statutClient = Console.ReadLine();

                                     // Calcul du prix total des fleurs sélectionnées
                                     int prixTotal = 0;
                                     foreach (Tuple<string, int, int> fleur in fleursSelectionnees)
                                     {
                                         int quantiteFleur = fleur.Item2;
                                         int prixFleur = fleur.Item3;
                                         int prixTotalFleur = 0;
                                         if (statutClient == "OR")
                                         {
                                             prixTotalFleur = (int)(prixFleur * 0.85 * quantiteFleur);
                                         }
                                         else if (statutClient == "Bronze")
                                         {
                                             prixTotalFleur = (int)(prixFleur * 0.95 * quantiteFleur);
                                         }
                                         prixTotal += prixTotalFleur;
                                     }

                                     Console.WriteLine($"Total : {prixTotal}€");
                                 }*/
                                else if (d == "2")
                                {
                                    Console.WriteLine("Vous ne désirez pas de fleurs.");
                                }
                                else
                                {
                                    Console.WriteLine("Sélection invalide !");
                                }
                                Console.WriteLine("desirez vous des accessoires ?");
                                Console.WriteLine("1: oui");
                                Console.WriteLine("2: non");
                                string e = Console.ReadLine();
                                if (e == "1")
                                {
                                    Console.WriteLine("Vous avez choisi de prendre des accessoires");
                                    // Affichage des noms des accessoires avec leur numéro associé
                                    MySqlCommand command11 = new MySqlCommand("SELECT nom_accessoire, prix_accessoire FROM accessoire", connexion);
                                    MySqlDataReader reader4 = command11.ExecuteReader();

                                    Console.WriteLine("Sélectionnez les accessoires que vous souhaitez acheter en entrant les numéros correspondants :");

                                    List<(string, int)> accessoires = new List<(string, int)>();
                                    int index2 = 1;
                                    while (reader4.Read())
                                    {
                                        string nomAccessoire = reader4.GetString(0);
                                        int prix = reader4.GetInt32(1);
                                        accessoires.Add((nomAccessoire, prix));
                                        Console.WriteLine($"{index2}. {nomAccessoire} ({prix}€)");
                                        index2++;
                                    }

                                    reader4.Close();

                                    Dictionary<string, int> selection = new Dictionary<string, int>();
                                    bool continuer = true;
                                    while (continuer)
                                    {
                                        Console.Write("Sélectionnez (0 pour arrêter) : ");
                                        int selectionAccessoire = Int32.Parse(Console.ReadLine());
                                        if (selectionAccessoire == 0)
                                        {
                                            continuer = false;
                                        }
                                        else if (selectionAccessoire > 0 && selectionAccessoire < index2)
                                        {
                                            string nomAccessoire = accessoires[selectionAccessoire - 1].Item1;
                                            int prix = accessoires[selectionAccessoire - 1].Item2;

                                            Console.WriteLine($"Accessoire {nomAccessoire} sélectionné.");

                                            int quantite = 0;
                                            while (quantite <= 0)
                                            {
                                                Console.Write("Quantité : ");
                                                quantite = Int32.Parse(Console.ReadLine());
                                            }

                                            selection[nomAccessoire] = quantite;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Sélection invalide !");
                                        }
                                    }

                                    int prixTotal2 = 0;
                                    foreach (KeyValuePair<string, int> item in selection)
                                    {
                                        string nomAccessoire = item.Key;
                                        int quantite = item.Value;
                                        int prix = accessoires.Find(x => x.Item1 == nomAccessoire).Item2;

                                        prixTotal2 += quantite * prix;
                                    }



                                    // Application de la réduction en fonction du statut du client
                                    /*if (statutClient == "OR")
                                    {
                                        prixTotal2 = (int)(prixTotal2 * 0.85);
                                    }
                                    else if (statutClient == "Bronze")
                                    {
                                        prixTotal2 = (int)(prixTotal2 * 0.95);
                                    }*/

                                    Console.WriteLine($"Total : {prixTotal2}€");
                                }

                                if (e == "2")
                                {
                                    Console.WriteLine("Vous ne desirez pas d'accesoires");
                                }
                                // ajout de la commande dans la table commande
                                // Lecture de l'ID de la dernière commande insérée
                                MySqlCommand command12 = new MySqlCommand("SELECT MAX(id_commande) FROM commande", connexion);
                                string lastOrderId = (string)command12.ExecuteScalar();

                                // Calcul du nouvel ID de commande
                                int newOrderId = Int32.Parse(lastOrderId.Substring(1)) + 1;
                                string newOrderIdString = "C" + newOrderId.ToString("D4");

                                // Lecture des informations de la nouvelle commande saisies par le client
                                Console.Write("Entrez le numéro de commande : ");
                                string numeroCommande = Console.ReadLine();

                                Console.Write("Entrez la date de commande (format : YYYY-MM-DD) : ");
                                DateTime dateCommande = DateTime.Parse(Console.ReadLine());

                                Console.Write("Entrez l'adresse de livraison : ");
                                string adresseLivraison = Console.ReadLine();

                                Console.Write("Entrez la date de livraison (format : YYYY-MM-DD) : ");
                                DateTime dateLivraison = DateTime.Parse(Console.ReadLine());

                                // Vérification de la contrainte
                                TimeSpan delaiLivraison = dateLivraison - dateCommande;
                                if (delaiLivraison.TotalDays < 3)
                                {
                                    Console.WriteLine("Attention, le délai de livraison est inférieur à 3 jours. Il est possible que la commande soit en retard.");
                                }

                                Console.Write("Entrez un message (ou saisir 'aucun' si vous ne souhaitez pas de message) : ");
                                string message = Console.ReadLine();
                                if (message.ToLower() == "aucun")
                                {
                                    message = "";
                                }

                                // Statut de commande par défaut : CPAV
                                string statutCommande = "CPAV";

                                Console.Write("Saisir votre ID client : ");
                                string idClient = Console.ReadLine();

                                Console.Write("Saisir le nom de la boutique où vous souhaitez commander : ");
                                string nomBoutique = Console.ReadLine();

                                // Création de la commande à insérer dans la table "commande"
                                MySqlCommand command2 = new MySqlCommand("INSERT INTO commande (id_commande, numero_commande, date_commande, adresse_livraison, date_livraison, message, statut_commande, id_client, nom_boutique) VALUES (@idCommande, @numeroCommande, @dateCommande, @adresseLivraison, @dateLivraison, @message, @statutCommande, @idClient, @nomBoutique)", connexion);
                                command2.Parameters.AddWithValue("@idCommande", newOrderIdString);
                                command2.Parameters.AddWithValue("@numeroCommande", numeroCommande);
                                command2.Parameters.AddWithValue("@dateCommande", dateCommande);
                                command2.Parameters.AddWithValue("@adresseLivraison", adresseLivraison);
                                command2.Parameters.AddWithValue("@dateLivraison", dateLivraison);
                                command2.Parameters.AddWithValue("@message", message);
                                command2.Parameters.AddWithValue("@statutCommande", statutCommande);
                                command2.Parameters.AddWithValue("@idClient", idClient);
                                command2.Parameters.AddWithValue("@nomBoutique", nomBoutique);
                                command2.ExecuteNonQuery();

                                Console.WriteLine("La commande a été ajoutée avec succès !");
                                /*Console.WriteLine("entrez l'id_commande");
                                Console.WriteLine("entrez le numero de commande");
                                Console.WriteLine("entrez la date de commande");
                                Console.WriteLine("entrez l'adresse de livraison");
                                Console.WriteLine("entrez la date de livraison");
                                Console.WriteLine("entrez l'id_commande");
                                Console.WriteLine("entrez un message ou saisir aucun si vous ne souhaitez pas de message");
                                Console.WriteLine("Le statut de votre commande est  CPAV");
                                Console.WriteLine("saisir votre id_client");
                                Console.WriteLine("saisir le nom de la boutique où vous souhaitez commander");*/
                            }
                        }
                    }
                    if (t == "2")
                    {
                        Console.WriteLine("Connectez vous avec votre email et mot de passe");
                        string email_v = Console.ReadLine();
                        Console.WriteLine("inserer mdp");
                        string mdp_v = Console.ReadLine();

                        // Vérifier si le vendeur existe dans la base de données
                        MySqlCommand command9 = connexion.CreateCommand();
                        command9.CommandText = "SELECT COUNT(*) FROM vendeur WHERE email_v = @email_v AND mdp_v = @mdp_v";
                        command9.Parameters.AddWithValue("@email_v", email_v);
                        command9.Parameters.AddWithValue("@mdp_v", mdp_v);
                        MySqlDataReader reader5 = command9.ExecuteReader();
                        int count1 = -1;
                        while (reader5.Read())
                        {
                            count1 = reader5.GetInt32(0);
                        }
                        reader5.Close();
                        Console.WriteLine(count1);

                        //int count1 = Convert.ToInt32(command9.ExecuteScalar());
                        if (count1 == 0)
                            Console.WriteLine("ce n'est pas le bon mot de passe");
                        else if (0 < count1)
                        {
                            Console.WriteLine("Bienvenue vendeur " + email);
                        }
                        // Récupération du numéro de commande saisi par le vendeur
                        Console.WriteLine("Veuillez saisir le numéro de commande : ");
                        string numeroCommande = Console.ReadLine();

                        // Requête SQL pour récupérer le statut de la commande
                        string sql = "SELECT statut_commande FROM commande WHERE numero_commande = @numeroCommande";
                        MySqlCommand command = new MySqlCommand(sql, connexion);
                        command.Parameters.AddWithValue("@numeroCommande", numeroCommande);


                        // Exécution de la requête et récupération du résultat
                        string statutCommande = command.ExecuteScalar() as string;

                        // Vérification et modification du statut de la commande
                        switch (statutCommande)
                        {
                            case "VINV":
                                Console.WriteLine("La commande doit être vérifiée par un employé.");
                                break;
                            case "CC":
                                Console.WriteLine("La commande est complète et prête à être livrée.");
                                break;
                            case "CPAV":
                                Console.WriteLine("La commande personnalisée doit être vérifiée par un employé.");
                                Console.WriteLine("Des items peuvent être ajoutés si nécessaire.");
                                Console.WriteLine("Veuillez saisir 'CC' si la commande est complète : ");
                                string nouveauStatut = Console.ReadLine();
                                if (nouveauStatut == "CC")
                                {
                                    // Requête SQL pour mettre à jour le statut de la commande
                                    string updateSql = "UPDATE commande SET statut_commande = 'CC' WHERE numero_commande = @numeroCommande";
                                    MySqlCommand updateCommand = new MySqlCommand(updateSql, connexion);
                                    updateCommand.Parameters.AddWithValue("@numeroCommande", numeroCommande);
                                    updateCommand.ExecuteNonQuery();
                                    Console.WriteLine("Le statut de la commande a été mis à jour : CC.");
                                }
                                break;
                            case "CAL":
                                Console.WriteLine("La commande est prête à être livrée.");
                                break;
                            case "CL":
                                Console.WriteLine("La commande a déjà été livrée.");
                                break;
                            default:
                                Console.WriteLine("Le statut de la commande est inconnu.");
                                break;
                        }
                        // Récupération des produits dont la quantité en stock est inférieure à leur seuil d'alerte
                        MySqlCommand command20 = new MySqlCommand("SELECT * FROM fleur WHERE quantite_fleur < seuil_alerte", connexion);
                        MySqlDataReader reader20 = command.ExecuteReader();

                        while (reader20.Read())
                        {
                            string nomFleur = reader20.GetString("nom_fleur");
                            int quantiteFleur = reader20.GetInt32("quantite_fleur");
                            int seuilAlerte = reader20.GetInt32("seuil_alerte");
                            string messageAlerte = reader20.GetString("message_alerte");

                            Console.WriteLine("ALERTE : la quantité en stock de la fleur " + nomFleur + " est de " + quantiteFleur + ", ce qui est inférieur à son seuil d'alerte de " + seuilAlerte);
                            Console.WriteLine("Message d'alerte : " + messageAlerte);
                        }

                        reader20.Close();

                        // Récupération des produits dont la quantité en stock est inférieure à leur seuil d'alerte
                        MySqlCommand command2 = new MySqlCommand("SELECT * FROM accessoire WHERE quantite_accessoire < seuil_alerte", connexion);
                        MySqlDataReader reader2 = command2.ExecuteReader();

                        while (reader2.Read())
                        {
                            string nomAccessoire = reader2.GetString("nom_accessoire");
                            int quantiteAccessoire = reader2.GetInt32("quantite_accessoire");
                            int seuilAlerte = reader2.GetInt32("seuil_alerte");
                            string messageAlerte = reader2.GetString("message_alerte");

                            Console.WriteLine("ALERTE : la quantité en stock de l'accessoire " + nomAccessoire + " est de " + quantiteAccessoire + ", ce qui est inférieur à son seuil d'alerte de " + seuilAlerte);
                            Console.WriteLine("Message d'alerte : " + messageAlerte);
                        }
                        if (t == "3")
                        {
                            Console.WriteLine("Connectez vous avec votre email et mot de passe");
                            string email_d = Console.ReadLine();
                            Console.WriteLine("inserer mdp");
                            string mdp_d = Console.ReadLine();

                            // Vérifier si le directeur existe dans la base de données
                            MySqlCommand command10 = connexion.CreateCommand();
                            command10.CommandText = "SELECT COUNT(*) FROM directeur WHERE email_d = @email_d AND mdp_d = @mdp_d";
                            command10.Parameters.AddWithValue("@email_d", email_d);
                            command10.Parameters.AddWithValue("@mdp_d", mdp_d);
                            //int count10 = Convert.ToInt32(command10.ExecuteScalar());
                            MySqlDataReader reader7 = command10.ExecuteReader();
                            int count10 = -1;
                            while (reader7.Read())
                            {
                                count10 = reader7.GetInt32(0);
                            }
                            reader7.Close();
                            Console.WriteLine(count10);
                            if (count10 == 0)
                                Console.WriteLine("ce n'est pas le bon mot de passe");
                            else if (0 < count10)
                            {
                                Console.WriteLine("Bienvenue directeur " + email);
                            }
                            Console.WriteLine("Quelle statistique souhaitez vous consulter ?");
                            Console.WriteLine("1: Calcul du prix moyen du bouquet acheté");
                            Console.WriteLine("2: Quel est le meilleur client du mois");
                            Console.WriteLine("3: Quel est le meilleur client de l'année");
                            Console.WriteLine("4: Quel est le bouquet standard qui a eu le plus de succès");
                            Console.WriteLine("5: Quel est le magasin qui a généré le plus de chiffre d’affaires");
                            Console.WriteLine("6: Quelle est la fleur exotique la moins vendue");
                            string f = Console.ReadLine();
                            if (f == "1")
                            {
                                Console.WriteLine("Vous avez choisi de connaitre prix moyen du bouquet acheté");

                            }
                            if (f == "2")
                            {
                                Console.WriteLine("Vous avez choisi de connaitre le meilleur client du mois");

                            }
                            if (f == "3")
                            {
                                Console.WriteLine("Vous avez choisi de connaitre le meilleur client de l'année");

                            }
                            if (f == "4")
                            {
                                Console.WriteLine("Vous avez choisi de connaitre le bouquet standard qui a eu le plus de succès");

                            }
                            if (f == "5")
                            {
                                Console.WriteLine("Vous avez choisi de connaitre le magasin qui a généré le plus de chiffre d’affaires");

                            }
                            if (f == "6")
                            {
                                Console.WriteLine("Vous avez choisi de connaitre la fleur exotique la moins vendue");

                            }
                        }
                    }
                }
            }
        }
    }
}


/*int x = 0 /// int x ch = 0;
for (x = 0; i < 4; i++)
{
    #region // connexion du client
    if (x==1)
    {
        connexion.Open();

    }
    #endregion

    #region // inscription du client
    if (x == 2)
    {
        connexion.Open();

    }
    #endregion

    #region // afficher la liste des clients
    if (x == 3)
    {
        connexion.Open();
        string queryString = "SELECT * FROM client";
        MySqlCommand command = new MySqlCommand(queryString, connexion);
        MySqlDataReader dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            string nom = (string)dataReader["nom"];
            string prenom = (string)dataReader["prenom"];
            int num_tel = (int)dataReader["num_tel"];
            string email = (string)dataReader["email"];
            string mdp = (string)dataReader["mdp"];
            string adresse = (string)dataReader["adresse"];
            int carte_credit = (int)dataReader["carte_credit"];

            Console.WriteLine($"nom={nom}, prenom={prenom}, num_tel={num_tel}, email={email}, mdp={mdp}, adresse ={adresse}, carte_credit={carte_credit}");
        }
        dataReader.Close();
        connexion.Close();
    }
#endregion

}
}
Console.WriteLine("1 : Bouquet de roses");
                    Console.WriteLine("2 : Bouquet de lys");
                    Console.WriteLine("3 : Bouquet de marguerites");
                    Console.WriteLine("4 : Bouquet de jacinthes");
                    Console.WriteLine("5 : Bouquet de muguet");
                    Console.WriteLine("6 : Bouquet de jonquilles");
                    Console.WriteLine("7 : Bouquet de tulipes");
                    string choixStandard = Console.ReadLine();
                    if (choixStandard == "1")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de roses");
                    }
                    else if (choixStandard == "2")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de lys");
                    }
                    else if (choixStandard == "3")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de margherites");
                    }
                    else if (choixStandard == "4")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de jacinthes");
                    }
                    else if (choixStandard == "5")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de muguet");
                    }
                    else if (choixStandard == "6")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de jonquilles");
                    }
                    else if (choixStandard == "7")
                    {
                        Console.WriteLine("Vous avez choisi le Bouquet de tulipes");
                    }*/

