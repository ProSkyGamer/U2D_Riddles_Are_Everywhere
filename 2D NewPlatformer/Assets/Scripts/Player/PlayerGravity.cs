using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private float gravityScale = 1f;
    private float gravityConst = -9.8f;
    private float slowDownScale = 1f;

    private Vector3 gravityVector;
    private BoxCollider2D playerCollision;

    [SerializeField] private LayerMask terrainLayer;

    private void Awake()
    {
        playerCollision = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        GravityMove();
    }

    private void GravityMove()
    {
        if (!IsGrounded() || gravityVector.y > 0)
        {
            transform.position += gravityVector * gravityScale * Time.deltaTime * slowDownScale;

            if (gravityVector.y > gravityConst)
            {
                gravityVector += new Vector3(0f, gravityConst, 0f) * Time.deltaTime * slowDownScale;

                if (gravityVector.y < gravityConst)
                    gravityVector.y = gravityConst;
            }
        }
        else
        {
            gravityVector.y = 0;
        }
    }

    public void ChangeGravitySlowDown(float slowDon)
    {
        slowDownScale = slowDon;
    }

    public bool IsGrounded()
    {
        Vector2 castPosition = playerCollision.bounds.center;
        Vector2 vectorCastSize = playerCollision.bounds.size;
        float angleRotation = 0f;
        Vector2 castDirection = Vector2.down;
        float additionalCastRange = .02f;

        return Physics2D.BoxCast(castPosition, vectorCastSize, angleRotation, castDirection, additionalCastRange, terrainLayer);
    }

    public void SetYInGravityVector(float yGravity)
    {
        gravityVector.y = yGravity;
    }

    public Vector3 GetGravityVector()
    {
        return gravityVector;
    }
}
