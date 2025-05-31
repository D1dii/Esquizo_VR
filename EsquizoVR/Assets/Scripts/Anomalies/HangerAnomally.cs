using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerAnomally : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Hanger"]);
    }
}
