using System.Collections;
using HIGHWAYS.Generics;

namespace HIGHWAYS.GameObjects;
public class Lane : IEnumerable<GameObject>
{
    private readonly ObjectBuffer<GameObject> _objects;
    public int LaneNumber { get; }
    public int XPosition { get; }

    public Lane(int laneNumber, int xPosition, int capacity = 50)
    {
        LaneNumber = laneNumber;
        XPosition = xPosition;
        _objects = new ObjectBuffer<GameObject>(capacity);
    }

    public void AddObject(GameObject obj)
    {
        obj.X = XPosition;
        _objects.TryAdd(obj);
    }

    public void RemoveInactiveObjects()
    {
        _objects.RemoveWhere(obj => !obj.IsActive);
    }

    public void RemoveWhere(Predicate<GameObject> predicate)
    {
        _objects.RemoveWhere(predicate);
    }

    public void UpdateAll()
    {
        foreach (var obj in _objects.GetAll())
        {
            obj.Update();
        }
    }

    public void RenderAll()
    {
        foreach (var obj in _objects.GetAll())
        {
            obj.Render();
        }
    }

    public IEnumerable<GameObject> GetActiveObjects()
    {
        return _objects.GetAll().Where(obj => obj.IsActive);
    }

    public IEnumerator<GameObject> GetEnumerator()
    {
        // Använd GetEnumerator() från IEnumerable istället för att yielda
        return _objects.GetAll()
            .Where(obj => obj.IsActive)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

