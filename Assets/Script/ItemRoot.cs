using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public enum TYPE
    { // 아이템 종류.
        NONE = -1, IRON = 0, APPLE, PLANT, // 없음, 철광석, 사과, 식물.
        STRESS,HEAL,
        NUM,
    }; // 아이템이 몇 종류인가 나타낸다(3+2). 스트레스 아이템, 구조자 치료 
};

public class ItemRoot : MonoBehaviour
    {
    protected List<Vector3> plant_respawn_points; // Plant 출현 지점 List.
    protected List<Vector3> stress_respawn_points;

    public GameObject ironPrefab = null; // Prefab 'Iron'
    public GameObject plantPrefab = null; // Prefab 'Plant'
    public GameObject applePrefab = null; // Prefab 'Apple'

    public GameObject stressPrefab = null; // Prefab 'Stress'
    public GameObject healPrefab = null; // Prefab 'Heal'

    public float step_timer = 0.0f;

    public static float RESPAWN_TIME_APPLE = 22.0f; // 사과 출현 시간 상수.
    public static float RESPAWN_TIME_IRON = 18.0f; // 철광석 출현 시간 상수.
    public static float RESPAWN_TIME_PLANT = 15.0f; // 식물 출현 시간 상수.

    // 스트레스, 힐템 출현 시간 상수.
    public static float RESPAWN_TIME_STRESS = 17.0f;
    public static float RESPAWN_TIME_HEAL = 15.0f;

    private float respawn_timer_apple = 0.0f; // 사과의 출현 시간.
    private float respawn_timer_iron = 0.0f; // 철광석의 출현 시간.
    private float respawn_timer_plant = 0.0f; // 식물의 출현 시간.

    // 스트레스, 힐템 출현 시간.
    private float respawn_timer_stress = 0.0f;
    private float respawn_timer_heal = 0.0f;

    // 아이템의 종류를 Item.TYPE형으로 반환하는 메소드.
    public Item.TYPE getItemType(GameObject item_go)
    {
        Item.TYPE type = Item.TYPE.NONE;
        if (item_go != null)
        { // 인수로 받은 GameObject가 비어있지 않으면.
            switch (item_go.tag)
            { // 태그로 분기.
                case "Iron": type = Item.TYPE.IRON; break;
                case "Apple": type = Item.TYPE.APPLE; break;
                case "Plant": type = Item.TYPE.PLANT; break;
                case "Stress": type = Item.TYPE.STRESS; break;
                case "Heal": type = Item.TYPE.HEAL; break;
            }
        }
        return (type);
    }

    public void respawnIron()
    {
        // 철광석 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.ironPrefab) as GameObject;
        // 철광석의 출현 포인트를 취득.
        Vector3 pos = GameObject.Find("IronRespawn").transform.position;
        // 출현 위치를 조정.
        pos.y = 1.0f;
        pos.x += Random.Range(-3.0f, 3.0f);
        pos.z += Random.Range(-3.0f, 3.0f);
        // 철광석의 위치를 이동.
        go.transform.position = pos;
    }

    public void respawnApple()
    {
        // 사과 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.applePrefab) as GameObject;
        // 사과의 출현 포인트를 취득.
        Vector3 pos = GameObject.Find("AppleRespawn").transform.position;
        // 출현 위치를 조정.
        pos.y = 1.0f;
        pos.x += Random.Range(-3.0f, 3.0f);
        pos.z += Random.Range(-3.0f, 3.0f);
        // 사과의 위치를 이동.
        go.transform.position = pos;
    }

    public void respawnPlant()
    {
        if (this.plant_respawn_points.Count > 0)
        { // List가 비어있지 않으면.
          // 식물 프리팹을 인스턴스화.
            GameObject go = GameObject.Instantiate(this.plantPrefab) as GameObject;
            // 식물의 출현 포인트를 랜덤하게 취득.
            int n = Random.Range(0, this.plant_respawn_points.Count);
            Vector3 pos = this.plant_respawn_points[n];
            // 출현 위치를 조정.
            pos.y = 1.0f;
            pos.x += Random.Range(-3.0f, 3.0f);
            pos.z += Random.Range(-3.0f, 3.0f);
            // 식물의 위치를 이동.
            go.transform.position = pos;
        }
    }

    public void respawnStress() 
    {
        if (this.stress_respawn_points.Count > 0)
        { // List가 비어있지 않으면.
          // 스트레스 프리팹을 인스턴스화.
            GameObject go = GameObject.Instantiate(this.stressPrefab) as GameObject;
            // 식물의 출현 포인트를 랜덤하게 취득.
            int n = Random.Range(0, this.stress_respawn_points.Count);
            Vector3 pos = this.stress_respawn_points[n];
            // 출현 위치를 조정.
            pos.y = 1.0f;
            pos.x += Random.Range(-3.0f, 3.0f);
            pos.z += Random.Range(-3.0f, 3.0f);
            // 스트레스의 위치를 이동.
            go.transform.position = pos;
        }
    }

    public void respawnHeal() 
    {
        // 힐 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.healPrefab) as GameObject;
        // 힐의 출현 포인트를 취득.
        Vector3 pos = GameObject.Find("HealRespawn").transform.position;
        // 출현 위치를 조정.
        pos.y = 1.0f;
        pos.x += Random.Range(-3.0f, 3.0f);
        pos.z += Random.Range(-3.0f, 3.0f);
        // 힐의 위치를 이동.
        go.transform.position = pos;
    }

    // 들고 있는 아이템에 따른 ‘수리 진척 상태’를 반환
    public float getGainRepairment(GameObject item_go)
    {
        float gain = 0.0f;
        if (item_go == null)
        {
            gain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.IRON:
                    gain = GameStatus.GAIN_REPAIRMENT_IRON; break;
            }
        }
        return (gain);
    }

    // 들고 있는 아이템에 따른 ‘체력 감소 상태’를 반환
    public float getConsumeSatiety(GameObject item_go)
    {
        float consume = 0.0f;
        if (item_go == null)
        {
            consume = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.IRON:
                    consume = GameStatus.CONSUME_SATIETY_IRON; break;
                case Item.TYPE.APPLE:
                    consume = GameStatus.CONSUME_SATIETY_APPLE; break;
                case Item.TYPE.PLANT:
                    consume = GameStatus.CONSUME_SATIETY_PLANT; break;
                case Item.TYPE.STRESS:
                    consume = GameStatus.CONSUME_SATIETY_STRESS; break;
                case Item.TYPE.HEAL:
                    consume = GameStatus.CONSUME_SATIETY_HEAL; break;
            }
        }
        return (consume);
    }

    // 들고 있는 아이템에 따른 ‘체력 회복 상태’를 반환
    public float getRegainSatiety(GameObject item_go)
    {
        float regain = 0.0f;
        if (item_go == null)
        {
            regain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.APPLE:
                    regain = GameStatus.REGAIN_SATIETY_APPLE; break;
                case Item.TYPE.PLANT:
                    regain = GameStatus.REGAIN_SATIETY_PLANT; break;
            }
        }
        return (regain);
    }
    // 스트레스 아이템에 대한 수치 반환 
    public float getRegainEmotion(GameObject item_go)
    {
        float regain = 0.0f;
        if (item_go == null)
        {
            regain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.STRESS:
                    regain = GameStatus.REGAIN_EMOTION_STRESS; break;
            }
        }
        return (regain);
    }

    public float getRegainNPCGauge(GameObject item_go)
    {
        float regain = 0.0f;
        if (item_go == null)
        {
            regain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.HEAL:
                    regain = GameStatus.REGAIN_GAUGE_HEAL; break;
            }
        }
        return (regain);
    }


    void Start()
    {
        // Plant ----------------------------
        // 메모리 영역 확보.
        this.plant_respawn_points = new List<Vector3>();
        // "PlantRespawn" 태그가 붙은 모든 오브젝트를 배열에 저장.
        GameObject[] plantRespawns = GameObject.FindGameObjectsWithTag("PlantRespawn");

        // 배열 respawns 내 각각의 GameObject를 순서대로 처리한다.
        foreach (GameObject go in plantRespawns)
        {
            // 렌더러 획득.
            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            { // 렌더러가 존재하면.
                renderer.enabled = false; // 그 렌더러를 보이지 않게.
            }
            // 출현 포인트 List에 위치 정보를 추가.
            this.plant_respawn_points.Add(go.transform.position);
        }

        // ----------------------------------

        // Stress ----------------------------
        // 메모리 영역 확보.
        this.stress_respawn_points = new List<Vector3>();
        // "StressRespawn" 태그가 붙은 모든 오브젝트를 배열에 저장.
        GameObject[] stressRespawns = GameObject.FindGameObjectsWithTag("StressRespawn");

        // 배열 respawns 내 각각의 GameObject를 순서대로 처리한다.
        foreach (GameObject go in stressRespawns)
        {
            // 렌더러 획득.
            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            { // 렌더러가 존재하면.
                renderer.enabled = false; // 그 렌더러를 보이지 않게.
            }
            // 출현 포인트 List에 위치 정보를 추가.
            this.stress_respawn_points.Add(go.transform.position);
        }

        // ----------------------------------

        // 사과의 출현 포인트를 취득하고, 렌더러를 보이지 않게.
        GameObject applerespawn = GameObject.Find("AppleRespawn");
        applerespawn.GetComponent<MeshRenderer>().enabled = false;

        // 철광석의 출현 포인트를 취득하고, 렌더러를 보이지 않게.
        GameObject ironrespawn = GameObject.Find("IronRespawn");
        ironrespawn.GetComponent<MeshRenderer>().enabled = false;

        GameObject healrespawn = GameObject.Find("HealRespawn");
        healrespawn.GetComponent<MeshRenderer>().enabled = false;

        this.respawnIron(); // 시작 시 철광석을 하나 생성.

    }

    void Update()
    {
        respawn_timer_apple += Time.deltaTime;
        respawn_timer_iron += Time.deltaTime;
        respawn_timer_plant += Time.deltaTime;

        respawn_timer_heal += Time.deltaTime;
        respawn_timer_stress += Time.deltaTime; 

        if (respawn_timer_apple > RESPAWN_TIME_APPLE)
        {
            respawn_timer_apple = 0.0f;
            this.respawnApple(); // 사과를 출현시킨다.
        }
        if (respawn_timer_iron > RESPAWN_TIME_IRON)
        {
            respawn_timer_iron = 0.0f;
            this.respawnIron(); // 철광석을 출현시킨다.
        }
        if (respawn_timer_plant > RESPAWN_TIME_PLANT)
        {
            respawn_timer_plant = 0.0f;
            this.respawnPlant(); // 식물을 출현시킨다.
        }

        if (respawn_timer_stress > RESPAWN_TIME_STRESS)
        {
            respawn_timer_stress = 0.0f;
            this.respawnStress(); // 스트레스 아이템을 출현시킨다.
        }
        if (respawn_timer_heal > RESPAWN_TIME_HEAL)
        {
            respawn_timer_heal = 0.0f;
            this.respawnHeal(); // 힐템을 출현시킨다.
        }
    }
}
