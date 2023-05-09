[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/lW1QFBff)



![task](./task.png) Denk er aan dat de services draaien binnen een docker container. De web api draait binnen de docker container op `http://api:80`. De web app draait binnen de container op `http://web:80`. Betekent dit dat request eigenlijk gestuurd wordt van `http://web:80` naar `http://api:80`? Of van `http://localhost:8080` naar `http://api:80`? Of van `http://localhost:8080` naar `http://localhost:5000`?


De request wordt gestuurd van http://web:80 naar http://api:80 binnen de container. Vanuit de host machine zou de request via http://localhost:8080 naar http://web:80 gestuurd worden.

Waarom hebben we deze signature nodig? Wat is deze signature?

De signature is een gecodeerde waarde die de integriteit van de token garandeert. Het zorgt ervoor dat 
de token niet is gewijzigd nadat deze is gemaakt en ondertekend. De signature is het resultaat van het 
coderen van de header en body met behulp van een gedeeld geheim tussen de identity server en de
 resource server (API). De resource server kan de handtekening decoderen en 
controleren om te verifiëren dat de token geldig is en niet is gewijzigd sinds deze is gemaakt.

Waarom staat het `client_secret` niet in de token?

Het client_secret is een geheime waarde die alleen bekend is bij de client en de authorization server. 
Het doel van het client_secret is om te bewijzen dat de client is wie hij beweert te zijn en om te voorkomen 
dat andere partijen toegang krijgen tot de resources van de client. Als het client_secret in de token zou 
worden opgenomen, zou het niet meer geheim zijn en zou het het doel van het client_secret ondermijnen. 
Daarom wordt het client_secret alleen gebruikt bij het opvragen van de token en wordt het niet in de 
token zelf opgenomen.

![task](./task.png) Zou het gebruik van HTTPS ons helpen om de client secret in de web app te verbergen?

Ja, het gebruik van HTTPS zou helpen om de client secret te verbergen, omdat HTTPS de communicatie tussen de web app en de server beveiligt met behulp van versleuteling. Dit betekent dat de client secret niet als platte tekst wordt verzonden en dus niet kan worden onderschept door derden. Het is belangrijk om HTTPS te gebruiken om de veiligheid van de applicatie te waarborgen en om gevoelige informatie zoals wachtwoorden en tokens te beschermen.


![task](./task.png) Welke andere OAuth flow zou hier toepasselijk zijn? Is dit wel een correcte case om OAuth toe te passen?


Als het doel is om een webapplicatie te bouwen waarbij een gebruiker toegang heeft tot zijn of haar eigen gegevens, dan is de authorization_code flow meestal de meest geschikte optie. Hierbij wordt de gebruiker doorverwezen naar de identity server om zich te authentiseren en geeft de identity server toegang tot de gegevens van de gebruiker aan de applicatie. De gebruiker kan dan zelf bepalen welke gegevens hij of zij deelt met de applicatie.

In het geval van deze case lijkt de client_credentials flow echter passend, omdat het doel is om de applicatie toegang te geven tot resources op de server zonder tussenkomst van een gebruiker.