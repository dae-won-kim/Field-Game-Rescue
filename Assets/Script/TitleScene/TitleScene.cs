using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class TitleScene : MonoBehaviour
{
    [SerializeField] GameObject Title;
    [SerializeField] GameObject PressAnyKey;

    // How To Play Images
    [SerializeField] GameObject GoalUI;
    [SerializeField] GameObject PlayerItemUI;
    [SerializeField] GameObject CarItemUI;
    [SerializeField] GameObject HealItemUI;

    [SerializeField] int PageNum;
    [SerializeField] bool isPressed;
    // prevent skipping first image
    [SerializeField] bool firstInputHandled;
    
    // How to Play Text UI position
    Vector2 textPos1 = new Vector2(239f, 195f);
    Vector2 textPos2 = new Vector2(-201f, 367f);
    Vector2 textPos3 = new Vector2(-164f, 367f);

    [SerializeField] GameObject CopyRight;
    [SerializeField] GameObject ScreenResolution;


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

        KeyMessage.text = "Press Spacebar to Continue";
        KeyMessage.fontStyle = FontStyles.Normal;
        KeyMessage.fontSize = 50;
        KeyMessage.alignment = TextAlignmentOptions.Center;

        // Gaol UI
        TextMeshProUGUI GoalMessage = GoalUI.GetComponentInChildren<TextMeshProUGUI>();
        GoalMessage.rectTransform.localPosition = Vector2.zero + new Vector2(-133f,0f);
        GoalMessage.text =
           "Goal\n" +
           "Repair Car and Rescue injured person in 5 Min\n"+
           "Move: Arrow Keys\n"+
           "Pick Item: Z key, Interaction: X key\n"+
           "\n"+
           "Game Over Conditions\n"+
           "Health goes 0 or time over ";
        GoalMessage.enableAutoSizing = true;
        GoalMessage.alignment = TextAlignmentOptions.Center;

        // Default Item UI
        TextMeshProUGUI PlayerItemMessage = PlayerItemUI.GetComponentInChildren<TextMeshProUGUI>();
        PlayerItemMessage.rectTransform.localPosition = textPos1;
        PlayerItemMessage.text =
           "Red: Health item, +70\n"+
           "Green: Health item, +20\n" +
           "Orange: Stress item, -25\n"+
           "How To Use: Pick item and Press X Key";
        PlayerItemMessage.enableAutoSizing = true;
        PlayerItemMessage.alignment = TextAlignmentOptions.Center;

        // Car Item UI
        TextMeshProUGUI CarItemMessage = CarItemUI.GetComponentInChildren<TextMeshProUGUI>();
        CarItemMessage.rectTransform.localPosition = textPos2;
        CarItemMessage.text =
           "White: Fix Item, Can Only Use to Car\n"+
           "How To Use: Pick item and Go To the Car Object and Press X key";
        CarItemMessage.enableAutoSizing = true;
        CarItemMessage.alignment = TextAlignmentOptions.Center;

        // Heal Item UI
        TextMeshProUGUI HealItemMessage = HealItemUI.GetComponentInChildren<TextMeshProUGUI>();
        HealItemMessage.rectTransform.localPosition = textPos3;
        HealItemMessage.text =
           "Purple: Heal Item, Can Only Use to NPC \n"+
           "How To Use: Pick item and Go to the NPC and Press X key\n"+
           "\n"+
           "Press Enter to Start Game!";
        HealItemMessage.enableAutoSizing= true;
        HealItemMessage.alignment = TextAlignmentOptions.Center;

    }

    // Explanations for Game
    void HowToPlay(bool isPressed)
    {
        if (isPressed)
        {
            switch (PageNum)
            {
                case 1:
                    GoalUI.SetActive(true);
                    PlayerItemUI.SetActive(false);
                    CarItemUI.SetActive(false);
                    HealItemUI.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PageNum = 2;
                    }
                    break;
                case 2:
                    GoalUI.SetActive(false);
                    PlayerItemUI.SetActive(true);
                    CarItemUI.SetActive(false);
                    HealItemUI.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PageNum=3;
                    }
                    else if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        PageNum = 1;
                    }
                    break;
                case 3:
                    GoalUI.SetActive(false);
                    PlayerItemUI.SetActive(false);
                    CarItemUI.SetActive(true);
                    HealItemUI.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PageNum=4;
                    }
                    else if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        PageNum=2;
                    }
                    break;
                case 4:
                    GoalUI.SetActive(false);
                    PlayerItemUI.SetActive(false);
                    CarItemUI.SetActive(false);
                    HealItemUI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        PageNum=3;
                    }
                    else if(Input.GetKeyDown(KeyCode.Return))
                    {
                        SceneManager.LoadScene("LoadingScene");
                    }
                    break;
            }
        }
    }
    void Awake()
    {
        Title = GameObject.Find("Title");
        PressAnyKey = GameObject.Find("PressAnyKey");
        CopyRight = GameObject.Find("CopyRight");
        ScreenResolution = GameObject.Find("Resolution");

        GoalUI = GameObject.Find("GoalUI");
        PlayerItemUI = GameObject.Find("PlayerItemUI");
        CarItemUI = GameObject.Find("CarItemUI");
        HealItemUI = GameObject.Find("HealItemUI");

        PageNum = 0;
        isPressed = false;
        firstInputHandled = false;
    }
    void Start()
    {
        // Initialize
        Title.SetActive(true);
        PressAnyKey.SetActive(true);
        CopyRight.SetActive(true);
        ScreenResolution.SetActive(true);

        GoalUI.SetActive(false);
        PlayerItemUI.SetActive(false);
        CarItemUI.SetActive(false);
        HealItemUI.SetActive(false);

        SetTexts();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPressed && PageNum == 0)
        {
            isPressed = true;
            PageNum = 1;

            Title.SetActive(false);
            PressAnyKey.SetActive(false);
            CopyRight.SetActive(false);
            ScreenResolution.SetActive(false);

            firstInputHandled = true;
            return;
        }

        if (firstInputHandled)
        {
            firstInputHandled = false;
            return;
        }

        HowToPlay(isPressed);
    }
}
