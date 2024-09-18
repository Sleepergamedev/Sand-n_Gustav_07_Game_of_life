Beskrivning av projekt:

Mitt game of life består av en grid gjort med hjälp av en matrix. matrixen tar in typen cell, som är en klass jag gjort. inuti klassen finns olika funktioner och variabler som används för att instansiera varje cell, kolla regler samt anropa reglerna när det behövs.
För att kolla levande grannar använder jag mig av två for-loops som går från cellens startvärde samt -1 på x samt y axeln till startvärdet +1.
den kollar dessa celler och ifall de lever plussas den nuvarande cellens lokala "living neighbour" variabel med 1. Sedan efter alla dessa lokala värden har sparats i varje cell så körs alla regler på varje cell.

 Jag har även implementerat max värden för hur stor gridden kan vara, hur snabbt varje generation tickar, hur stor chans det är att varje cell ska leva vid start samt hur stor kameran kan vara. 

