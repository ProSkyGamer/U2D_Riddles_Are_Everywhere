using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAddInteractButton : AddInteractButtonUI
{
    public event EventHandler OnInteractButtonAdded;
    public event EventHandler OnInteractButtonRemoved;

    protected override void AddInteractButtonToInterafce()
    {
        InteractInterface.Instance.AddButtonInteractToScreen(this, buttonTextTranslationsSO);
        isHasButtonOnInterface = true;
        OnInteractButtonAdded?.Invoke(this, EventArgs.Empty);

    }

    public override void RemoveInteractButtonFromInterafce()
    {
        InteractInterface.Instance.RemoveButtonInteractToScreen(this);
        isHasButtonOnInterface = false;
        OnInteractButtonRemoved?.Invoke(this, EventArgs.Empty);
    }
}
