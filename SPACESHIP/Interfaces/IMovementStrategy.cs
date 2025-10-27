using HIGHWAYS.GameObjects;

namespace HIGHWAYS.Interfaces;
public interface IMovementStrategy
{
    int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes);
}

