using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static float GAIN_REPAIRMENT_IRON = 0.20f;
    //  static float GAIN_REPAIRMENT_PLANT = 0.10f;

    // 철광석, 사과, 식물을 운반했을 때 각각의 체력 소모 정도.
    public static float CONSUME_SATIETY_IRON = 0.02f;
    public static float CONSUME_SATIETY_APPLE = 0.05f;
    public static float CONSUME_SATIETY_PLANT = 0.01f;

    // 스트레스, 힐템을 운반했을 때 각각의 체력 소모 정도
    public static float CONSUME_SATIETY_STRESS = 0.03f;
    public static float CONSUME_SATIETY_HEAL = 0.05f;

    // 항상 감소하는 배고픔 수치
    public static float CONSUME_SATIETY_ALWAYS = 0.03f;

    // 항상 증가하는 감정 수치
    public static float CONSUME_EMOTION_ALWAYS = 0.01f;

    // 스트레스,힐이 차는 속도
    public static float CONSUME_EMOTION_STRESS = 0.0f;
    public static float CONSUME_RESCUE_HEAL = 0.0f;

    // 사과, 식물을 먹었을 때 각각의 체력 회복 정도.
    public static float REGAIN_SATIETY_APPLE = 0.6f;
    public static float REGAIN_SATIETY_PLANT = 0.15f;

    // 스트레스 아이템을 사용했을 때 게이지가 감소하는 정도
    public static float REGAIN_EMOTION_STRESS = 0.25f;

    public static float REGAIN_GAUGE_HEAL = 0.1f;


    public float repairment = 0.0f; // 우주선의 수리 정도(0.0f~1.0f).
    public float satiety = 1.0f; // 배고픔,체력(0.0f~1.0f).
    public float emotion = 0.0f; // 감정 -> stress EMOTION

    public GUIStyle guistyle; // 폰트 스타일.

    [SerializeField] RescueNPC rescueNPC;

    // 배를 고프게 하는 메서드 추가
    public void alwaysSatiety()
    {
        this.satiety = Mathf.Clamp01(this.satiety - CONSUME_SATIETY_ALWAYS * Time.deltaTime);
    }

    // 스트레스 증가 추가
    public void alwaysEmotion()
    {
        this.emotion = Mathf.Clamp01(this.emotion + CONSUME_SATIETY_ALWAYS * Time.deltaTime);
    }

    // 우주선 수리를 진행
    public void addRepairment(float add)
    {
        this.repairment = Mathf.Clamp01(this.repairment + add); // 0.0~1.0 강제 지정
    }

    // 체력을 늘리거나 줄임
    public void addSatiety(float add)
    {
        this.satiety = Mathf.Clamp01(this.satiety + add);
    }

    // 감정 수치 조절
    public void subtractEmotion(float amount)
    {
        this.emotion = Mathf.Clamp01(this.emotion - amount);
    }

    public bool isGameClear()
    {
        bool is_clear = false;
        if (this.repairment >= 1.0f && rescueNPC.Gauge >=1.0f)
        { // 수리 정도가 100% 이상 && NPC 게이지가 100%
            is_clear = true; // 클리어했다.
        }
        return (is_clear);
    }

    public bool isGameOver()
    {
        bool is_over = false;
        if (this.satiety <= 0.0f)
        { // 체력이 0이하라면.
            is_over = true; // 게임 오버.
        }
        return (is_over);
    }

    void OnGUI()
    {
        float x = Screen.width * 0.2f;
        float y = 20.0f;
        // 체력을 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f),
            "체력: " + (this.satiety * 100.0f).ToString("000"), guistyle);

        x += 250;
        // 감정 수치를 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f),
            "스트레스: " + (this.emotion * 100.0f).ToString("000"), guistyle);

        x += 350;
        // 수리 정도를 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f),
            "구급차 " + (this.repairment * 100.0f).ToString("000"), guistyle);
    }

    void Start()
    {
        this.guistyle.fontSize = 48;
        rescueNPC = GameObject.Find("RescueNPC").GetComponentInChildren<RescueNPC>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
