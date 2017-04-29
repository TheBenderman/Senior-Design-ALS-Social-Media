using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataInterpreter<T> : MonoBehaviour
{ 
    public abstract void Interpeter(IEnumerable<T> sample);
}
