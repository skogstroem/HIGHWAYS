namespace SPACESHIP.Generics;

public class ObjectBuffer<T> where T : class
{
    private readonly T[] _buffer;
    private int _writeIndex;
    private int _count;

    public int Count => _count;
    public int Capacity => _buffer.Length;

    public ObjectBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Kapacitet lär ju vara större än 0", nameof(capacity));

        _buffer = new T[capacity];
        _writeIndex = 0;
        _count = 0;
    }

    public bool TryAdd(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (_count >= _buffer.Length)
            return false;

        _buffer[_writeIndex] = item;
        _writeIndex = (_writeIndex + 1) % _buffer.Length;
        _count++;
        return true;
    }

    public IEnumerable<T> GetAll()
    {
        for (int i = 0; i < _buffer.Length; i++)
        {
            if (_buffer[i] != null)
                yield return _buffer[i];
        }
    }

    public void RemoveWhere(Predicate<T> predicate)
    {
        for (int i = 0; i < _buffer.Length; i++)
        {
            if (_buffer[i] != null && predicate(_buffer[i]))
            {
                _buffer[i] = null!;
                _count--;
            }
        }
    }
    public void Clear()
    {
        Array.Clear(_buffer, 0, _buffer.Length);
        _count = 0;
        _writeIndex = 0;
    }
}

