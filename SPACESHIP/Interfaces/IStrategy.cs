using HIGHWAYS.GameObjects;
using HIGHWAYS.Players;

namespace HIGHWAYS.Interfaces;
public interface IStrategy
{
    int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes);
}

