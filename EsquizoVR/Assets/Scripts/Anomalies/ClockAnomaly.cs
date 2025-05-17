using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnomaly : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Clock"]);
    }
}
