using System.Collections;
using HIGHWAYS.Generics;

namespace HIGHWAYS.GameObjects;
public class Lane : IEnumerable<GameObject>
{
    private readonly ObjectBuffer<GameObject> _objects;
    public int LaneNumber { get; }
    public int XPosition { get; }

    public Lane(int laneNumber, int xPosition, int capacity = 50000)
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

    // KRAV #5:
    // 1: Enumerable och enumerator 
    // 2: Vi implenterar  GetEnumerator() som returnerar en enumerator över enbart de aktiva
    //    objekten som finns i den aktuella lane:en. På så sätt döljer vi den interna strukturen
    //    av objekten och ger en skapar en kontrollerad iteration. 
    // 3: Vi använder konceptet dels för att det gör koden renare och tydligare, men också för att det tillåter 
    //    andra klasser att traversera över lanes:en och utföra operationer på objekten
    //    som uppdaterar deras tillstånd exempelvis kollisoner mellan spelare och objekt.

    public IEnumerator<GameObject> GetEnumerator()
    {
        // använd GetEnumerator() från IEnumerable istället för att yielda för att uppfylla kravet korrekt
        return _objects.GetAll()
            .Where(obj => obj.IsActive)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

