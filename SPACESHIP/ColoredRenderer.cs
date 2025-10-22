using SPACESHIP.Interfaces;

namespace SPACESHIP.Rendering;

public class ColoredRenderer : IRenderable
{
    private readonly char _symbol;
    private readonly ConsoleColor _foregroundColor;
    private readonly ConsoleColor _backgroundColor;

    public ColoredRenderer(char symbol, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        _symbol = symbol;
        _foregroundColor = foregroundColor;
        _backgroundColor = backgroundColor;
    }

    public void Render(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = _foregroundColor;
        Console.BackgroundColor = _backgroundColor;
        Console.Write(_symbol);
        Console.ResetColor();
    }

    public char GetSymbol() => _symbol;
    public ConsoleColor GetColor() => _foregroundColor;
}


