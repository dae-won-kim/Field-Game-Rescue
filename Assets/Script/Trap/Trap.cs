using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : IObject 
{
    [SerializeField] Collider Event_Area;
    [SerializeField] bool isInTrap = false;
    [SerializeField] WaitForSeconds waitTime;

    void OnTriggerEnter(Collider other)
    {
       // other -> Collider가 있는 player의 capsule 
        if(other.tag.Contains("Player") && !isInTrap)
        {
            isInTrap = true;
            Transform[] transform = other.transform.GetComponentsInParent<Transform>();
            foreach (Transform t in transform)
            {
                if(t.name == "Player")
                { 
                    PlayerControl player_control = t.GetComponent<PlayerControl>();
                    Debug.Log(player_control.name);
                    StartCoroutine(HandleTrap(player_control));
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player") && isInTrap)
        {
            isInTrap = false;
        }
    }

    private IEnumerator HandleTrap(PlayerControl player)
    {
        player.SetTrapped(true);                 
        yield return new WaitForSeconds(2f);    
        player.SetTrapped(false);                
        // this.gameObject.SetActive(false);        
        PoolObject();
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnInit()
    {
        isInTrap = false;
        Vector3 pos = this.transform.position;
        pos.y = 2.3f;

        waitTime = new WaitForSeconds(1);
        this.transform.position = pos;
    }

    public override void OnDisabled()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        isInTrap = false;
        Vector3 pos = this.transform.position;
        pos.y = 2.3f;
        this.transform.position = pos;

        waitTime = new WaitForSeconds(1);
        Event_Area = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
