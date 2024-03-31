using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;
    private string newGameScene = "Map_v1";

    public AudioClip bg_music;
    public AudioSource main_channel;

    // Start is called before the first frame update
    void Start()
    {
        main_channel.PlayOneShot(bg_music);
        //Set the high score text
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Round Survived: {highScore}";
    }


    public void StartNewGame()
    {
        main_channel.Stop();
        SceneManager.LoadScene(newGameScene); 
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitApplication()
    {

        Application.Quit();

    }
}
