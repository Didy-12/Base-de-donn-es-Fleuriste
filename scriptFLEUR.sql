CREATE TABLE commande (numero_commande varchar(50) NOT NULL, date_commande varchar(1), adresse_livraison varchar(100), date_livraison varchar(1), message text(200), statut_commande varchar(10));

CREATE TABLE client (email varchar(20) NOT NULL, mdp varchar(20), nom varchar(10), prenom varchar(10), num_tel varchar(10), adresse_facture varchar(50), date_inscription varchar(10), statut_client varchar(10), carte_credit varchar(20));

CREATE TABLE standard (prix_bouquet_standard decimal(50) NOT NULL, nom_bouquet_standard varchar(40), id_standard varchar(50));

CREATE TABLE personnalisee (prix_max decimal(50) NOT NULL, id_personnalisee varchar(50), description text(200));

CREATE TABLE item (id_item varchar(50) NOT NULL, nom_item varchar(20), description_item text(100), prix_unite decimal(60));

CREATE TABLE arrangement (id_arrangement varchar(50) NOT NULL, quantite real(30000));

CREATE TABLE boutique (nom_boutique varchar(20) NOT NULL, ville_boutique varchar(20), adresse_boutique varchar(20));

CREATE TABLE stock (quantite_stock real(10000) NOT NULL);

CREATE TABLE effectue (email varchar(20) NOT NULL, numero_commande varchar(50) NOT NULL);

CREATE TABLE de_type (numero_commande varchar(50) NOT NULL, prix_bouquet_standard decimal(50) NOT NULL, prix_max decimal(50) NOT NULL);

CREATE TABLE contient (prix_max decimal(50) NOT NULL, id_item varchar(50) NOT NULL);

CREATE TABLE compose (id_arrangement varchar(50) NOT NULL, prix_max decimal(50) NOT NULL);

CREATE TABLE prepare (numero_commande varchar(50) NOT NULL, nom_boutique varchar(20) NOT NULL);

CREATE TABLE possede (nom_boutique varchar(20) NOT NULL, quantite_stock real(10000) NOT NULL);

ALTER TABLE commande ADD CONSTRAINT PK_commande PRIMARY KEY (numero_commande);

ALTER TABLE client ADD CONSTRAINT PK_client PRIMARY KEY (email);

ALTER TABLE standard ADD CONSTRAINT PK_standard PRIMARY KEY (prix_bouquet_standard);

ALTER TABLE personnalisee ADD CONSTRAINT PK_personnalisee PRIMARY KEY (prix_max);

ALTER TABLE item ADD CONSTRAINT PK_item PRIMARY KEY (id_item);

ALTER TABLE arrangement ADD CONSTRAINT PK_arrangement PRIMARY KEY (id_arrangement);

ALTER TABLE boutique ADD CONSTRAINT PK_boutique PRIMARY KEY (nom_boutique);

ALTER TABLE stock ADD CONSTRAINT PK_stock PRIMARY KEY (quantite_stock);

ALTER TABLE effectue ADD CONSTRAINT PK_effectue PRIMARY KEY (email, numero_commande);

ALTER TABLE de_type ADD CONSTRAINT PK_de_type PRIMARY KEY (numero_commande, prix_bouquet_standard, prix_max);

ALTER TABLE contient ADD CONSTRAINT PK_contient PRIMARY KEY (prix_max, id_item);

ALTER TABLE compose ADD CONSTRAINT PK_compose PRIMARY KEY (id_arrangement, prix_max);

ALTER TABLE prepare ADD CONSTRAINT PK_prepare PRIMARY KEY (numero_commande, nom_boutique);

ALTER TABLE possede ADD CONSTRAINT PK_possede PRIMARY KEY (nom_boutique, quantite_stock);

ALTER TABLE effectue ADD CONSTRAINT FK_effectue_email FOREIGN KEY (email) REFERENCES client (email);

ALTER TABLE effectue ADD CONSTRAINT FK_effectue_numero_commande FOREIGN KEY (numero_commande) REFERENCES commande (numero_commande);

ALTER TABLE de_type ADD CONSTRAINT FK_de_type_numero_commande FOREIGN KEY (numero_commande) REFERENCES commande (numero_commande);

ALTER TABLE de_type ADD CONSTRAINT FK_de_type_prix_bouquet_standard FOREIGN KEY (prix_bouquet_standard) REFERENCES standard (prix_bouquet_standard);

ALTER TABLE de_type ADD CONSTRAINT FK_de_type_prix_max FOREIGN KEY (prix_max) REFERENCES personnalisee (prix_max);

ALTER TABLE contient ADD CONSTRAINT FK_contient_prix_max FOREIGN KEY (prix_max) REFERENCES personnalisee (prix_max);

ALTER TABLE contient ADD CONSTRAINT FK_contient_id_item FOREIGN KEY (id_item) REFERENCES item (id_item);

ALTER TABLE compose ADD CONSTRAINT FK_compose_id_arrangement FOREIGN KEY (id_arrangement) REFERENCES arrangement (id_arrangement);

ALTER TABLE compose ADD CONSTRAINT FK_compose_prix_max FOREIGN KEY (prix_max) REFERENCES personnalisee (prix_max);

ALTER TABLE prepare ADD CONSTRAINT FK_prepare_numero_commande FOREIGN KEY (numero_commande) REFERENCES commande (numero_commande);

ALTER TABLE prepare ADD CONSTRAINT FK_prepare_nom_boutique FOREIGN KEY (nom_boutique) REFERENCES boutique (nom_boutique);

ALTER TABLE possede ADD CONSTRAINT FK_possede_nom_boutique FOREIGN KEY (nom_boutique) REFERENCES boutique (nom_boutique);

ALTER TABLE possede ADD CONSTRAINT FK_possede_quantite_stock FOREIGN KEY (quantite_stock) REFERENCES stock (quantite_stock);

