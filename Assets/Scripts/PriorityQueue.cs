
using System.Collections.Generic;

// Priority Queue Implementation (Simple version for demonstration purposes)
public class PriorityQueue<T> where T : Node
{
    private List<T> elements = new List<T>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item)
    {
        elements.Add(item);
        elements.Sort((a, b) => a.distance.CompareTo(b.distance));
    }

    public T Dequeue()
    {
        T firstItem = elements[0];
        elements.RemoveAt(0);
        return firstItem;
    }
}