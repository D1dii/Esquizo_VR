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

    private int currentPage = 0;
    private const int photosPerPage = 9;

    public InputActionReference nextPageAction;
    public InputActionReference prevPageAction;

    public void Awake()
    {
        openNotebook.action.Enable();
        openNotebook.action.performed += ctx => OpenNotebook();

        if (nextPageAction != null)
        {
            nextPageAction.action.Enable();
            nextPageAction.action.performed += ctx => NextPage();
        }
        if (prevPageAction != null)
        {
            prevPageAction.action.Enable();
            prevPageAction.action.performed += ctx => PreviousPage();
        }
    }

    public void OnDestroy()
    {
        openNotebook.action.performed -= ctx => OpenNotebook();
        openNotebook.action.Disable();

        if (nextPageAction != null)
        {
            nextPageAction.action.performed -= ctx => NextPage();
            nextPageAction.action.Disable();
        }
        if (prevPageAction != null)
        {
            prevPageAction.action.performed -= ctx => PreviousPage();
            prevPageAction.action.Disable();
        }
    }

    public void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    public void Update()
    {
        if (!interactable.isSelected)
        {
            FollowCharacterNotGrabbed();
            isNotebookOpen = false;
        }

        if (isNotebookOpen)
        {
            ArrangeCameraShotsInGrid();
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
                currentPage = 0;
                ArrangeCameraShotsInGrid();
                Debug.Log("Notebook opened");
            }
        }
    }

    public void CloseNotebook()
    {
        isNotebookOpen = false;
        Debug.Log("Notebook closed");
    }

    public void NextPage()
    {
        int maxPage = (cameraShots.Count - 1) / photosPerPage;
        if (currentPage < maxPage)
        {
            currentPage++;
            ArrangeCameraShotsInGrid();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ArrangeCameraShotsInGrid();
        }
    }

    public void ArrangeCameraShotsInGrid()
    {
        int columns = 3;
        float spacing = 0.2f;
        Vector3 startPosition = transform.position + new Vector3(-0.3f, 0.1f, 0);
        Quaternion extraRotation = Quaternion.Euler(90, 0, 0);

        // Oculta todas las fotos primero
        for (int i = 0; i < cameraShots.Count; i++)
        {
            cameraShots[i].SetActive(false);
        }

        int startIdx = currentPage * photosPerPage;
        int endIdx = Mathf.Min(startIdx + photosPerPage, cameraShots.Count);

        for (int i = startIdx; i < endIdx; i++)
        {
            int localIndex = i - startIdx;
            int row = localIndex / columns;
            int column = localIndex % columns;

            Vector3 position = startPosition + new Vector3(column * spacing, 0, -row * spacing);

            cameraShots[i].SetActive(true);
            cameraShots[i].transform.position = position;
            cameraShots[i].transform.rotation = transform.rotation * extraRotation;
        }
    }
}

