using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.DeleteAll(); // Clear PlayerPrefs for testing purposes
        SceneLoader.instance.LoadAuth();
        // if (PlayerPrefs.HasKey("PlayerName"))
        // {
        //     string playerName = PlayerPrefs.GetString("PlayerName");
        //     Debug.Log("Player Name: " + playerName);
        //     SceneLoader.instance.LoadHome();
        // }
        // else
        // {
        //     Debug.Log("No Player Name found in PlayerPrefs.");

        // }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
