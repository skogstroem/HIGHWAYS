namespace HIGHWAYS.Interfaces;

public interface IRenderable
{
    void Render(int x, int y);
    char GetSymbol();
    ConsoleColor GetColor();
}

