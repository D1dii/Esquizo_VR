using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public int currentPage = 0;
    private const int photosPerPage = 9;

    public InputActionReference nextPageAction;
    public InputActionReference prevPageAction;

    private Transform cameraTransform;
    private Rigidbody rb;
    private Transform notebookAnchor;

    public Animator notebookAnimator;

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
        notebookAnchor.SetParent(null);
        notebookAnchor.localRotation = Quaternion.Euler(-90, 0, 0);
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
        notebookAnimator = GetComponent<Animator>();
    }

    public void Update()
    {

        if (SelectFinalShotsManager.instance.selectingFinalShots == true) { return; }

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
        if (notebookAnchor == null || rb == null || cameraTransform == null) return;

        if (!rb.isKinematic) rb.isKinematic = true;

        // Posici�n frente al jugador solo con yaw
        Vector3 offset = new Vector3(-0.2f, -0.5f, 0.3f);
        Quaternion yawOnly = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        Vector3 targetPosition = cameraTransform.position + yawOnly * offset;

        notebookAnchor.position = targetPosition;
        transform.position = notebookAnchor.position;

        // Aplicar solo rotaci�n horizontal
        transform.rotation = yawOnly;
    }

    public void OpenNotebook()
    {
        if (interactable.isSelected && !SelectFinalShotsManager.instance.selectingFinalShots)
        {
            if (isNotebookOpen)
            {
                CloseNotebook();
            }
            else
            {
                if (notebookAnimator != null)
                {
                    notebookAnimator.SetTrigger("IsOpening");
                }
                else
                {
                    Debug.LogWarning("Notebook Animator is not assigned.");
                }
                isNotebookOpen = true;
                currentPage = 0;
                ArrangeCameraShotsInGrid();
                Debug.Log("Notebook opened");
            }
        }
    }

    public void CloseNotebook()
    {
        if (notebookAnimator != null)
        {
            notebookAnimator.SetTrigger("IsClosing");
        }
        else
        {
            Debug.LogWarning("Notebook Animator is not assigned.");
        }
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
        float spacing = 0.1f;
        Vector3 startPosition = transform.position + new Vector3(-0.1f, 0.1f, 0.1f);
        Quaternion extraRotation = Quaternion.Euler(90, 0, 0);


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

            Vector3 position = startPosition + new Vector3(column * spacing, 0, -row * spacing);

            cameraShots[i].SetActive(true);
            cameraShots[i].transform.position = position;
            cameraShots[i].transform.rotation = transform.rotation * extraRotation;
        }
    }

    public void DeletePhotos()
    {
        for (int i = 0; i < cameraShots.Count; i++)
        {
            if (cameraShots[i] != null)
            {
                Destroy(cameraShots[i]);
                cameraShots.RemoveAt(i);
            }
        }
        currentPage = 0;

    }
}

