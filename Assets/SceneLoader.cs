using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void LoadSplashScreen()
    {
        LoadScene("SplashScreen");
    }

    public void LoadAuth()
    {
        LoadScene("Auth");
    }
    public void LoadLogin()
    {
        LoadScene("Login");
    }

    public void LoadRegister()
    {
        LoadScene("Register");
    }

    public void LoadHome()
    {
        LoadScene("Game");
    }

    public void LoadProfil()
    {
        LoadScene("Profil");
    }

    
}

public enum GameScene
{
    SPLASH_SCREEN, AUTH, MENU, GAME
}
