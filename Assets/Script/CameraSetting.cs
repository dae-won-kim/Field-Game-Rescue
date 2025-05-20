using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    private Transform playerTransform;       
    public Vector3 offset;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponentInChildren<Transform>();
        offset = this.transform.position - playerTransform.position;
    }

    void LateUpdate()
    {
        // 위치는 따라가고, 회전은 고정
        this.transform.position = playerTransform.position + offset;
    }
}
