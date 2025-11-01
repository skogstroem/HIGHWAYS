using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;
using HIGHWAYS.Players;

namespace HIGHWAYS;

public class RandomStrategy : IStrategy
{
    
    public int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes)
    {
        Random randomLane = new Random();
        return randomLane.Next(0, maxLanes);
    }
}