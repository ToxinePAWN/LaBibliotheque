Hello Mickael

Pendant notre discussion la fois passée et ces dernières semaines, le concept de clean architecture est revenu plusieurs fois. J'ai voulu profiter de cette occasion pour découvrir cette architecture et essayer de répondre au sujet en suivant ce modèle. 

Pour découvrir comment ça fonctionne et ne pas perdre trop de temps d'implémentation, je me suis basé sur un template existant (https://github.com/stphnwlsh/CleanMinimalApi). C'est le plus simple que je l'ai trouvé même si il contient déjà beaucoup pour ce qu'on veut faire

De base, il permet de mettre des review sur des films. 
Je n'ai pas eu le temps de renommer proprement, j'ai réutilisé les entités existantes en suivant :
Author = member
Review = loan
Movie = book 
Sur la partie endpoint, certaines logiques il y a encore beaucoup de mentions des anciennes entitées. 

Pour partager mes choix :

Un livre peut avoir plusieurs exemplaires
J'ai choisi de le gérer avec un simple nombre qui indique le nombre d'exemplaires pour un livre. 
Ça permet de ne pas à avoir à faire un livre par exemplaire. Pour check si on peut l'emprunter, dans les logiques il suffit de regarder combien a d'emprunt en cours pour un livre donné afin de savoir si il reste des exemplaires. 

Je n'ai pas terminé la partie création de membre et de livre, j'ai focus sur la partie emprunt qui me semblait être la plus pertinentes notamment pour les règles métiers 

Je n'ai pas eu le temps d'aborder la partie testing 

La création d'un emprunt fonctionne et les règles métiers s'appliquent bien (testable facilement depuis le swagger)
Si tu veux tester, et que tu as la 409 qui dit que tu as un emprunt en retard, il faut essayer avec un autre membre, Bogus peut générer des emprunts en retard random pour un utilisateur donné 

Il y avait des nuget dans le template, comme Bogus qui permet de faker des données et MediatR gérer le lien entre les endpoint et les handlers. Les deux sont lourds pour une solution si petite, à choisir je ne les aurais pas utilisé. 

Globalement, c'est le bilan que je fais de cette architecture, pour une api de cette taille, rajouter autant de couche est une aberration mais je commence à déceler l'intérêt qu'un tel découplage peut avoir dans une grosse app, donc j'en tire un bilan plutôt positif et j'ai découvert pas mal de points intéressants. 

