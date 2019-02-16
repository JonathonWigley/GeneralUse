using UnityEngine;

public abstract class DatabaseDataReference<T> : ScriptableObject where T : DatabaseData
{
    public T data;
}