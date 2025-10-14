using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    // BGM
    public AudioClip TitleBGM;
    public AudioClip LoadingBGM;
    public AudioClip InGameBGM;

    // GameEffect Sound
    public AudioClip GameClearSound;
    public AudioClip GameFailSound;
    public AudioClip FeverTimeSound;


    //// PlayerEffect Sound
    public AudioClip PickSound;
    public AudioClip DropSound;
    //public AudioClip TrapSound;


    public AudioClip EatSound;
    public AudioClip RepairSound;
    public AudioClip StressSound;
    public AudioClip HealSound;



    private AudioSource currAudio;
    private AudioSource PlayerEffectAudio;
    private string currSceneName = "";


    // 오디오 반복 재생을 요할 때만 사용
    private AudioClip queuedClip;
    private int queuedCount;



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = scene.name;
        if (newSceneName != currSceneName)
        {
            currSceneName = newSceneName;
            PlayBGMForScene(currSceneName);
        }
    }

    void PlayBGMForScene(string sceneName)
    {
        AudioClip newBGM = null;

        switch (sceneName)
        {
            case "TitleScene":
                newBGM = TitleBGM;
                break;
            case "LoadingScene":
                newBGM = LoadingBGM;
                break;
            case "GameScene":
                newBGM = InGameBGM;
                break;
        }


        if (newBGM != null && currAudio.clip != newBGM)
        {
            currAudio.clip = newBGM;
            currAudio.volume = 0.2f;
            currAudio.Play();
        }
    }

    public void PlayerEating()
    {
        if (EatSound != null)
            PlayerEffectAudio.PlayOneShot(EatSound, 1.0f);
    }

    public void PlayerStressDown()
    {
        if (StressSound != null)
            PlayerEffectAudio.PlayOneShot(StressSound, 1.0f);
    }

    public void PlayerRepairing()
    {
        PlayAudioRepeated(RepairSound, 2);
    }

    public void PlayerFeverTime()
    {
        PlayAudioRepeated(FeverTimeSound, 5);
    }
    public void PlayerHealing()
    {
        if (HealSound != null)
            PlayerEffectAudio.PlayOneShot(HealSound, 1.0f);
    }

    public void PlayerPickUp()
    {
        if (PickSound != null)
            PlayerEffectAudio.PlayOneShot(PickSound,0.5f);
    }

    public void PlayerPut()
    {
        if (DropSound != null)
            PlayerEffectAudio.PlayOneShot(DropSound, 0.5f);
    }

    public void PlayGameClear()
    {
        if (GameClearSound != null)
            PlayerEffectAudio.PlayOneShot(GameClearSound, 0.5f);
    }
    public void PlayGameOver()
    {
        if (GameFailSound != null)
            PlayerEffectAudio.PlayOneShot(GameFailSound, 0.5f);
    }


    public void PlayAudioRepeated(AudioClip clip, int count)
    {
        if (clip == null || count <= 0 || PlayerEffectAudio == null)
            return;

        queuedClip = clip;
        queuedCount = count;

        PlayNextAudio();  // 첫 재생 시작
    }

    // 내부에서 1회 재생하고 다음을 예약
    private void PlayNextAudio()
    {
        if (queuedCount <= 0 || queuedClip == null) return;

        PlayerEffectAudio.PlayOneShot(queuedClip, 1.0f);
        queuedCount--;

        if (queuedCount > 0)
            Invoke(nameof(PlayNextAudio), queuedClip.length); // 다음 재생 예약
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            currAudio = gameObject.AddComponent<AudioSource>();
            currAudio.loop = true;
            currAudio.volume = 0.5f;

            PlayerEffectAudio = gameObject.AddComponent<AudioSource>(); // player effect sound
            PlayerEffectAudio.loop = false;

            SceneManager.sceneLoaded += OnSceneLoaded; // Scene-Change Detect Event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currSceneName = SceneManager.GetActiveScene().name;
        PlayBGMForScene(currSceneName);
    }


}
