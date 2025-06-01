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
            PlayerControl player_control = other.gameObject.GetComponent<PlayerControl>(); 
            ChangePlayerSetting(player_control);

            // Debug.Log(other.gameObject.name);
            // Destroy(this);
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
        player.setMoveSpeed(0f);
        yield return waitTime;

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
