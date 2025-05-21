using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC
{
    public enum TYPE
    {
        NONE,
        RESCUE,
        NUM
    }
}

public class NPCRoot : MonoBehaviour
{
    public float Gauge; // Max 1.0f

    private GameObject GaugeBackGround;      // 게이지 전체 위치 기준
    private GameObject GaugeFill;      // 빨간색 채움 바 
    private MeshRenderer FillRenderer;

    private Vector3 offset = new Vector3(0f, 1.3f, 0f); // 머리 위 위치

    void setVariable()
    {
        // GameObject는 Component가 아니기 때문에
        // Transform으로 접근해 gameObject로 타고 가야함.
        Transform[] children = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            switch (child.gameObject.name)
            {
                case "GaugeBackGround":
                    GaugeBackGround = child.gameObject;
                    break;
                case "GaugeFill":
                    GaugeFill = child.gameObject;
                    break;
            }
        }
        FillRenderer = GaugeFill.GetComponent<MeshRenderer>();
    }
    public NPC.TYPE getNPCType(GameObject npc)
    {
        NPC.TYPE type = NPC.TYPE.NONE;
        if (npc != null)
        {
            switch (npc.tag)
            {
                case "RescueNPC":
                    type = NPC.TYPE.RESCUE;
                    break;
            }
        }
        return type;
    }

    void Start()
    {
        setVariable();

        // 초기값 0
        Gauge = 0f;

        // Gauge 위치 설정
        GaugeBackGround.transform.localPosition = offset;
        GaugeFill.transform.localPosition = offset + new Vector3(0f, 0f, -0.01f);

    }
    void Update()
    {
        if (GaugeFill != null)
        {
            // 게이지가 변화 -> 채워야 함.
            float clampedGauge = Mathf.Clamp01(Gauge);
            GaugeFill.transform.localScale = new Vector3(0.05f, 0.5f, 0.25f * Gauge);
        }
    }
}

