using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAnomally : Anomaly
{
    public bool playerHasEntered = false;
    public bool alreadyPlayed = false;

    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Mirror"]);
    }
}
