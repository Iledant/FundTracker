# FundTracker

FundTracker est un logiciel permettant de suivre l'�volution des cours d'actions ou de fonds de placement. 
Il r�cup�re l'historique des donn�es via le compte MorningStar de l'utilisateur et permet un suivi simplifi�

## Fonctions

### Gestion de portefeuilles

FundTracker permet de constituer des portefeuilles qui consistent en une liste de placements. 

L'utilisateur peut cr�er des portefeuilles en les nommant. Il peut les supprimer.

La liste comporte pour chaque placement comporte :
* son identification
* sa quantit�
* sa valeur moyenne d'achat

La gestion de portefeuille consiste � ajouter ou retirer des lignes de placement.

### Gestion des placements

Une base d'historique est tenue � jour par FundTracker soit au moment de l'ajout d'un nouveau placement dans un portefeuille soit au lancement de FundTracker.

Les placements sont mutualis�s entre les portefeuilles de l'utilisateur.

Lors de l'ajout d'un placement � un portefeuille, le placement est ajout� � la base si son identifiant MorningStar n'est pas connu.

La base de donn�es des placements tient � jour le nombre de portefeuille contenant un placement.

Lorsque la suppression d'un placement fait qu'il n'y a plus aucune occurrence d'un placement dans un portefeuille, le placement est supprim� de la base de donn�es et son historique est donc purg�.

Chaque ligne de placements comporte :
* l'identifiant unique MorningStar
* le nom du placement
* son historique de cours

### Gestion des param�tres

FundTracker stocke dans les param�tres de l'application les identifiants de l'utilisateur sur MorningStar pour r�cup�rer l'historique des placements.

## Ecrans

### Liste des portefeuilles

La page principale de FundTracker qui appara�t lors du lancement de l'application est celle des portefeuilles. Elle contient la liste des portefeuilles cr��s par l'utilisateur.

La page permet de :
* cr�er un nouveau portefeuille
* supprimer un portefeuille
* renommer un portefeuille
* obtenir la valorisation du portefeuille

Les portefeuilles doivent avoir des noms diff�rents

### Valorisation des portefeuilles

La page de valorisation des portefeuilles afficher la liste des placements et leur �volution en pourcentage en comparant le dernier cours connu avec la valeur moyenne d'achats.

La page de valorisation permet de :
* ajouter un placement
* supprimer un placement
* obtenir le d�tail de l'historique d'un placement

### Ajout d'un placement

La page d'ajout d'un placement comporte un champ de recherche du placement � partir de son nom. Elle affiche les r�sultats de la recherche fournit par Morningstar en affichant le nom du placement et quelques informations de classifications.

Une fois le fond s�lectionn� dans la liste, l'utilisateur doit ajouter la quantit� du placement dans le portefeuille et la valeur moyenne d'achat.

### Historique d'un placement

La page d'historique d'un placement affiche un graphe de l'historique des valeurs et permet de zoomer sur des p�riodes de temps.