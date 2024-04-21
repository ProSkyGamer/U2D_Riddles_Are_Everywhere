using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReserStaticData
{
    public static void ResetStaticData()
    {
        PlayerController.ResetStaticData();
        TextTranslationManager.ResetStaticData();
        CoinsManager.ResetStaticData();
        MovableHead.ResetStaticData();
        BreakableBlock.ResetStaticData();
    }
}
