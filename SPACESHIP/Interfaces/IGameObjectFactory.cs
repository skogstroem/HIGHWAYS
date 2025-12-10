using HIGHWAYS.GameObjects;

namespace HIGHWAYS.Interfaces;


// KRAV #4:
// 1: Factory Method Pattern.
// 2: Vi är i abstraktionen; vi har en hierarki av subtyper (obstacleF./powerupF.) med respektive produkter (supertyp GameObject, subtyp Powerup och Obstacle). Dessa fabriker injiceras i Game-klassen.
//    Vi använder detta koncept för att vid runtime kunna generera nya objekt som dyker upp på spelplanen, eftersom spelplanen är generativ / fortlöpande. Därmed kan vi inte skapa alla objekt i 
//    main klassen, då vi inte vet hur länge en spelare överlever och måste anpassa antalet objekt därefter. 
// 
//   Subtyperna Powerup och Obstacle skiljer sig i beteende eftersom de implementerar metoden HandleStreak() från supertypen (GameObject) på olika sätt. 
//
// 3: Vi g�r detta f�r att kunna kapsla skapandet av olika objekt i spelet som �r r�tt komplext. Genom att anv�nda fabriker har vi enkelt kunna l�gga till nya typer av objekt. Utan detta koncept 
//   hade det varit svårt att veta hur många objekt som skulle skapas i spelet i och med att det är fortlöpanden. Vilket betyder att det fortsätter till spelaren har dött. Så vi kan inte veta på 
//  förhand hur många objekt som behövs och måste därefter generera dem allteftersom. 

public interface IGameObjectFactory
{
    GameObject CreateGameObject(int x, int y);
}

