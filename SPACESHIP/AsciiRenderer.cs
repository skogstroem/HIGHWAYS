using SPACESHIP.Interfaces;

namespace SPACESHIP.Rendering;

public class AsciiRenderer : IRenderable
{
    private readonly char _symbol;
    private readonly ConsoleColor _color;

    public AsciiRenderer(char symbol, ConsoleColor color = ConsoleColor.White)
    {
        _symbol = symbol;
        _color = color;
    }

    public void Render(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = _color;
        Console.Write(_symbol);
        Console.ResetColor();
    }

    public char GetSymbol() => _symbol;
    public ConsoleColor GetColor() => _color;
}

