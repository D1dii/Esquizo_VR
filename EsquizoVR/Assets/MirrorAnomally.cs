using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAnomally : Anomaly
{
    public bool playerHasEntered = false;
    public bool alreadyPlayed = false;
    public Material mirrorMaterial;

    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Mirror"]);
    }

    private void Update()
    {
        // Aquí podrías agregar lógica adicional si es necesario
        if (alreadyPlayed == true)
        {
            // cambiar el material
            mirrorMaterial.color = Color.red; // Cambia el color del material a rojo
        }
    }
}

