using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ListenForButtonRebind : MonoBehaviour
{
    [SerializeField] private Input.Binding listeningBinding;
    private TextMeshProUGUI buttonBindingText;

    private void Start()
    {
        buttonBindingText = GetComponent<TextMeshProUGUI>();

        Input.Instance.OnBindingRebing += Input_OnBindingRebing;
    }

    private void Input_OnBindingRebing(object sender, Input.OnBindingRebingEventArgs e)
    {
        if(e.bingingChanged == listeningBinding)
        {
            buttonBindingText.text = Input.Instance.GetBindingText(listeningBinding);
        }
    }

    private void OnDestroy()
    {
        Input.Instance.OnBindingRebing -= Input_OnBindingRebing;
    }
}
