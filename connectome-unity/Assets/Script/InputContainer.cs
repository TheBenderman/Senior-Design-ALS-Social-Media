using UnityEngine;
using System.Collections;

public class InputContainer : MonoBehaviour
{
    private InputGenerator _inputGenerator; 

    public InputGenerator inputGenerator
    {
        get
        {
           return  _inputGenerator ?? (_inputGenerator = new InputGenerator()); 
        }
    }


    public void Yes()
    {
        if (inputGenerator.OnYes != null)
            inputGenerator.OnYes(); 
    }

    public void No()
    {
        if (inputGenerator.OnNo != null)
            inputGenerator.OnNo(); 
    }




}
