using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlockGravity : MonoBehaviour
{
    public event EventHandler OnBreakableBlockGroundHit;
    public event EventHandler OnBreakableBlockDeathGroundHit;

    [SerializeField] private float gravityScale = 1f;
    private float gravityConst = -9.8f;

    [SerializeField] private float minDistanceToBreak = 2f;
    private Vector3 startFallingPosition;
    private Vector3 gravityVector;
    private BoxCollider2D boxCollision;

    [SerializeField] private LayerMask terrainLayer;

    private void Awake()
    {
        boxCollision = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        GravityMove();
    }

    private void GravityMove()
    {
        if (!IsGrounded())
        {
            if (gravityVector.y == 0)
            {
                startFallingPosition = transform.position;
            }

            transform.position += gravityVector * gravityScale * Time.deltaTime;

            if (gravityVector.y > gravityConst)
            {
                gravityVector += new Vector3(0f, gravityConst, 0f) * Time.deltaTime;

                if (gravityVector.y < gravityConst)
                    gravityVector.y = gravityConst;
            }
        }
        else
        {
            if (gravityVector.y != 0)
            {
                gravityVector.y = 0;
                if (startFallingPosition.y - transform.position.y >= minDistanceToBreak)
                    OnBreakableBlockDeathGroundHit?.Invoke(this, EventArgs.Empty);
                else
                    OnBreakableBlockGroundHit?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool IsGrounded()
    {
        Vector2 castPosition = transform.position + (Vector3)boxCollision.offset + new Vector3(0f, -boxCollision.bounds.size.y * 4.5f / 10, 0f); ;
        Vector2 vectorCastSize = new Vector2(boxCollision.bounds.size.x * 0.5f, boxCollision.bounds.size.y / 10);
        float angleRotation = 0f;
        Vector2 castDirection = Vector2.down;
        float additionalCastRange = 0f;

        RaycastHit2D[] allHits = Physics2D.BoxCastAll(castPosition, vectorCastSize,
            angleRotation, castDirection, additionalCastRange, terrainLayer);
        int collidibleObjects = 0;
        foreach(var hit in allHits)
        {
            if (!hit.collider.isTrigger)
                collidibleObjects++;
        }

        if(!boxCollision.isTrigger)
            return collidibleObjects > 1;
        else
            return collidibleObjects > 0;
    }
}

