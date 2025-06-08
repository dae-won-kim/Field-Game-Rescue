using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] GameObject Title;
    [SerializeField] GameObject PressAnyKey;

    // How To Play Images
    [SerializeField] GameObject appleImage;
    [SerializeField] GameObject stressImage;
    [SerializeField] GameObject plantImage;

    [SerializeField] GameObject healImage;
    [SerializeField] GameObject healUseImage;

    [SerializeField] GameObject carImage;
    [SerializeField] GameObject carUseImage;

    [SerializeField] GameObject CopyRight;


    //[SerializeField] GameObject HowToPlayButton;
    //[SerializeField] GameObject GameStartButton;
    //[SerializeField] GameObject BackButton;
    //[SerializeField] GameObject PlayMethodImg;
     

    // Button Functions
    /* public void HowToPlayClicked()
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
    */

    // Set Button
    /*
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
    */

    void SetTexts()
    {
        // Titel Message
        TextMeshProUGUI TitleText = Title.GetComponent<TextMeshProUGUI>();

        TitleText.text = "RESCUE!!";
        TitleText.fontStyle = FontStyles.Bold;
        TitleText.fontSize = 150;
        TitleText.alignment = TextAlignmentOptions.Center;

        // Press Key Message
        TextMeshProUGUI KeyMessage = PressAnyKey.GetComponent<TextMeshProUGUI>();

        KeyMessage.text = "Press Any Key to Continue";
        KeyMessage.fontStyle = FontStyles.Normal;
        KeyMessage.fontSize = 50;
        KeyMessage.alignment = TextAlignmentOptions.Center;
    }

    void HowToPlay()
    {

    }
    void Start()
    {
        Title = GameObject.Find("Title");
        PressAnyKey = GameObject.Find("PressAnyKey");
        CopyRight = GameObject.Find("CopyRight");

        //HowToPlayButton = GameObject.Find("HowToPlayButton");
        //GameStartButton = GameObject.Find("GameStartButton");
        //BackButton = GameObject.Find("BackButton");

        //PlayMethodImg = GameObject.Find("PlayMethodImg");
        //PlayMethodImg.SetActive(false);


        // SetButtonTexts();
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.Escape))
        //{
        //    BackButtonClicked();
        //}
    }
}
