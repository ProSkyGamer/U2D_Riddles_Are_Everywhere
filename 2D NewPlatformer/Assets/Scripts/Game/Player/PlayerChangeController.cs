using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeController : MonoBehaviour
{
    [SerializeField] private bool isPlayerChangeEnabled = true;
    [SerializeField] private AllPlayersSO allPlayersSO;
    [SerializeField] private List<PlayerSO> alwaysAvailiblePlayers = new();

    public static PlayerChangeController Instance { get; private set; }

    [SerializeField] private PlayerSO currentPlayer;
    private List<int> playerLives = new List<int>();
    [SerializeField] private Transform currentPlayerTransfrom;
    private PlayerController currentPlayerController;

    public event EventHandler OnPlayerChange;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        for (int i = 0; i < allPlayersSO.allPlayersSO.Length; i++)
        {
            playerLives.Add(allPlayersSO.allPlayersSO[i].playerPrefab.gameObject.
                GetComponentInChildren<PlayerController>().GetMaxHearts());
        }

        currentPlayerController = currentPlayerTransfrom.GetComponent<PlayerController>();
    }

    private void Start()
    {
        PlayerController.OnPlayerHealthChange += PlayerController_OnPlayerHealthChange;
    }

    private void PlayerController_OnPlayerHealthChange(object sender, PlayerController.OnPlayerHealthChangeEventArgs e)
    {
        for (int i = 0; i < allPlayersSO.allPlayersSO.Length; i++)
        {
            if(currentPlayer == allPlayersSO.allPlayersSO[i])
                playerLives[i] = e.currentHealth;
        }
    }

    public bool IsCanChangePlayer()
    {
        return isPlayerChangeEnabled;
    }

    public void ChangePlayer(PlayerSO playerToChange)
    {
        if (currentPlayerController.IsCanTeleportToCheckpoint())
        {
            if (currentPlayer != playerToChange)
            {
                currentPlayer = playerToChange;

                Destroy(currentPlayerTransfrom?.gameObject);

                currentPlayerTransfrom = Instantiate(playerToChange.playerPrefab);
                currentPlayerController = currentPlayerTransfrom.GetComponent<PlayerController>();
                for (int i = 0; i < allPlayersSO.allPlayersSO.Length; i++)
                {
                    if (currentPlayer == allPlayersSO.allPlayersSO[i])
                        currentPlayerController.ChangeCurrentHearts(playerLives[i]);
                }
                currentPlayerController.TeleportToCurrentCheckpoint();
                CameraFollowing.Instance.ChangeFollowingPlayer(currentPlayerTransfrom);

                OnPlayerChange?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public PlayerSO GetCurrentPlayerSO() 
    {
        return currentPlayer; 
    }

    public AllPlayersSO GetAllPlayersSO()
    {
        return allPlayersSO;
    }

    public PlayerController GetCurrentPlayerController()
    {
        return currentPlayerController;
    }

    public List<PlayerSO> GetAlwaysAvailiblePlayers()
    {
        return alwaysAvailiblePlayers;
    }
}
