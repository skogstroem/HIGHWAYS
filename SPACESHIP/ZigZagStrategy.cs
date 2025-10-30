using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;
using HIGHWAYS.Players;

namespace HIGHWAYS.Movement;

public class ZigZagStrategy : IMovementStrategy
{
    private bool _movingRight = true;
    private int _moveCounter = 0;
    private readonly int _movesBeforeChange;

    public ZigZagStrategy(int movesBeforeChange = 3)
    {
        _movesBeforeChange = movesBeforeChange;
    }

    public int DecideNextLane(int currentLane, IEnumerable<Lane> lanes, int maxLanes)
    {
        _moveCounter++;

        if (_moveCounter >= _movesBeforeChange)
        {
            _movingRight = !_movingRight;
            _moveCounter = 0;
        }

        int nextLane = currentLane;

        if (_movingRight)
        {
            nextLane = Math.Min(currentLane + 1, maxLanes - 1);
        }
        else
        {
            nextLane = Math.Max(currentLane - 1, 0);
        }

        return nextLane;
    }
}

