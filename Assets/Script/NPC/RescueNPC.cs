using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueNPC : NPCRoot
{
    [SerializeField] bool isRescued = false;

    void setGauges()
    {
        GaugeBackGround.transform.localPosition = offset;
        GaugeFill.transform.localPosition = offset + new Vector3(0f, 0f, -0.01f);

        this.GaugeFill.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    void updateGauge()
    {
        if (GaugeFill != null)
        {
            // 게이지가 변화 -> 채워야 함.
            float clampedGauge = Mathf.Clamp01(Gauge);
            GaugeFill.transform.localScale = new Vector3(0.05f, 0.5f, 0.25f * Gauge);
        }
    }

    void Start()
    {
        this.Gauge = 0f;
        base.setVariable();

    }

    void Update()
    {
        updateGauge();
        if (this.Gauge >=1.0f)
        {
            isRescued = true;
        }
    }
}
