using System.Collections.Generic;
using UnityEngine;

public class FanParticles : MonoBehaviour
{
    [SerializeField] private Transform creatingParticleTransform;
    [SerializeField] private Transform creatingParticlePosition;
    [SerializeField] private float creatingParticleInterval = 1.5f;
    private float creatingParticleTimer;
    [SerializeField] private float creatingParticleSpeed = 1f;
    [SerializeField] private float maxParticleDistance;
    private Vector2 blowingDirection;

    private bool isParticlesOn;

    private class Particle
    {
        public Transform particleTransform;
        public float particleDistance;
    }

    private readonly List<Particle> allCreatedParticles = new();

    public void ChangeDirection(Vector2 newDirection)
    {
        blowingDirection = newDirection;
    }

    public void ChangeDistance(float newDistance)
    {
        maxParticleDistance = newDistance;
    }

    public void ChangeParticlesState(bool newState)
    {
        isParticlesOn = newState;
        foreach (var createdParticle in allCreatedParticles) Destroy(createdParticle.particleTransform.gameObject);
        allCreatedParticles.Clear();
    }

    private void Awake()
    {
        creatingParticleTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isParticlesOn) return;

        var deltaPosition = (Vector3)blowingDirection * creatingParticleSpeed * Time.deltaTime;
        for (var i = 0; i < allCreatedParticles.Count; i++)
        {
            var createdParticle = allCreatedParticles[i];
            if (createdParticle.particleDistance >= maxParticleDistance)
            {
                Destroy(createdParticle.particleTransform.gameObject);
                allCreatedParticles.RemoveAt(i);
                i--;
            }

            createdParticle.particleTransform.position += deltaPosition;
            createdParticle.particleDistance += deltaPosition.magnitude;
        }

        creatingParticleTimer -= Time.deltaTime;
        if (creatingParticleTimer <= 0)
        {
            creatingParticleTimer = creatingParticleInterval;
            var newCreatingParticleTransform = Instantiate(creatingParticleTransform, creatingParticlePosition.position,
                Quaternion.identity);
            newCreatingParticleTransform.gameObject.SetActive(true);
            var newParticle = new Particle
            {
                particleTransform = newCreatingParticleTransform,
                particleDistance = 0
            };
            allCreatedParticles.Add(newParticle);
        }
    }
}
