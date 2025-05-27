using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneControl : MonoBehaviour
{
    private GameStatus game_status = null;
    private PlayerControl player_control = null;

    public enum STEP
    { // 게임 상태.
        NONE = -1, // 상태 정보 없음.
        PLAY = 0, // 플레이 중.
        CLEAR, // 클리어 상태.
        GAMEOVER, // 게임 오버 상태.
        NUM, // 상태가 몇 종류인지 나타낸다(=3).
    };


    public STEP step = STEP.NONE; // 현대 단계.
    public STEP next_step = STEP.NONE; // 다음 단계.
    
    public float step_timer = 000.0f; // 타이머.
    private float clear_time = 0.0f; // 클리어 시간.
    private float GAME_OVER_TIME = 300.0f;
    
    public GUIStyle guistyle; // 폰트 스타일.



    void OnGUI()
    {
        float pos_x = Screen.width * 0.1f;
        float pos_y = Screen.height * 0.5f;
        switch (this.step)
        {
            case STEP.PLAY:
                GUIStyle timeStyle = new GUIStyle(GUI.skin.label);
                timeStyle.fontSize = 40;
                timeStyle.normal.textColor = Color.black;

                // 제한 시간에 도달할 때까지 남은 시간을 표시.
                float blast_time = GAME_OVER_TIME - this.step_timer;
                GUI.Label(new Rect(pos_x, pos_y, 200, 60),
                blast_time.ToString("000.00"), timeStyle);

                break;
            case STEP.CLEAR:
                GUI.color = Color.black;
                // 클리어 메시지와 클리어 시간 표시.
                GUI.Label(new Rect(pos_x, pos_y, 200, 20),
                 "탈출" + this.clear_time.ToString("000.00"), guistyle);
                pos_y -= 52;
                int ct = (int)clear_time; // 클리어 시간(float)를 int로 변환.

                this.guistyle.fontSize = 60;
                if (ct < 50)
                { 
                    GUI.Label(new Rect(pos_x, pos_y, 200, 120),
                     "성공! 다음 번에 더 빨리 단축할 듯?", guistyle);
                }
                else if (ct < 40)
                { 
                    GUI.Label(new Rect(pos_x, pos_y, 200, 120),
                     "성공! 실력이 좋으신 데요?", guistyle);
                }
                else if (ct < 30)
                { 
                    GUI.Label(new Rect(pos_x, pos_y, 200, 120),
                     "30초도 안남기고 아슬아슬하게 성공!", guistyle);
                }
                else
                { // 제일 빨리 탈출 
                    GUI.Label(new Rect(pos_x, pos_y, 200, 120),
                     "엄청난 속도로 성공!", guistyle);
                }
                break;
            case STEP.GAMEOVER:
                GUI.color = Color.black;
                this.guistyle.fontSize = 60;
                // 게임 오버 메시지를 표시.
                GUI.Label(new Rect(pos_x, pos_y, 200, 120),
                 "게임 오버, 좌클릭 시 시작 전 화면으로 이동", guistyle);
                break;
        }
    }

    void Start()
    {
        this.game_status = this.gameObject.GetComponent<GameStatus>();
        this.player_control =
        GameObject.Find("Player").GetComponent<PlayerControl>();
        this.step = STEP.PLAY;
        this.next_step = STEP.PLAY;
        this.guistyle.fontSize = 100;
    }

    // Update is called once per frame
    void Update()
    {
        this.step_timer += Time.deltaTime;
        if (this.next_step == STEP.NONE)
        {
            switch (this.step)
            {
                case STEP.PLAY:
                    if (this.game_status.isGameClear())
                    {
                        // 클리어 상태로 이동.
                        this.next_step = STEP.CLEAR;
                    }
                    if (this.game_status.isGameOver())
                    {
                        // 게임 오버 상태로 이동.
                        this.next_step = STEP.GAMEOVER;
                    }
                    if (this.step_timer > GAME_OVER_TIME)
                    {
                        // 제한 시간을 넘었으면 게임 오버.
                        this.next_step = STEP.GAMEOVER;
                    }
                    break;
                // 클리어 시 및 게임 오버 시의 처리.
                case STEP.CLEAR:
                    if (Input.GetMouseButtonDown(0))
                    {
                        // 마우스 버튼이 눌렸으면 TitleScene을 다시 읽는다.
                        SceneManager.LoadScene("TitleScene");
                    }
                    break;
                case STEP.GAMEOVER:
                    if (Input.GetMouseButtonDown(0))
                    {
                        // 마우스 버튼이 눌렸으면 TitleScene을 다시 읽는다.
                        SceneManager.LoadScene("TitleScene");
                    }
                    break;
            }
        }
        while (this.next_step != STEP.NONE)
        {
            this.step = this.next_step;
            this.next_step = STEP.NONE;
            switch (this.step)
            {
                case STEP.CLEAR:
                    // PlayerControl을 제어 불가로.
                    this.player_control.enabled = false;

                    // 클리어 시간 갱신.
                    this.clear_time = this.GAME_OVER_TIME - this.step_timer;
                    break;
                case STEP.GAMEOVER:
                    // PlayerControl를 제어 불가.
                    this.player_control.enabled = false;
                    break;
            }
            this.step_timer = 0.0f;
        }
    }
}
