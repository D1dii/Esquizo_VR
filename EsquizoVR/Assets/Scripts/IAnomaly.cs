using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnomaly
{
    public Transform spawnPos { get; set; }
    public void OnAnomalyStart();
}
