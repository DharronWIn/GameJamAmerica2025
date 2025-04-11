using UnityEngine;
using TMPro;
using Starter;
public class MenuController : MonoBehaviour
{
    public TMP_Text PlayerNameText;
    public TMP_Text ScoreText;

    public GameObject lobbyPanel;
    public GameObject mainMenuPanel;

    public UIGameMenu uIGameMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // Load player name from PlayerPrefs and display it in the UI
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string playerName = PlayerPrefs.GetString("PlayerName");
            PlayerNameText.text = playerName;
        }
        else
        {
            PlayerNameText.text = "Guest";
        }

        // Load score from PlayerPrefs and display it in the UI
        if (PlayerPrefs.HasKey("Score"))
        {
            int score = PlayerPrefs.GetInt("Score");
            ScoreText.text = score.ToString();
        }
        else
        {
            ScoreText.text = "0";
        }
        
    }

    public void OnPlayButtonClicked()
    {
       lobbyPanel.SetActive(true);
       uIGameMenu.StartGame();
    }
}
