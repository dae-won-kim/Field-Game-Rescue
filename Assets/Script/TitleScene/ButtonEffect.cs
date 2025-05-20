using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler    // 마우스 포인터가 들어갔을 때, 나왔을 때
{
    private Vector3 OriginalScale;
    private float LargeFactor = 1.2f;  // 커질 버튼 크기

    void Start()
    {
        OriginalScale = transform.localScale;
    }

    // 인터페이스 함수 (이름 수정 X)
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = OriginalScale * LargeFactor;
    }

    // 인터페이스 함수 (이름 수정 X)
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = OriginalScale;
    }
}
