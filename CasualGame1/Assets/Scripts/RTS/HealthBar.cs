using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");

        if (bar == null)
            Debug.Log("Help");
    }
    
    public void SetSize(float sizeNormal)
    {
        bar.localScale = new Vector3(sizeNormal, 1f);
      //  Debug.Log("Done");
    }
}
