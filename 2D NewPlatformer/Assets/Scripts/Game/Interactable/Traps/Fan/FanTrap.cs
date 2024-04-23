using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FanTrapVisual))]
public class FanTrap : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;

    [SerializeField] private Vector2 blowingDirection = Vector2.up;
    [SerializeField] private float blowingSpeed = 5f;
    private List<PlayerMovement> blowingPlayersList = new List<PlayerMovement>();
    private FanTrapVisual fanTrapVisual;
    private FanParticles fanParticles;

    private void Awake()
    {
        fanTrapVisual = GetComponent<FanTrapVisual>();
        fanParticles = GetComponent<FanParticles>();
    }

    private void Start()
    {
        fanTrapVisual.ChangeAnimationState(isEnabled);
        if (fanParticles != null)
        {
            fanParticles.ChangeDirection(blowingDirection);
            fanParticles.ChangeParticlesState(isEnabled);
        }
    }

    private void Update()
    {
        if (isEnabled)
            if (blowingPlayersList.Count > 0)
            {
                foreach (PlayerMovement player in blowingPlayersList)
                {
                    if (blowingDirection.y == 0)
                        player.Move(blowingDirection * blowingSpeed * Time.deltaTime);
                    else
                    {
                        player.Jump(blowingDirection.y * blowingSpeed);
                    }
                }
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement blowingPlayer;
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out blowingPlayer))
        {
            blowingPlayersList.Add(blowingPlayer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement blowingPlayer;
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out blowingPlayer))
        {
            blowingPlayersList.Remove(blowingPlayer);
        }
    }

    public void ChangeFanEnabledToggle(bool newState)
    {
        isEnabled = newState;
        fanTrapVisual.ChangeAnimationState(newState);
        fanParticles.ChangeParticlesState(isEnabled);
    }

    public bool GetFanTrapCurrentState()
    {
        return isEnabled;
    }

    public Vector2 GetCurrentBlowingDirection()
    {
        return blowingDirection;
    }

    public void ChangeCurrentBlowingDirection(Vector2 newBlowingDirection, Quaternion newFanRotation, Vector3 additionalMovement)
    {
        blowingDirection = newBlowingDirection;
        fanParticles.ChangeDirection(blowingDirection);
        transform.rotation = newFanRotation;
        transform.position += additionalMovement;
    }
}
