using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    private Transform playerTransform;       
    [SerializeField] Vector3 offset;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponentInChildren<Transform>();
        offset = new Vector3(0f, 10f, -7f);
    }

    void LateUpdate()
    {
        // 위치만 일정 거리에서 따라가도록 
        this.transform.position = playerTransform.position + offset;
    }
}
