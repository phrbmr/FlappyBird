using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake() {
        scoreText = transform.Find("ScoreText").GetComponent<TMP_Text>();

        transform.Find("RetryBtn").GetComponent<Button_UI>().ClickFunc = () => { 
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); 
        };
    }

    private void Start() {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        Show();
    }

    private void Hide() {
        Debug.Log("Hide method called");
        gameObject.SetActive(false);
    }

    private void Show() {
        Debug.Log("Show method called");
        gameObject.SetActive(true);
    }
}
