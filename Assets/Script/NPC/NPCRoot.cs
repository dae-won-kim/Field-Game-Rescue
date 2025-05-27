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

    protected GameObject GaugeBackGround;      // 게이지 전체 위치 기준
    protected GameObject GaugeFill;      // 빨간색 채움 바 
    protected MeshRenderer FillRenderer;
    protected Collider Event_Area;

    protected Vector3 offset = new Vector3(0f, 1.3f, 0f); // 머리 위 위치

    protected void setVariable()
    {
        // GameObject는 Component가 아니기 때문에
        // Transform으로 접근해 gameObject로 타고 가야함.
        Transform[] children = this.transform.root.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            switch (child.name)
            {
                case "GaugeBackGround":
                    GaugeBackGround = child.gameObject;
                    break;
                case "GaugeFill":
                    GaugeFill = child.gameObject;
                    break;
            }
        }
        if (GaugeFill != null)
            FillRenderer = GaugeFill.GetComponent<MeshRenderer>();

        Event_Area = this.GetComponent<Collider>();

    }

    public virtual void addGauge(float number)
    {
        Gauge += number;
    }

    public virtual void subTractGauge(float number)
    {
        Gauge -= number;
    }

    public NPC.TYPE getNPCType(GameObject npc)
    {
        NPC.TYPE type = NPC.TYPE.NONE;
        if (npc != null)
        {
            switch (npc.tag)
            {
                case "Rescue":
                    type = NPC.TYPE.RESCUE;
                    break;
            }
        }
        return type;
    }

}

