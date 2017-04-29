using Connectome.Core.Common;
using Connectome.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataSampler<T> : MonoBehaviour
{
    [Header("Collection Size (0 for uncapped)")]
    [Range(0, 10000)]
    public int Capacity = DEFAULT_CAPACITY;

    public static int DEFAULT_CAPACITY = 5000;

    public DataInterpreter<T>[] Interpeters;

    public bool IsInterperting { private set; get; }

    protected T[] Data { private set; get; }

    protected int Index { private set; get; }
    protected int Added { private set; get; }

    protected T LastAdded {private set; get;}

    private void Start()
    {
        Clear(); 
    }

    public void Register(T dataElement)
    {
        Data[Index = (Index+1) % Data.Length] = dataElement;

        Added++; 

        LastAdded = dataElement;
    }

    public void Clear()
    {
        IsInterperting = false;
        Index = 0;
        Data = new T[(Capacity == 0) ? int.MaxValue : Capacity];
        IsInterperting = true; 
    }


    public abstract IEnumerable<T> GetSample();
   
}
