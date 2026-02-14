using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IObject : MonoBehaviour
{
    protected ObjectPool _parentPool;
    /*
     ObjectPool 에서 불러올때
     Enter : 생성
     Exit : 삭제
     Init : 불러올때마다 초기화
     Disabled : 비활성화
     */

    public virtual void ConnectPool(ObjectPool pool)
    {
        _parentPool = pool;

    }
    public virtual void PoolObject()
    {
        _parentPool.PoolObject(this);
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnInit();
    public abstract void OnDisabled();
}

