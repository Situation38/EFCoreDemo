# EFCoreDemo
Premier test d'integration sur une application ASP.Net implementant le Framework Entity
Ici les tests les plus pertinents sont ceux de :

 - Acces a l'url de l'API
 - creation ou post d'un etudiant 
 - Affichage de la liste des etudiants

ces tests sont experimentes dans un premier tmps par la classe TestServer et dans un second temps par la classe WebApplicationFactory

Dans notre serie de test ,les tests sur la Post ne reussissent pas  car l'attribut pour le jeton anti contrefacon est declarer sur la methode post ou create de notre controller
pour palier a ce pB la methode la moins recommendee est de supprimer cet attribut ou pour mieux faire on va creer une classe AntiForgeryToken qui va nous permettre de recuper 
les cookies et les champ anti-contrefaçon.

L'amelioration de notre test se fera avec la creation et l'utilisation d'une classe AntiForgeryToken dans notre prochain commit.
