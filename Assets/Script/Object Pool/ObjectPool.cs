using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Field ───────────────────────────────────────────────────────────
    // Pool Queue로 관리
    private Queue<IObject> _pool = new Queue<IObject>();

    /* Pool 기능 최적화를 위해 Component 상속
        poolObject : Component의 경우 this.gameObject 사용불가, 따라서 자기 자신을 담은 GameObject 생성
        origin : 자신이 담고있는 IObject 원본
    */
    private GameObject poolObject;
    private IObject origin;

    private List<IObject> useObjectList = new List<IObject>();

    // Method ───────────────────────────────────────────────────────────
    private IObject InstantiatePoolObject()
    {
        IObject newObj = GameObject.Instantiate<IObject>(origin);

        newObj.OnEnter();
        newObj.ConnectPool(this);

        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(poolObject.transform);

        useObjectList.Add(newObj);

        return newObj;
    }

    public ObjectPool(IObject prefab, Transform parent = null, int count = 1)
    {
        origin = prefab;

        GameObject poolObject = new GameObject();

        poolObject.name = prefab.gameObject.name + "Pool";
        poolObject.transform.SetParent(parent);
        poolObject.transform.localScale = Vector3.one;
        poolObject.transform.localPosition = Vector3.zero;

        this.poolObject = poolObject;

        for (int i = 0; i < count; i++)
        {
            _pool.Enqueue(InstantiatePoolObject());
        }
    }

    public void DestroyPool()
    {
        while (_pool.Count != 0)
        {
            IObject obj = _pool.Dequeue();

            obj.OnExit();
            Destroy(obj);
        }
        Destroy(this);
    }

    public IObject GetObject(Transform startTrans, Transform lookAtTrans = null, Transform parent = null)
    {
        IObject obj = null;

        if (_pool.Count == 0)
        {
            obj = InstantiatePoolObject();
        }
        else
        {
            obj = _pool.Dequeue();
        }

        obj.gameObject.SetActive(true);
        obj.transform.SetParent(parent);

        obj.transform.position = startTrans.position;

        if (lookAtTrans != null)
        {
            obj.transform.LookAt(lookAtTrans);
        }

        obj.OnInit();

        return obj;
    }

    public IObject GetObject(Transform parent = null, bool sameOrigin = false)
    {
        IObject obj = null;

        if (_pool.Count == 0)
        {
            obj = InstantiatePoolObject();
        }
        else
        {
            obj = _pool.Dequeue();
        }

        obj.gameObject.SetActive(true);
        obj.transform.SetParent(parent);

        if (sameOrigin)
        {
            obj.transform.localPosition = origin.transform.localPosition;
            obj.transform.localScale = origin.transform.localScale;
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
        }

        obj.OnInit();

        return obj;
    }

    public void PoolObject(IObject obj)
    {
        if (origin.gameObject.name + "(Clone)" == obj.gameObject.name)
        {
            obj.OnDisabled();
            obj.transform.SetParent(poolObject.transform);
            obj.gameObject.SetActive(false);

            _pool.Enqueue(obj);
        }
        else
        {
            Debug.Log("Pooling error : " + obj.name);
        }
    }

    public void PoolObject()
    {
        foreach(IObject obj in useObjectList)
        {
            if(obj.gameObject.activeSelf)
            {
                obj.OnDisabled();
                obj.transform.SetParent(poolObject.transform);
                obj.gameObject.SetActive(false);

                _pool.Enqueue(obj);
            }
        }
    }

    public IObject GetObjectAtPosition(Vector3 position, Transform parent = null)
    {
        IObject obj;

        if (_pool.Count == 0)
        {
            obj = InstantiatePoolObject();  // 풀에 없으면 새로 생성
        }
        else
        {
            obj = _pool.Dequeue();  // 풀에서 꺼냄
        }

        obj.gameObject.SetActive(true);
        obj.transform.SetParent(parent);
        obj.transform.position = position;

        obj.OnInit();
        return obj;
    }
}

