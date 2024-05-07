using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{
    private void Start() {
        transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.GameScene);
        };

        transform.Find("quitBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Application.Quit();
        };
    }

}
