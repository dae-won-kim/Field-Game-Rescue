using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static float MOVE_AREA_RADIUS = 20.0f; // 섬의 반지름.
    public float MoveSpeed = 7.0f; // 이동 속도.
    public bool isTrapped = false;

    private struct Key
    { // 키 조작 정보 구조체.
        public bool up; // ↑.
        public bool down; // ↓.
        public bool right; // →.
        public bool left; // ←.
        public bool pick; // 줍는다／버린다.
        public bool action; // 먹는다 / 수리한다.
    };

    private Key key; // 키 조작 정보를 보관하는 변수.

    public enum STEP
    { // 플레이어의 상태를 나타내는 열거체.
        NONE = -1, // 상태 정보 없음.
        MOVE = 0, // 이동 중.
        REPAIRING, // 수리 중.
        EATING, // 식사 중.
        EMOTION, // 감정 관련 처리
        RESCUE,  // 구조자 치료 관련 처리
        NUM, // 상태가 몇 종류 있는지 나타낸다(=3).
    };
    public STEP step = STEP.NONE; // 현재 상태.
    public STEP next_step = STEP.NONE; // 다음 상태.
    public float step_timer = 0.0f; // 타이머.
                                    // Use this for initialization

    // 다음 네 개의 멤버 변수를 PlayerControl class에 추가.
    private GameObject closest_item = null; // 플레이어의 정면에 있는 GameObject.
    private GameObject carried_item = null; // 플레이어가 들어올린 GameObject.
    private ItemRoot item_root = null; // ItemRoot 스크립트를 가짐.
    public GUIStyle guistyle; // 폰트 스타일.

    private GameObject closest_event = null;// 주목하고 있는 이벤트를 저장
    private EventRoot event_root = null; // EventRoot 클래스를 사용
    private GameObject rocket_model = null; // 우주선의 모델을 사용

    [SerializeField] RescueNPC rescueNPC = null;
    private GameStatus game_status = null;

    private Animator animator;
    private GameObject FeverEffect;

    public void SetTrapped(bool value)
    {
        isTrapped = value;
        if (value) MoveSpeed = 0f;
    }
    private void ChangeMoveSpeed()
    {
        if (GameStatus.IsFeverTime) return;

        if (rescueNPC.isRescued && !GameStatus.IsFeverTime && !rescueNPC.feverTimeTriggered)
        {
            StartCoroutine(FeverTime());
        }
        else if (isTrapped)
        {
            animator.Play("05_died");
            return;
        }
        else
        {
            if (game_status.emotion <= 0.4f)
                MoveSpeed = 7.0f;
            else if (game_status.emotion <= 0.65f)
                MoveSpeed = 6.0f;
            else if (game_status.emotion <= 0.8f)
                MoveSpeed = 5.0f;
            else
                MoveSpeed = 4.5f;
        }
    }

    private void get_input()
    {
        this.key.up = false;
        this.key.down = false;
        this.key.right = false;
        this.key.left = false;
        // ↑키가 눌렸으면 true를 대입.
        this.key.up |= Input.GetKey(KeyCode.UpArrow);
        this.key.up |= Input.GetKey(KeyCode.Keypad8);

        // ↓키가 눌렸으면 true를 대입.
        this.key.down |= Input.GetKey(KeyCode.DownArrow);
        this.key.down |= Input.GetKey(KeyCode.Keypad2);

        // →키가 눌렸으면 true를 대입.
        this.key.right |= Input.GetKey(KeyCode.RightArrow);
        this.key.right |= Input.GetKey(KeyCode.Keypad6);

        // ←키가 눌렸으면 true를 대입..
        this.key.left |= Input.GetKey(KeyCode.LeftArrow);
        this.key.left |= Input.GetKey(KeyCode.Keypad4);

        // Z 키가 눌렸으면 true를 대입.
        this.key.pick = Input.GetKeyDown(KeyCode.Z);

        // X 키가 눌렸으면 true를 대입.
        this.key.action = Input.GetKeyDown(KeyCode.X);
    }
    private void move_control()
    {
        Vector3 move_vector = Vector3.zero;
        Vector3 position = this.transform.position;
        bool is_moved = false;

        // 방향키 입력
        if (this.key.right)
        {
            move_vector += Vector3.right;
            is_moved = true;
        }
        if (this.key.left)
        {
            move_vector += Vector3.left;
            is_moved = true;
        }
        if (this.key.up)
        {
            move_vector += Vector3.forward;
            is_moved = true;
        }
        if (this.key.down)
        {
            move_vector += Vector3.back;
            is_moved = true;
        }

        ChangeMoveSpeed();

        if (is_moved && !GameStatus.IsFeverTime)
        {
            float consume = this.item_root.getConsumeSatiety(this.carried_item);
            this.game_status.addSatiety(-consume * Time.deltaTime);
        }

        // 애니메이션 제어 (중복 방지)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (is_moved)
        {
            if (!stateInfo.IsName("02_Move"))
            {
                animator.Play("02_Move");
            }
        }
        else
        {
            if (!stateInfo.IsName("01_Idle"))
            {
                animator.Play("01_Idle");
            }
        }

        move_vector.Normalize();
        move_vector *= MoveSpeed * Time.deltaTime;
        position += move_vector;
        position.y = 0.0f;

        if (position.magnitude > MOVE_AREA_RADIUS)
        {
            position.Normalize();
            position *= MOVE_AREA_RADIUS;
        }
        position.y = this.transform.position.y;
        this.transform.position = position;

        if (is_moved)
        {
            Quaternion q = Quaternion.LookRotation(move_vector, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, 0.2f);
        }
    }


    private IEnumerator FeverTime()
    {
        rescueNPC.feverTimeTriggered = true;
        AudioController.Instance?.PlayerFeverTime();
        GameStatus.StartFeverTime();   // GameState 변경

        float originalSpeed = MoveSpeed;
        float originalSatiety = this.game_status.satiety;
        float originalEmotion = this.game_status.emotion;
        FeverEffect.SetActive(true);

        MoveSpeed = 10.0f;
        game_status.satiety = 1f;
        game_status.emotion = 0f;

        yield return new WaitForSeconds(7f);

        FeverEffect.SetActive(false);
        MoveSpeed = originalSpeed; // 속도 복원
        this.game_status.satiety = originalSatiety; // 배고픔 복원
        this.game_status.emotion = originalEmotion; // emotion수치 복원
        GameStatus.EndFeverTime();  // GameState 변경

    }

    // 물건을 줍거나 떨어뜨린다.
    private void pick_or_drop_control()
    {
        do
        {
            if (!this.key.pick)
            { // '줍기/버리기'키가 눌리지 않았으면.
                break; // 아무것도 하지 않고 메소드 종료.
            }
            if (this.carried_item == null)
            { // 들고 있는 아이템이 없고.
                if (this.closest_item == null)
                {// 주목 중인 아이템이 없으면.
                    break; // 아무것도 하지 않고 메소드 종료.
                }
                // 주목 중인 아이템을 들어올린다.
                this.carried_item = this.closest_item;

                // 들고 있는 아이템을 자신의 자식으로 설정.
                this.carried_item.transform.parent = this.transform;

                // 2.0f 위에 배치(머리 위로 이동).
                this.carried_item.transform.localPosition = Vector3.up * 2.0f;

                // 주목 중 아이템을 없앤다.
                this.closest_item = null;
                AudioController.Instance?.PlayerPickUp();
            }
            else
            { // 들고 있는 아이템이 있을 경우.
              // 들고 있는 아이템을 약간(1.0f) 앞으로 이동시켜서.
                this.carried_item.transform.localPosition = Vector3.forward * 1.0f;
                this.carried_item.transform.parent = null; // 자식 설정을 해제.
                this.carried_item = null; // 들고 있던 아이템을 없앤다.
                AudioController.Instance?.PlayerPut();
            }
        } while (false);
    }

    // 애니메이션 실패
    /*
    // 물건을 줍거나 떨어뜨린다.
    private void pick_or_drop_control()
    {
        do
        {
            if (!this.key.pick)
            {
                // '줍기/버리기'키가 눌리지 않았으면 아무것도 하지 않고 메소드 종료.
                break;
            }

            if (this.carried_item == null)
            {
                // 들고 있는 아이템이 없을 경우.
                if (this.closest_item == null)
                {
                    // 주목 중인 아이템이 없으면 아무것도 하지 않고 종료.
                    break;
                }
                // Coroutine을 시작하여 pick 애니메이션을 재생하고 끝나기를 기다림.
                StartCoroutine(PickItemCoroutine());
            }
            else
            {
                // 들고 있는 아이템이 있을 경우, put 애니메이션과 함께 내려놓기.
                StartCoroutine(PutItemCoroutine());
            }
        } while (false);
    }

    // 아이템을 줍는 애니메이션과 행동을 처리하는 코루틴
    private IEnumerator PickItemCoroutine()
    {
        animator.SetTrigger("PickTrigger");
        // 아이템을 들어올림
        this.carried_item = this.closest_item;

        // 들고 있는 아이템을 자신의 자식으로 설정
        this.carried_item.transform.parent = this.transform;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("09_pickup"));
      

        // 2.0f 위에 배치 (머리 위로 이동)
        this.carried_item.transform.localPosition = Vector3.up * 2.0f;

        // 주목 중 아이템을 없앰
        this.closest_item = null;


        // 해당 애니메이션의 길이만큼 대기
        float animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animDuration);

        
    }

    // 아이템을 내려놓는 애니메이션과 행동을 처리하는 코루틴
    private IEnumerator PutItemCoroutine()
    {
        animator.SetTrigger("PutTrigger");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("10_put"));

        // 아이템을 약간 앞으로 이동
        this.carried_item.transform.localPosition = Vector3.forward * 1.0f;

        // 자식 설정을 해제
        this.carried_item.transform.parent = null;

        // 들고 있던 아이템을 없앰
        this.carried_item = null;


        // 해당 애니메이션의 길이만큼 대기
        float animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animDuration);
    }
    */


    // 접촉한 물건이 자신의 정면에 있는지 판단한다.
    private bool is_other_in_view(GameObject other)
    {
        bool ret = false;
        do
        {
            Vector3 heading = // 자신이 현재 향하고 있는 방향을 보관.
           this.transform.TransformDirection(Vector3.forward);
            Vector3 to_other = // 자신 쪽에서 본 아이템의 방향을 보관.
           other.transform.position - this.transform.position;
            heading.y = 0.0f;
            to_other.y = 0.0f;
            heading.Normalize(); // 길이를 1로 하고 방향만 벡터로.
            to_other.Normalize(); // 길이를 1로 하고 방향만 벡터로.
            float dp = Vector3.Dot(heading, to_other); // 양쪽 벡터의 내적을 취득.
            if (dp < Mathf.Cos(45.0f)) // 내적이 45도인 코사인 값 미만이면.
            {
                break; // 루프를 빠져나간다.
            }
            ret = true; // 내적이 45도인 코사인 값 이상이면 정면에 있다.
        } while (false);
        return (ret);
    }

    // 들고 있는 아이템의 종류와 주목하는 이벤트의 종류를 보고 이벤트 시작
    private bool is_event_ignitable()
    {
        bool ret = false;
        do
        {
            if (this.closest_event == null)
            { // 주목 이벤트가 없으면.
                break; // false를 반환한다.
            }
            // 들고 있는 아이템 종류를 가져온다.
            Item.TYPE carried_item_type =
            this.item_root.getItemType(this.carried_item);

            // 들고 있는 아이템 종류와 주목하는 이벤트의 종류에서.
            // 이벤트가 가능한지 판정하고, 이벤트 불가라면 false를 반환한다.
            if (!this.event_root.isEventIgnitable(
            carried_item_type, this.closest_event))
            {
                break;
            }
            ret = true; // 여기까지 오면 이벤트를 시작할 수 있다고 판정!.
        } while (false);
        return (ret);
    }


    // 입력 정보를 가져오고 상태에 변화가 있을 때의 처리를 거쳐 각 상태별로 실행.
    // 트리거에 걸린 게임 오브젝트가 Item 레이어에 설정되어 있고,
    // 플레이어의 정면에 있을 때, 그 게임 오브젝트를 주목하게 한다.
    void OnTriggerStay(Collider other)
    {
        GameObject other_go = other.gameObject;
        // 트리거의 GameObject 레이어 설정이 Item이라면.
        if (other_go.layer == LayerMask.NameToLayer("Item"))
        {
            // 아무 것도 주목하고 있지 않으면.
            if (this.closest_item == null)
            {
                if (this.is_other_in_view(other_go))
                { // 정면에 있으면.
                    this.closest_item = other_go; // 주목한다.
                }
            }

            // 뭔가 주목하고 있으면.
            else if (this.closest_item == other_go)
            {
                if (!this.is_other_in_view(other_go))
                { // 정면에 없으면.
                    this.closest_item = null; // 주목을 그만둔다.
                }
            }
        }
        // 트리거의 GameObject의 레이어 설정이 Event라면.
        else if (other_go.layer == LayerMask.NameToLayer("Event"))
        {
            // 아무것도 주목하고 있지 않으면.
            if (this.closest_event == null)
            {
                if (this.is_other_in_view(other_go))
                {// 정면에 있으면
                    this.closest_event = other_go; // 주목한다.
                }
                // 뭔가에 주목하고 있으면.
            }
            else if (this.closest_event == other_go)
            {
                if (!this.is_other_in_view(other_go))
                {// 정면에 없으면
                    this.closest_event = null; // 주목을 그만둔다.
                }
            }
        }
        else if (other_go.tag == "coin")
        {
            this.game_status.coin += 1;
            other_go.SetActive(false);
        }
    }

    // 주목을 그만두게 한다.
    void OnTriggerExit(Collider other)
    {
        if (this.closest_item == other.gameObject)
        {
            this.closest_item = null; // 주목을 그만둔다.
        }
    }

    // 주목 중이거나 들고 있는 아이템이 있을 때 표시
    void OnGUI()
    {
        float x = 20.0f;
        float y = Screen.height - 40.0f;
        if (this.is_event_ignitable()) // 이벤트가 시작 가능한 경우.
        {
            // 이벤트용 메시지를 취득.
            string message =
            this.event_root.getIgnitableMessage(this.closest_event);
            GUI.Label(new Rect(x + 100.0f, y, 200.0f, 20.0f),
            "X:" + message, guistyle);
        }
        // 들고 있는 아이템이 있다면.
        if (this.carried_item != null)
        {
            GUI.Label(new Rect(x, y, 200.0f, 20.0f), "Z:버린다", guistyle);
            // 아이템 종류에 따라 메세지를 나눔.
            if (this.carried_item.tag == "Stress")
            {
                GUI.Label(new Rect(x + 100.0f, y, 200.0f, 20.0f), "X:해소한다", guistyle);
            }
            else if (this.carried_item.tag == "Heal")
            {
                GUI.Label(new Rect(x + 100.0f, y, 200.0f, 20.0f), "X:구출한다", guistyle);
            }
            else if (this.carried_item.tag == "Iron")
                GUI.Label(new Rect(x + 100.0f, y, 200.0f, 20.0f), "", guistyle);
        }
        else
        {
            // 주목하고 있는 아이템이 있다면.
            if (this.closest_item != null)
            {
                GUI.Label(new Rect(x, y, 200.0f, 20.0f), "Z:줍는다", guistyle);
            }
        }
        guistyle.fontSize = 24;
        switch (this.step)
        {
            case STEP.EATING:
                GUI.Label(new Rect(x, y, 200.0f, 20.0f),
                 "우적우적우물우물……", guistyle);
                break;
            case STEP.EMOTION:
                GUI.Label(new Rect(x, y, 200.0f, 20.0f),
                 "습하...습하....", guistyle);
                break;
            case STEP.RESCUE:
                GUI.Label(new Rect(x, y, 200.0f, 20.0f),
                 "구해드릴께요!", guistyle);
                break;
            case STEP.REPAIRING:
                GUI.Label(new Rect(x + 200.0f, y, 200.0f, 20.0f), "수리중", guistyle);
                break;
        }

    }

    void Start()
    {
        this.step = STEP.NONE; // 현 단계 상태를 초기화.
        this.next_step = STEP.MOVE; // 다음 단계 상태를 초기화.
        this.item_root = GameObject.Find("GameRoot").GetComponent<ItemRoot>();
        this.guistyle.fontSize = 16;

        this.event_root =
        GameObject.Find("GameRoot").GetComponent<EventRoot>();
        this.rocket_model = GameObject.Find("rocket").transform.Find("rocket_model").gameObject;
        this.game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();

        this.rescueNPC = GameObject.Find("RescueNPC").GetComponentInChildren<RescueNPC>();

        animator = GetComponentInChildren<Animator>();
        FeverEffect = GameObject.Find("MuzzleFlash");
        FeverEffect.SetActive(false);

    }

    void Update()
    {
        this.get_input(); // 입력 정보 취득.
                          // 상태가 변화했을 때------------.

        this.step_timer += Time.deltaTime;
        float eat_time = 1.0f;
        float repair_time = 1.0f;

        float stress_time = 1.5f;
        float rescue_time = 2.0f;

        // 상태를 변화시킨다---------------------.
        if (this.next_step == STEP.NONE)
        { // 다음 예정이 없으면.
            switch (this.step)
            {
                case STEP.MOVE: // '이동 중' 상태의 처리.
                    do
                    {
                        if (!this.key.action)
                        { // 액션 키가 눌려있지 않다.
                            break; // 루프 탈출.
                        }
                        // 주목하는 이벤트가 있을 때.
                        if (this.closest_event != null)
                        {
                            if (!this.is_event_ignitable())
                            { // 이벤트를 시작할 수 없으면.
                                break; // 아무 것도 하지 않는다.
                            }
                            // 이벤트 종류를 가져온다.
                            Event.TYPE ignitable_event =
                            this.event_root.getEventType(this.closest_event);
                            switch (ignitable_event)
                            {
                                case Event.TYPE.ROCKET: // 이벤트의 종류가 ROCKET이면.
                                                        // REPAIRING(수리) 상태로 이행.
                                    this.next_step = STEP.REPAIRING;
                                    break;
                                case Event.TYPE.RESCUE:
                                    this.next_step = STEP.RESCUE;
                                    break;
                            }
                            break;
                        }
                        if (this.carried_item != null)
                        {
                            // 가지고 있는 아이템 판별.
                            Item.TYPE carried_item_type =
                            this.item_root.getItemType(this.carried_item);
                            switch (carried_item_type)
                            {
                                case Item.TYPE.APPLE: // 사과라면.
                                    this.next_step = STEP.EATING; // ＇식사 중＇ 상태로 이행.
                                    break;
                                case Item.TYPE.PLANT: // 식물이라면.
                                    this.next_step = STEP.EATING; // ＇식사 중＇ 상태로 이행.
                                    break;
                                case Item.TYPE.STRESS: // 스트레스 아이템 이라면
                                    this.next_step = STEP.EMOTION; // ＇감정＇ 상태로 이행.
                                    break;
                            }
                        }
                    } while (false);
                    break;

                case STEP.EATING: // '식사 중' 상태의 처리.
                    if (this.step_timer > eat_time)
                    {
                        this.next_step = STEP.MOVE; // '이동' 상태로 이행.
                    }
                    break;
                case STEP.EMOTION: // '감정' 상태의 처리.
                    if (this.step_timer > stress_time)
                    {
                        this.next_step = STEP.MOVE; // '이동' 상태로 이행.
                    }
                    break;
                case STEP.RESCUE: // '구조' 상태의 처리.
                    if (this.step_timer > rescue_time)
                    {
                        this.next_step = STEP.MOVE; // '이동' 상태로 이행.
                    }
                    break;
                case STEP.REPAIRING: // '수리 중' 상태의 처리.
                    if (this.step_timer > repair_time)
                    {
                        this.next_step = STEP.MOVE; // '이동' 상태로 이행.
                    }
                    break;
            }
        }

        // 상태가 변화했을 때------------.
        while (this.next_step != STEP.NONE)
        {
            this.step = this.next_step;
            this.next_step = STEP.NONE;
            switch (this.step)
            {
                case STEP.MOVE:
                    break;
                case STEP.EATING:
                    if (this.carried_item != null)
                    {
                        AudioController.Instance?.PlayerEating();

                        // 들고 있는 아이템의 '체력 회복 정도'를 가져와서 설정.
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        if (!stateInfo.IsName("08_eat"))
                        {
                            animator.Play("08_eat");
                        }
                        this.game_status.addSatiety(this.item_root.getRegainSatiety(this.carried_item));

                        GameObject.Destroy(this.carried_item);
                        this.carried_item = null;
                    }
                    break;
                case STEP.EMOTION:
                    if (this.carried_item != null)
                    {
                        // 스트레스 수치 낮추기
                        AudioController.Instance?.PlayerStressDown();
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        if (!stateInfo.IsName("08_eat"))
                        {
                            animator.Play("08_eat");
                        }
                        this.game_status.subtractEmotion(this.item_root.getRegainEmotion(this.carried_item));

                        GameObject.Destroy(this.carried_item);
                        this.carried_item = null;
                    }
                    break;
                case STEP.RESCUE:
                    if (this.carried_item != null)
                    {
                        // NPC의 게이지 채우기
                        AudioController.Instance?.PlayerHealing();
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        if (!stateInfo.IsName("07_repair"))
                        {
                            animator.Play("07_repair");
                        }
                        // this.RescueNPC.addGauge(this.item_root.getRegainNPCGauge(this.carried_item));
                        this.rescueNPC.addGauge(0.2f);

                        GameObject.Destroy(this.carried_item);
                        this.carried_item = null;
                        this.closest_item = null;
                    }
                    break;

                case STEP.REPAIRING: // ‘수리 중’이 되면.
                    if (this.carried_item != null)
                    {
                        // 들고 있는 아이템의 '수리 진척 상태'를 가져와서 설정.
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        if (!stateInfo.IsName("07_repair"))
                        {
                            animator.Play("07_repair");
                        }
                        AudioController.Instance?.PlayerRepairing();

                        this.game_status.addRepairment(this.item_root.getGainRepairment(this.carried_item));

                        GameObject.Destroy(this.carried_item);
                        this.carried_item = null;
                        this.closest_item = null;
                    }
                    break;
            }
            this.step_timer = 0.0f;
        }
        // 각 상황에서 반복할 것----------.
        switch (this.step)
        {
            case STEP.MOVE:
                this.move_control();
                this.pick_or_drop_control();

                if (!GameStatus.IsFeverTime)
                {
                    // 이동 가능한 경우는 항상 배가 고파진다.
                    this.game_status.alwaysSatiety();
                    this.game_status.alwaysEmotion();
                }

                break;
            case STEP.REPAIRING:
                // 우주선을 회전시킨다.
                this.rocket_model.transform.localRotation *=
                Quaternion.AngleAxis(360.0f / 10.0f * Time.deltaTime, Vector3.up);
                break;
        }
    }


}

