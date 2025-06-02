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
    public List<bool> isAnomaly;

    public bool isNotebookOpen = false;

    private int currentPage = 0;
    private const int photosPerPage = 9;

    public InputActionReference nextPageAction;
    public InputActionReference prevPageAction;

    private Transform cameraTransform;
    private Rigidbody rb;
    private Transform notebookAnchor;

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

        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        notebookAnchor = new GameObject("NoteBookAnchor").transform;
        notebookAnchor.SetParent(cameraTransform);
        notebookAnchor.localPosition = new Vector3(-0.2f, -0.3f, 0.6f);
        notebookAnchor.localRotation = Quaternion.identity;
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
        if (notebookAnchor == null || rb == null) return;

        if (!rb.isKinematic) rb.isKinematic = true;

        transform.position = notebookAnchor.position;
        transform.rotation = notebookAnchor.rotation;
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
        int columns = 3; // Number of columns in the grid
        float spacing = 0.1f; // Spacing between photos
        float rowSpacing = 0.1f; // Spacing between rows
        Vector3 forwardOffset = transform.up * 0.15f; // Offset to position the grid in front of the notebook
        Vector3 startPosition = transform.position + forwardOffset + new Vector3(-0.1f, 0, 0.1f); // Start position for the grid
        Quaternion notebookRotation = transform.rotation; // Align photos with notebook rotation

        // Hide all photos first
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

            // Calculate position relative to the notebook
            Vector3 position = startPosition
                + transform.right * (column * spacing) // Offset horizontally
                - transform.forward * (row * rowSpacing);  // Offset vertically

            cameraShots[i].SetActive(true);
            cameraShots[i].transform.position = position;
            cameraShots[i].transform.rotation = notebookRotation; // Align rotation with notebook
            cameraShots[i].transform.rotation = Quaternion.Euler(90, 0, 0); // Flip the photo to face the player
        }
    }

}

