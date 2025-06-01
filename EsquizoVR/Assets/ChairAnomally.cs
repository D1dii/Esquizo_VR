using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairAnomally : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Chairs"]);
    }
}
