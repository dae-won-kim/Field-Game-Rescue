using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Collider Event_Area;
    public bool isInTrap = false;  

    void OnTriggerStay(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            isInTrap = true;
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isInTrap=false;
        Event_Area = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
