using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueNPC : NPCRoot
{
    [SerializeField] bool isRescued = false;

    void Update()
    {
        if(this.Gauge >=1.0f)
        {
            isRescued = true;
        }
    }
}
