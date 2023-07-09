using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeController : MonoBehaviour
{
    [SerializeField] private AllPlayersSO allPlayersSO;
    [SerializeField] private float timeBetwenChangingPlayer = 5f;
    private float timerBetwenChangingPlayer;

    public static PlayerChangeController Instance { get; private set; }
    public event EventHandler OnPLayerRechargeDone;

    [SerializeField] private PlayerSO currentPlayer;
    private List<int> playerLives = new List<int>();
    [SerializeField] private Transform currentPlayerTransfrom;

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

    private void Update()
    {
        if (!IsChangeRecharged())
        {
            timerBetwenChangingPlayer -= Time.deltaTime;
            if (IsChangeRecharged())
                OnPLayerRechargeDone?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChangePlayer(PlayerSO playerToChange)
    {
        if (IsChangeRecharged())
        {
            if (currentPlayer != playerToChange)
            {
                currentPlayer = playerToChange;

                Destroy(currentPlayerTransfrom?.gameObject);

                currentPlayerTransfrom = Instantiate(playerToChange.playerPrefab);
                for (int i = 0; i < allPlayersSO.allPlayersSO.Length; i++)
                {
                    if (currentPlayer == allPlayersSO.allPlayersSO[i])
                        currentPlayerTransfrom.GetComponent<PlayerController>().ChangeCurrentHearts(playerLives[i]);
                }
                currentPlayerTransfrom.position = CheckpointsController.Instance.GetCurrentCheckpoint().gameObject.transform.position;
                CameraFollowing.Instance.ChangeFollowingPlayer(currentPlayerTransfrom);

                timerBetwenChangingPlayer = timeBetwenChangingPlayer;
            }
        }
    }

    public PlayerSO GetCurrentPlayerSO() 
    {
        return currentPlayer; 
    }

    public bool IsChangeRecharged()
    {
        return timerBetwenChangingPlayer <= 0f;
    }

    public AllPlayersSO GetAllPlayersSO()
    {
        return allPlayersSO;
    }
}
