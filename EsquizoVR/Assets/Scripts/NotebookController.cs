using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class NotebookController : MonoBehaviour
{

    public Transform playerTransform;
    public XRGrabInteractable interactable;
    public InputActionReference openNotebook;

    public List<GameObject> cameraShots;

    public bool isNotebookOpen = false;

    public void Awake()
    {
        openNotebook.action.Enable();
        openNotebook.action.performed += ctx => OpenNotebook();
    }

    public void OnDestroy()
    {
        openNotebook.action.performed -= ctx => OpenNotebook();
        openNotebook.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactable.isSelected)
        {
            FollowCharacterNotGrabbed();
        }

        if (isNotebookOpen)
        {
            for (int i = 0; i < cameraShots.Count; i++)
            {
                cameraShots[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < cameraShots.Count; i++)
            {
                cameraShots[i].SetActive(false);
            }
        }
    }

    public void FollowCharacterNotGrabbed()
    {
        transform.position = new Vector3(playerTransform.position.x + 0.5f, transform.position.y, playerTransform.position.z - 0.5f);
    }

    public void OpenNotebook()
    {
        if (interactable.isSelected)
        {
            if (isNotebookOpen)
            {
                CloseNotebook();
            }
            else
            {
                isNotebookOpen = true;
                ArrangeCameraShotsInGrid();
                // Logic to open the notebook
                Debug.Log("Notebook opened");
            }
        }
        
    }

    public void CloseNotebook()
    {
        isNotebookOpen = false;
        // Logic to close the notebook
        Debug.Log("Notebook closed");
    }

    public void ArrangeCameraShotsInGrid()
    {
        int columns = 3; // Número de columnas
        float spacing = 1.5f; // Espaciado entre los elementos
        Vector3 startPosition = transform.position + new Vector3(-1.5f, 0, 1.5f); // Posición inicial de la cuadrícula

        for (int i = 0; i < cameraShots.Count; i++)
        {
            int row = i / columns; // Calcula la fila
            int column = i % columns; // Calcula la columna

            // Calcula la posición en la cuadrícula
            Vector3 position = startPosition + new Vector3(column * spacing, 0, -row * spacing);

            // Mueve el objeto a la posición calculada
            cameraShots[i].transform.position = position;
        }
    }

}
