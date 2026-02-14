using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : IObject
{

    private GameStatus _gameStatus;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_gameStatus != null)
            {
                _gameStatus.coin += 1;
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
        if (_gameStatus == null)
        {
            GameObject root = GameObject.Find("GameRoot");
            if (root != null) _gameStatus = root.GetComponent<GameStatus>();
        }

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
