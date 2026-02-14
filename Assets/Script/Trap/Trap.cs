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
        if (other.CompareTag("Player") && !isInTrap)
        {
            PlayerControl playerControl = other.GetComponentInParent<PlayerControl>();
            if (playerControl != null)
            {
                isInTrap = true;
                StartCoroutine(HandleTrap(playerControl));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isInTrap)
        {
            isInTrap = false;
        }
    }

    private IEnumerator HandleTrap(PlayerControl player)
    {
        player.SetTrapped(true);
        yield return new WaitForSeconds(2f);
        player.SetTrapped(false);
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
