using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleActiveButtonUI : MonoBehaviour
{
    [SerializeField] private Input.Binding trackedBinding;
    [SerializeField] private Transform lockedTransform;

    private void Start()
    {
        PlayerController.OnLockedBindingChange += PlayerController_OnLockedBindingChange;

        lockedTransform.gameObject.SetActive(false);
    }

    private void PlayerController_OnLockedBindingChange(object sender, PlayerController.OnLockedBindingChangeEventArgs e)
    {
        if(e.lockedBinding.Contains(trackedBinding))
            lockedTransform.gameObject.SetActive(true);
        else
            lockedTransform.gameObject.SetActive(false);
    }
}
