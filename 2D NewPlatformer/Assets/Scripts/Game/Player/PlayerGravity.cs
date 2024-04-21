using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private float gravityScale = 1f;
    private float gravityConst = -9.8f;
    private float slowDownScale = 1f;

    private Vector3 gravityVector;
    [SerializeField] private BoxCollider2D playerCollision;

    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask additinalTerrainsLayer;

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
            if(gravityVector.y > 0)
                transform.position += gravityVector * Time.deltaTime * slowDownScale;
            else
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
        Vector2 castPosition = transform.position + (Vector3)playerCollision.offset + new Vector3(0f, -playerCollision.bounds.size.y * 4.5f / 10, 0f);
        Vector2 vectorCastSize =  new Vector2(playerCollision.bounds.size.x * 0.5f, playerCollision.bounds.size.y / 10);   
        float angleRotation = 0f;
        Vector2 castDirection = Vector2.down;
        float additionalCastRange = .02f;

        RaycastHit2D[] allHits = Physics2D.BoxCastAll(castPosition, vectorCastSize,
            angleRotation, castDirection, additionalCastRange, terrainLayer);
        int collidibleObjects = 0;
        foreach (var hit in allHits)
        {
            if (!hit.collider.isTrigger)
                collidibleObjects++;
        }

        if (collidibleObjects > 0)
            return true;
        else
            return Physics2D.BoxCast(castPosition, vectorCastSize, angleRotation,
                castDirection, additionalCastRange, additinalTerrainsLayer);
    }

    public void SetYInGravityVector(float yGravity)
    {
        gravityVector.y = yGravity;
    }

    public Vector3 GetGravityVector()
    {
        return gravityVector;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector2 castPosition = transform.position + (Vector3)playerCollision.offset + new Vector3(0f, -playerCollision.bounds.size.y * 4.5f / 10, 0f);
        Vector2 vectorCastSize = new Vector2(playerCollision.bounds.size.x * 0.5f, playerCollision.bounds.size.y / 10);
        Gizmos.DrawCube(castPosition, vectorCastSize);
    }
}
