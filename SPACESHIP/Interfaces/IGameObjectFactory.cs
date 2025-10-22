using SPACESHIP.GameObjects;

namespace SPACESHIP.Interfaces;
public interface IGameObjectFactory
{
    GameObject CreateGameObject(int x, int y);
}

