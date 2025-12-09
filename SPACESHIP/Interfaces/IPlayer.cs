namespace HIGHWAYS.Interfaces;

public interface IPlayer
{
    public string Name { get; set; }
    
    public string HighscoreName { get; set; }
    
    int CurrentLane { get; }
    int Hearts { get; }
    int Score { get; set; }
    bool IsAlive { get; }
    
    int Streak { set; get; }
    public int Difficulty { get; set; }
    
    public int NumberOfPowersUps { get; set; }
    
    void MoveToLane(int lane);
    void LoseHeart();
    void GainHeart();
    void IncreaseScore(int points);
    void Update();
    void Render(int yPosition);
}

