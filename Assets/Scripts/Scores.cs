using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        string Number = Convert.ToString(AsteroidManager.Scores);
        scoreText.text = Number;
    }
}
