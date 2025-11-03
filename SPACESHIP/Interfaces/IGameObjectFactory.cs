using HIGHWAYS.GameObjects;

namespace HIGHWAYS.Interfaces;
// KRAV #4:
// 1: Factory Method Pattern.
// 2: Vi är i abstraktionen; vi har en hierarki av subtyper (obstacleF./powerupF.) med respektive produkter. Dessa fabriker injiceras i Game-klassen.
// 3: Vi gör detta för att kunna kapsla skapandet av olika objekt i spelet som är rätt komplext. Genom att använda fabriker har vi enkelt kunna lägga till nya typer av objekt.
public interface IGameObjectFactory
{
    GameObject CreateGameObject(int x, int y);
}

