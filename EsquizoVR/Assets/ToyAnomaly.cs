using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyAnomaly : Anomaly
{
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Toy"]);
    }
}
