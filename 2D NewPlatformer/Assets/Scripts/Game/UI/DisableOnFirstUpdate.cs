using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnFirstUpdate : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void LateUpdate()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            gameObject.SetActive(false);
        }
    }
}
