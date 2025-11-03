using HIGHWAYS.GameObjects;
using HIGHWAYS.Players;

namespace HIGHWAYS.Interfaces;
// KRAV #2:
// 1: Strategy Pattern.
// 2: Vi använder strategyPattern för att implementera olika beteenden för spel i AI-läget.
// 3: Vi låter användaren välja beteende under runtime och instansen injiceras i AI-player konstruktorn.
public interface IStrategy
{
    int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes);
}

