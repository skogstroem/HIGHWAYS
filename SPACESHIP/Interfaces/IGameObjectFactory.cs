using HIGHWAYS.GameObjects;

namespace HIGHWAYS.Interfaces;
public interface IGameObjectFactory
{
    GameObject CreateGameObject(int x, int y);
}

