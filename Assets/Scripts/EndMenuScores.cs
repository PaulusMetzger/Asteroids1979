using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenuScores : MonoBehaviour
{
    Text endText;


    // Start is called before the first frame update
    void Start()
    {
        endText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       if(gameObject.activeSelf)
        {
            string Number = Convert.ToString(AsteroidManager.Scores);
            endText.text = "You collect " + Number + " asteroids scores.";
        }
    }
}
