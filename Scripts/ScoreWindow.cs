using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreWindow : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake() {
        scoreText = transform.Find("scoreText").GetComponent<TMP_Text>();
    }

    private void Update() {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
    }
}
