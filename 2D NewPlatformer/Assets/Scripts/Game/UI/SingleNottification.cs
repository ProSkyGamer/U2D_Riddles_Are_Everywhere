using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleNottification : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private TextMeshProUGUI nottificationText;

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 1 && lifetime >= 0)
                nottificationText.alpha = lifetime;
        }
        else
            Destroy(gameObject);
    }
}
