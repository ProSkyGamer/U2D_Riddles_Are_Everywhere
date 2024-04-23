using UnityEngine;

[RequireComponent(typeof(PlayerGravity))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;

    private PlayerGravity playerGravity;

    private float slowDownScale = 1f;

    private bool isForcedMovement;

    private void Awake()
    {
        playerGravity = GetComponent<PlayerGravity>();
    }

    public void Move(Vector3 toMoveVector, bool isForcedMovement = false)
    {
        this.isForcedMovement = isForcedMovement;

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

    public bool IsMovementForced()
    {
        return isForcedMovement;
    }
}
