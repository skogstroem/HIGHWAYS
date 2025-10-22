using SPACESHIP.GameObjects;
using SPACESHIP.Interfaces;

namespace SPACESHIP.Movement;

public class StraightStrategy : IMovementStrategy
{
    public int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes)
    {
        // Kör rakt fram - rör sig aldrig från nuvarande lane
        return currentLane;
    }
}

