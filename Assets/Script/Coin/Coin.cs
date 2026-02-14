using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : IObject
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameStatus gameStatus = GameObject.Find("GameRoot").GetComponent<GameStatus>();
            if (gameStatus != null)
            {
                gameStatus.coin += 1;
            }
            PoolObject();
        }
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnInit()
    {
        Vector3 pos = this.transform.position;
        pos.y = 2.3f;

        this.transform.position = pos;
    }

    public override void OnDisabled()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
