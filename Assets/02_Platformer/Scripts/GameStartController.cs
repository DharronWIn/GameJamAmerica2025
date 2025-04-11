using System.Linq;
using Fusion;
using Starter;
using TMPro;
using UnityEngine;

public class GameStartController : NetworkBehaviour
{
    [Networked]
    public bool GameStarted { get; set; }

    public int MaxPlayers = 2;
    public int CurrentPlayersCount;
    public TMP_Text playerConnectedText;

    public UIGameMenu uIGameMenu;

    private void OnEnable()
    {
        if (playerConnectedText != null) playerConnectedText.text = 1 + "/" + MaxPlayers.ToString();
    }

    public override void FixedUpdateNetwork()
    {
        CurrentPlayersCount = Runner.ActivePlayers.Count();
        if (playerConnectedText != null) playerConnectedText.text = CurrentPlayersCount.ToString() + "/" + MaxPlayers.ToString();

        if (HasStateAuthority && !GameStarted)
        {
            if (Runner.ActivePlayers.Count() >= MaxPlayers)
            {
                Debug.Log("All players joined. Starting game...");
                GameStarted = true;
                OnPlayerCompled();
            }
        }

        if (!GameStarted)
        {
            // Optional : freeze input or movement here
            // Or show a “Waiting for players…” UI
        }
    }

    public void OnPlayerCompled()
    {
        Debug.Log("Players completed: " + Runner.ActivePlayers.Count());
        uIGameMenu.LunchGame();

    }

}

