using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// For Effect
public class FXObject : IObject
{
    private ParticleSystem fxSystem;
    private Coroutine fxPlaying = null;
    public override void OnEnter()
    {
        fxSystem = GetComponent<ParticleSystem>();
    }

    public override void OnInit()
    {
        if (fxPlaying == null)
            fxPlaying = StartCoroutine(playFX());
    }

    public override void OnDisabled()
    {
        if (fxPlaying != null)
        {
            StopCoroutine(fxPlaying);
        }
        fxPlaying = null;
    }

    public override void OnExit()
    {
    }

    private IEnumerator playFX()
    {
        fxSystem.Play();

        while (fxSystem.isPlaying)
        {
            yield return null;
        }

        fxPlaying = null;

        PoolObject();

    }
}

