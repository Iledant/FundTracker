# FundTracker

FundTracker est un logiciel permettant de suivre l'évolution des cours d'actions ou de fonds de placement. 
Il récupère l'historique des données via le compte MorningStar de l'utilisateur et permet un suivi simplifié

## Fonctions

### Gestion de portefeuilles

FundTracker permet de constituer des portefeuilles qui consistent en une liste de placements. 

L'utilisateur peut créer des portefeuilles en les nommant. Il peut les supprimer.

La liste comporte pour chaque placement comporte :
* son identification
* sa quantité
* sa valeur moyenne d'achat

La gestion de portefeuille consiste à ajouter ou retirer des lignes de placement.

### Gestion des placements

Une base d'historique est tenue à jour par FundTracker soit au moment de l'ajout d'un nouveau placement dans un portefeuille soit au lancement de FundTracker.

Les placements sont mutualisés entre les portefeuilles de l'utilisateur.

Lors de l'ajout d'un placement à un portefeuille, le placement est ajouté à la base si son identifiant MorningStar n'est pas connu.

La base de données des placements tient à jour le nombre de portefeuille contenant un placement.

Lorsque la suppression d'un placement fait qu'il n'y a plus aucune occurrence d'un placement dans un portefeuille, le placement est supprimé de la base de données et son historique est donc purgé.

Chaque ligne de placements comporte :
* l'identifiant unique MorningStar
* le nom du placement
* son historique de cours

### Gestion des paramètres

FundTracker stocke dans les paramètres de l'application les identifiants de l'utilisateur sur MorningStar pour récupérer l'historique des placements.

## Ecrans

### Liste des portefeuilles

La page principale de FundTracker qui apparaît lors du lancement de l'application est celle des portefeuilles. Elle contient la liste des portefeuilles créés par l'utilisateur.

La page permet de :
* créer un nouveau portefeuille
* supprimer un portefeuille
* renommer un portefeuille
* obtenir la valorisation du portefeuille

Les portefeuilles doivent avoir des noms différents

### Valorisation des portefeuilles

La page de valorisation des portefeuilles afficher la liste des placements et leur évolution en pourcentage en comparant le dernier cours connu avec la valeur moyenne d'achats.

La page de valorisation permet de :
* ajouter un placement
* supprimer un placement
* obtenir le détail de l'historique d'un placement

### Ajout d'un placement

La page d'ajout d'un placement comporte un champ de recherche du placement à partir de son nom. Elle affiche les résultats de la recherche fournit par Morningstar en affichant le nom du placement et quelques informations de classifications.

Une fois le fond sélectionné dans la liste, l'utilisateur doit ajouter la quantité du placement dans le portefeuille et la valeur moyenne d'achat.

### Historique d'un placement

La page d'historique d'un placement affiche un graphe de l'historique des valeurs et permet de zoomer sur des périodes de temps.