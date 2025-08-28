using System.Collections.Generic;
using UnityEngine;

public class Observable<T>
{
    public List<IObserver<T>> observerObjList = new List<IObserver<T>>();

    public void RemoveObserver(IObserver inListener)
    {
        if (inListener is IObserver<T>)
        {
            observerObjList.RemoveAll(inData => inData.GetHashCode() == inListener.GetHashCode());
        }
    }

    public void AddObserver(IObserver inListener)
    {
        if (inListener is IObserver<T> data)
        {
            var findObj = observerObjList.Find(inData => inData.GetHashCode() == inListener.GetHashCode());
            if (findObj != null)
                return;
            observerObjList.Add(data);
        }
    }

    public void NotifyAll(T inData_1)
    {
        observerObjList.ForEach(inItem =>
        {
            inItem?.OnNotify(inData_1);
        });
    }
}

public class Observable<T1, T2>
{
    public List<IObserver<T1, T2>> observerObjList = new List<IObserver<T1, T2>>();

    public void RemoveObserver(IObserver inListener)
    {
        if (inListener is IObserver<T1, T2>)
        {
            observerObjList.RemoveAll(inData => inData.GetHashCode() == inListener.GetHashCode());
        }
    }

    public void AddObserver(IObserver inListener)
    {
        if (inListener is IObserver<T1, T2> data)
        {
            var findObj = observerObjList.Find(inData => inData.GetHashCode() == inListener.GetHashCode());
            if (findObj != null)
                return;
            observerObjList.Add(data);
        }
    }

    public void NotifyAll(T1 inData_1, T2 inData_2)
    {
        observerObjList.ForEach(inItem =>
        {
            inItem?.OnNotify(inData_1, inData_2);
        });
    }
}