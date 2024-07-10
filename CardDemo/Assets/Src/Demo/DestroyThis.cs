using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float waitTime = 3;
    private float nowWaitTime = 0;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        nowWaitTime += Time.deltaTime;
        if (nowWaitTime>=waitTime)
        {
            Destroy(this.gameObject);
        }
    }
}
