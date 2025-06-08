using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : IObject // 풀링할 오브젝트라서 상속
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
                    StartCoroutine(ChangePlayerSetting(player_control));
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

    IEnumerator ChangePlayerSetting(PlayerControl player)
    {
        
        player.SetTrapped(true);      // 트랩에 걸림
        player.MoveSpeed = 0f;       
        yield return new WaitForSeconds(2f);
        player.SetTrapped(false);     // 트랩 해제

    }

    public override void OnEnter()
    {
        // 예: 트랩이 생성될 때 초기화
    }

    public override void OnExit()
    {
        // 예: 트랩이 삭제될 때 정리
    }

    public override void OnInit()
    {
        // 예: 트랩이 풀에서 재사용될 때 초기화
        isInTrap = false;
        Vector3 pos = this.transform.position;
        pos.y = 2.3f;

        waitTime = new WaitForSeconds(1);
        this.transform.position = pos;
    }

    public override void OnDisabled()
    {
        // 예: 트랩이 비활성화될 때 수행할 작업
    }

    // Start is called before the first frame update
    void Start()
    {
        isInTrap = false;
        Vector3 pos = this.transform.position;
        pos.y = 2.3f;
        this.transform.position = pos;

        waitTime = new WaitForSeconds(1);
        Event_Area = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
