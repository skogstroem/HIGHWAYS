using SPACESHIP.GameObjects;

namespace SPACESHIP.Interfaces;
public interface IMovementStrategy
{
    int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes);
}

