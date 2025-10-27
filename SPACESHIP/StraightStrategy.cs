using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Movement;

public class StraightStrategy : IMovementStrategy
{
    public int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes)
    {
        // Kör rakt fram - rör sig aldrig från nuvarande lane
        return currentLane;
    }
}

