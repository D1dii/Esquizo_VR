using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAnomaly : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Empty"]);
    }
}
