using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;

public class LerpColour : MonoBehaviour
{
    //ArrayStuff
    public GameObject[] buttons;
    private int randomButtons = 0;
    private int randomButtons2 = 0;
    private int previousArrayNum = 0;

    //Ray Stuff
    Ray ray;
    RaycastHit hit;

    //Lerping Colour Stuff
    public Color colourStart = Color.white;
    public Color colourEnd = Color.green;
    private float lerpTime = 1.0f;
    public float timer;
    private float difficulty;
    private bool hasSpedUp = false;

    //UI Stuff
    public GameObject endGameUI;
    public Text highscoreText;
    public Text endScoreText;
    public Text scoreText;
    public int score;

    void Start()
    {
        //Find all buttons that have the tag button
        buttons = GameObject.FindGameObjectsWithTag("Button");
        timer = difficulty = 10.0f;
        //RequestBanner();
    }

    void Update()
    {
        //Decrease time at 0.1f
        timer -= 0.1f;

        //If timer is less than or equal too 0.0f then run the function
        if (timer <= 0.0f && !endGameUI.activeInHierarchy)
        {
            RandomGameObject();
        }

        //Detect where the ray is
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //If the left mouse button is clicked and the raycast hits then
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            //ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {

                //if the hit equals the current lit up button gameobject then
                if (hit.collider.gameObject == buttons[randomButtons].gameObject && buttons[randomButtons].gameObject.GetComponent<Renderer>().material.color == colourEnd)
                {
                    Properties();
                    buttons[randomButtons].GetComponent<Renderer>().material.color = Color.Lerp(colourEnd, colourStart, Time.time * lerpTime);

                }
                else if(hit.collider.gameObject == buttons[randomButtons2].gameObject && buttons[randomButtons2].gameObject.GetComponent<Renderer>().material.color == colourEnd)
                {
                    Properties();
                    buttons[randomButtons2].GetComponent<Renderer>().material.color = Color.Lerp(colourEnd, colourStart, Time.time * lerpTime);
                }
                else
                {
                    //If the current lit up button is not pressed so either missed or clicked the wrong one then go to the end game function
                    ShowEndGameUI();
                }
            }
        }

        if (timer <= 0.01f && buttons[randomButtons].gameObject.GetComponent<Renderer>().material.color == colourEnd)
        {
            ShowEndGameUI();
        }
        if(timer <= 0.01f && buttons[randomButtons2].gameObject.GetComponent<Renderer>().material.color == colourEnd)
        {
            previousArrayNum = randomButtons;
        }

        //This is the modulos operation, so every 10 points this will increment the speed once
        if (score % 10 == 0)
        {
                //Bool to say that when hasSpedUp is false it will call the function and set it to true so that it only goes through this once
                if (hasSpedUp == false && !(difficulty <= 5))
                {
                    hasSpedUp = true;
                    difficulty -= 1.5f;
                    difficulty = Mathf.Clamp(difficulty, 2, 10);
                    timer = difficulty;
                }
        }
        else
        {
            hasSpedUp = false;
        }
    }

    void RandomGameObject()
    {
        //Randomly find choose a number between 0 and the array length of 9
        randomButtons = Random.Range(0, buttons.Length);

        //randomButtons now has it's random gameobject, find the renderer, change it's material to lerp between white to green and vice versa
        buttons[randomButtons].GetComponent<Renderer>().material.color = Color.Lerp(colourStart, colourEnd, Time.time * lerpTime);

        //reset timer
        timer = difficulty;

        if(score >= 15)
        {
            RandomButton();
        }

        UnityEngine.Debug.Log(buttons[randomButtons].name);
    }

    void RandomButton()
    {
        randomButtons2 = Random.Range(0, buttons.Length);

        if (randomButtons == randomButtons2)
        {
            RandomButton();
        }
        else
        {
            buttons[randomButtons2].GetComponent<Renderer>().material.color = Color.Lerp(colourStart, colourEnd, Time.time * lerpTime);
        }
    }

    void Properties()
    {
        //Increment score by one, output that out to GUI and then change the green back to white. Stopwatch then restarts
        score += 1;
        scoreText.text = "Score: " + score.ToString();
        timer = difficulty;
    }

    void ShowEndGameUI()
    {
        endGameUI.SetActive(true);
        endScoreText.text = "Your Score: " + score.ToString();

        if (PlayerPrefs.HasKey("Highscore"))
        {
            if (PlayerPrefs.GetInt("Highscore") < score)
            {
                PlayerPrefs.SetInt("Highscore", score);
            }
            highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-4874312810764540/7307904102";
        #endif

        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);

    }
}
    