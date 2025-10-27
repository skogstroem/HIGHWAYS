namespace HIGHWAYS.Interfaces;

public interface IPlayer
{
    string Name { get; }
    int CurrentLane { get; }
    int Hearts { get; }
    int Score { get; }
    bool IsAlive { get; }

    void MoveToLane(int lane);
    void LoseHeart();
    void GainHeart();
    void IncreaseScore(int points);
    void Update();
    void Render(int yPosition);
}

