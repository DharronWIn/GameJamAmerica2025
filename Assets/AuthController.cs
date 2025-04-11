using UnityEngine;
using TMPro;

public class AuthController : MonoBehaviour
{
    public TMP_InputField NicknameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegisterButtonClicked()
    {
        string nickname = NicknameText.text;
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogError("Nickname cannot be empty!");
            return;
        }
        PlayerPrefs.SetString("PlayerName", nickname);
        SceneLoader.instance.LoadHome();
        Debug.Log("Nickname entered: " + nickname);
    }
}
