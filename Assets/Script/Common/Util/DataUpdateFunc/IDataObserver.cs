using System.Collections.Generic;
using UnityEngine;
public interface IDataObserver
{

}

public interface IDataObserver<T> : IDataObserver
{
    public void OnDataChanged(T inData_1);
}

