using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Попытаться получить компонент у текущего объекта или его родителей
    /// </summary>
    /// <typeparam name="T">Тип компонента</typeparam>
    /// <param name="gameObject">Целевой объект</param>
    /// <param name="component">Найденный компонент (null если не найден)</param>
    /// <param name="includeInactive">Включать неактивные объекты в поиск</param>
    /// <returns>True если компонент найден</returns>
    public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component, bool includeInactive = false) where T : Component
    {
        component = gameObject.GetComponentInParent<T>(includeInactive);
        return component != null;
    }

    /// <summary>
    /// Попытаться получить компонент у текущего объекта или его родителей
    /// </summary>
    /// <typeparam name="T">Тип компонента</typeparam>
    /// <param name="component">Целевой компонент</param>
    /// <param name="component">Найденный компонент (null если не найден)</param>
    /// <param name="includeInactive">Включать неактивные объекты в поиск</param>
    /// <returns>True если компонент найден</returns>
    public static bool TryGetComponentInParent<T>(this Component component, out T foundComponent, bool includeInactive = false) where T : Component
    {
        foundComponent = component.GetComponentInParent<T>(includeInactive);
        return foundComponent != null;
    }

     public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeInactive = false) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>(includeInactive);
        return component != null;
    }

    /// <summary>
    /// Попытаться получить компонент у текущего объекта или его родителей
    /// </summary>
    /// <typeparam name="T">Тип компонента</typeparam>
    /// <param name="component">Целевой компонент</param>
    /// <param name="component">Найденный компонент (null если не найден)</param>
    /// <param name="includeInactive">Включать неактивные объекты в поиск</param>
    /// <returns>True если компонент найден</returns>
    public static bool TryGetComponentInChildren<T>(this Component component, out T foundComponent, bool includeInactive = false) where T : Component
    {
        foundComponent = component.GetComponentInChildren<T>(includeInactive);
        return foundComponent != null;
    }
    
    public static bool TryGetInterface<T>(this GameObject gameObject, out T foundComponent) where T : class
    {
        foundComponent = gameObject.GetInterface<T>();
        return foundComponent != null;
    }

    public static T GetInterface<T>(this GameObject gameObject) where T : class
    {
        return gameObject.GetComponent(typeof(T)) as T;
    }

    public static T[] GetInterfaces<T>(this GameObject gameObject) where T : class
    {
        return gameObject.GetComponents(typeof(T)) as T[];
    }

    public static T GetInterfaceInChildren<T>(this GameObject gameObject) where T : class
    {
        return gameObject.GetComponentInChildren(typeof(T)) as T;
    }

    public static T[] GetInterfacesInChildren<T>(this GameObject gameObject) where T : class
    {
        return gameObject.GetComponentsInChildren(typeof(T)) as T[];
    }

    public static T GetInterfaceInParent<T>(this GameObject gameObject) where T : class
    {
        return gameObject.GetComponentInParent(typeof(T)) as T;
    }
}