using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CradleAnomaly : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Cradle"]);
    }
}
