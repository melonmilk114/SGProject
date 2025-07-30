using System.Collections.Generic;
using UnityEngine;

public class DataUpdateFunc<T>
{
    public List<IDataUpdateFunc<T>> funcList = new List<IDataUpdateFunc<T>>();

    public void RemoveListener(IDataUpdateFunc inListener)
    {
        if (inListener is IDataUpdateFunc<T>)
        {
            funcList.RemoveAll(inData => inData.GetHashCode() == inListener.GetHashCode());
        }
    }

    public void AddListener(IDataUpdateFunc inListener)
    {
        if (inListener is IDataUpdateFunc<T> data)
        {
            var findObj = funcList.Find(inData => inData.GetHashCode() == inListener.GetHashCode());
            if (findObj != null)
                return;
            funcList.Add(data);
        }
    }

    public void UpdateData(T inData_1)
    {
        funcList.ForEach(inItem =>
        {
            inItem?.IDataUpdateFunc(inData_1);
        });
    }
}