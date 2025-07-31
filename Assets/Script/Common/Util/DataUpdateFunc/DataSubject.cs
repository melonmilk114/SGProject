using System.Collections.Generic;
using UnityEngine;

public class DataSubject<T>
{
    public List<IDataObserver<T>> observerObjList = new List<IDataObserver<T>>();

    public void RemoveObserverObj(IDataObserver inListener)
    {
        if (inListener is IDataObserver<T>)
        {
            observerObjList.RemoveAll(inData => inData.GetHashCode() == inListener.GetHashCode());
        }
    }

    public void AddObserverObj(IDataObserver inListener)
    {
        if (inListener is IDataObserver<T> data)
        {
            var findObj = observerObjList.Find(inData => inData.GetHashCode() == inListener.GetHashCode());
            if (findObj != null)
                return;
            observerObjList.Add(data);
        }
    }

    public void NotifyObservers(T inData_1)
    {
        observerObjList.ForEach(inItem =>
        {
            inItem?.OnDataChanged(inData_1);
        });
    }
}