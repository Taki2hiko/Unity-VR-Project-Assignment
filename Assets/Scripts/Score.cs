using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.SetText("Score: " + score);
    }

    
}
