using System.Collections.Generic;
using UnityEngine;
public interface IObserver
{

}

public interface IObserver<T> : IObserver
{
    public void OnNotify(T inData_1);
}

public interface IObserver<T1,T2> : IObserver
{
    public void OnNotify(T1 inData_1, T2 inData_2);
}

