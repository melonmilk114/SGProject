using System.Collections.Generic;
using UnityEngine;
public interface IDataUpdateFunc
{

}

public interface IDataUpdateFunc<T> : IDataUpdateFunc
{
    public void IDataUpdateFunc(T inData_1);
}

