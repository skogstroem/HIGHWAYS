namespace HIGHWAYS.Interfaces;

public interface IRender
{
    void Render(int x, int y);
    char GetSymbol();
    ConsoleColor GetColor();
}

