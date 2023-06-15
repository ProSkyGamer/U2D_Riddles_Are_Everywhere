using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;

    private Rigidbody2D playerRB;
    private BoxCollider2D playerCollision;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector3 toMoveVector)
    {
        transform.position += toMoveVector;
    }

    public void Jump(Vector3 jumpForceVector)
    {
        playerRB.velocity = jumpForceVector;
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollision.bounds.center, playerCollision.bounds.size, 0f, Vector2.down, .02f, terrainLayer);
    }

    public bool IsAscending()
    {
        return playerRB.velocity.y > .1f;
    }

    public bool IsDescending()
    {
        return playerRB.velocity.y < -.1f;
    }
}
