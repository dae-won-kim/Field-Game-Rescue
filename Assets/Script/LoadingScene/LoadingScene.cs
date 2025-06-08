using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    static string nextScene = "GameScene";
    [SerializeField] Image Progressbar;

    AsyncOperation op;

    public static void LoadScene(string SceneName)
    {
        nextScene = SceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneProcess()
    {
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                Progressbar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                Progressbar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);

                if (Progressbar.fillAmount >= 1.0f)
                {
                    break;
                }
            }
        }
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    void Update()
    {
        if (op != null && op.progress >= 0.9f && Progressbar.fillAmount >= 1.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                op.allowSceneActivation = true;
            }
        }
    }
}
