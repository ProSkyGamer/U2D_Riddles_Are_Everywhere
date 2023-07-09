using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerGravity))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;

    private PlayerGravity playerGravity;

    private float slowDownScale = 1f;

    private void Awake()
    {
        playerGravity = GetComponent<PlayerGravity>();
    }

    public void Move(Vector3 toMoveVector)
    {
        transform.position += toMoveVector * slowDownScale;
    }

    public void Jump(float yJumpForce)
    {
        playerGravity.SetYInGravityVector(yJumpForce);
    }

    public void ChangeSlowDownMovement(float slowDown)
    {
        slowDownScale = slowDown;
    }

    public void ChangeSlowDownGravity(float slowDown)
    {
        playerGravity.ChangeGravitySlowDown(slowDown);
    }

    public bool IsGrounded()
    {
        return playerGravity.IsGrounded();
    }

    public bool IsAscending()
    {
        return playerGravity.GetGravityVector().y > .1f;
    }

    public bool IsDescending()
    {
        return playerGravity.GetGravityVector().y < -.1f;
    }
}
