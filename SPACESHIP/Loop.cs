namespace HIGHWAYS.Core;

public class Loop
{
    private readonly Game _game;
    private bool _isRunning;
    private const int LoopDelay = 50; // delayen i threads mellan varje loop

    public Loop(Game game)
    {
        _game = game;
        _isRunning = false;
    }

    public void Start()
    {
        _isRunning = true;
        Console.Clear();
        _game.DrawInitialFrame();
        Run();
    }

    private void Run()
    {
        while (_isRunning && !_game.IsGameOver())
        {
            _game.Update();
            _game.Render();
            
            Thread.Sleep(LoopDelay);
        }

        _game.ShowGameOver();
    }

    public void Stop()
    {
        _isRunning = false;
    }
}

