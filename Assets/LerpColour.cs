using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LerpColour : MonoBehaviour
{
    //ArrayStuff
    public GameObject[] buttons;
    private int randomButtons = 0;

    Ray ray;
    RaycastHit hit;

    //Lerping Colour
    public Color colourStart = Color.white;
    public Color colourEnd = Color.green;
    private float lerpTime = 1.0f;
    private float timer = 10.0f;
    public Text scoreText;
    public int score;

	void Start ()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");
    }
	
	void Update ()
    {
       timer -= 0.1f;
       
       if (timer <= 0.0f)
       {
           RandomGameObject();
       }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
       {
            if (hit.collider.gameObject == buttons[randomButtons].gameObject)
            {
                score += 1;
                scoreText.text = "Score: " + score.ToString();
                buttons[randomButtons].GetComponent<Renderer>().material.color = Color.Lerp(colourEnd, colourStart, Time.time * lerpTime);
            }
            else if(hit.collider.gameObject != buttons[randomButtons].gameObject)
            {
                //Pause the game and bring up Game Over screen
            }
            else
            {
                //If player does not press on square fast enough Game Over
            }
        }
    }

    void RandomGameObject()
    {
        //Randomly find choose a number between 0 and the array length of 9
        randomButtons = Random.Range(0, buttons.Length);
    
        //randomButtons now has it's random gameobject, find the renderer, change it's material to lerp between white to green and vice versa
        buttons[randomButtons].GetComponent<Renderer>().material.color = Color.Lerp(colourStart, colourEnd, Time.time * lerpTime);

        //reset timer. TO DO make random time
        timer = 10.0f;
    
        Debug.Log(randomButtons);
    }


}