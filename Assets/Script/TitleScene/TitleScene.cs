using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    // [SerializeField]
    [SerializeField] GameObject Title;
    [SerializeField] GameObject HowToPlayButton;
    [SerializeField] GameObject GameStartButton;
    [SerializeField] GameObject BackButton;

    [SerializeField] GameObject PlayMethodImg;
    [SerializeField] GameObject CopyRight;


    public void HowToPlayClicked()
    {
        Title.SetActive(false);
        HowToPlayButton.SetActive(false);
        GameStartButton.SetActive(false);

        PlayMethodImg.SetActive(true);
        CopyRight.SetActive(false);

        BackButton.SetActive(true);
    }
    public void GameStartClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void BackButtonClicked()
    {
        Title.SetActive(true);
        HowToPlayButton.SetActive(true);
        GameStartButton.SetActive(true);

        PlayMethodImg.SetActive(false);
        CopyRight.SetActive(true);

        BackButton.SetActive(false);
    }
   

    void SetButtonTexts()
    {   
        BackButton.SetActive(false);

        TextMeshProUGUI TitleText = Title.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI HowToPlayText = HowToPlayButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI GameStartText = GameStartButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI BackButtonText = BackButton.GetComponentInChildren<TextMeshProUGUI>();

        TitleText.text = "RESCUE!!";
        TitleText.fontStyle = FontStyles.Bold;
        TitleText.fontSize = 150;
        TitleText.alignment = TextAlignmentOptions.Center;

        HowToPlayText.text = "How To Play";
        HowToPlayText.fontSize = 50;

        GameStartText.text = "Start";
        GameStartText.fontSize = 50;

        BackButtonText.text = "Back";
        BackButtonText.fontSize = 50;
    }

    void Start()
    {
        Title = GameObject.Find("Title");
        HowToPlayButton = GameObject.Find("HowToPlayButton");
        GameStartButton = GameObject.Find("GameStartButton");
        BackButton = GameObject.Find("BackButton");

        PlayMethodImg = GameObject.Find("PlayMethodImg");
        PlayMethodImg.SetActive(false);

        CopyRight = GameObject.Find("CopyRight");

        SetButtonTexts();
    }

    void Update()
    {

    }
}
