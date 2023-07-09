using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerSO : ScriptableObject
{
    public Transform playerPrefab;
    public Sprite playerSprite;
    public string playerName;
    public string playerDescriptionText;
}
