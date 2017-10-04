using UnityEngine;
using System.Collections;

public class DataObject
{

    public int[] items;
    public int a;
    string b;

    public DataObject()
    {
    }
    public DataObject(int[] objdata, int a)
    {

        this.items = objdata;
        this.a = a;
    }

    public int[] getItems()
    {
        return items;
    }

    public void getA()
    {
        this.a = a;
    }

    public string getB()
    {
        return b;
    }
}
