using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkAnomally : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Box"]);
    }
}
