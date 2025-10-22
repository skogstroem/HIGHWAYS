using System.Diagnostics;

namespace SPACESHIP.Core;

public class GameLoop
{
    private readonly Game _game;
    private readonly Stopwatch _stopwatch;
    private bool _isRunning;
    private const int TargetFPS = 10;
    private const double TargetFrameTime = 1000.0 / TargetFPS;

    public double DeltaTime { get; private set; }
    public int FrameCount { get; private set; }

    public GameLoop(Game game)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _stopwatch = new Stopwatch();
        _isRunning = false;
    }

    public void Start()
    {
        _isRunning = true;
        _stopwatch.Start();
        Console.Clear();
        Console.CursorVisible = false;
        _game.DrawInitialFrame();
        Run();
    }

    private void Run()
    {
        double lastFrameTime = 0;

        while (_isRunning && !_game.IsGameOver())
        {
            double currentTime = _stopwatch.Elapsed.TotalMilliseconds;
            DeltaTime = currentTime - lastFrameTime;

            if (DeltaTime >= TargetFrameTime)
            {
                _game.Update();

                _game.Render();

                lastFrameTime = currentTime;
                FrameCount++;
                           }

                   }

        _game.ShowGameOver();
        Console.CursorVisible = true;
    }

    public void Stop()
    {
        _isRunning = false;
        _stopwatch.Stop();
    }
}

