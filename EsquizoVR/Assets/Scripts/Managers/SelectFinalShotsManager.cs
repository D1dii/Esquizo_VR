using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectFinalShotsManager : MonoBehaviour
{
    public static SelectFinalShotsManager instance;
    public bool selectingFinalShots = false;

    private Transform cameraTransform;
    [SerializeField] private NotebookController notebookController;

    public GameObject escritorio;

    public List<bool> finalShots;

    public float maxSelections;
    public float currentSelections = 0;

    public InputActionReference confirmSelectionAction;

    public SpriteRenderer fadeToBlackImage;

    private void Awake()
    {

        instance = this;
        confirmSelectionAction.action.Enable();
        confirmSelectionAction.action.performed += ctx => CheckPhotosSelected();

    }

    private void OnDestroy()
    {
        confirmSelectionAction.action.performed -= ctx => CheckPhotosSelected();
        confirmSelectionAction.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        selectingFinalShots = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectingFinalShots)
        {
            maxSelections = LevelManager.instance.CurrentAnomaliesOnLevel;
            // Arrange photos on the desk
            ArrangePhotosOnDesk();

            notebookController.transform.rotation = Quaternion.identity;

            
            
        }

        if (currentSelections != maxSelections)
        {
            confirmSelectionAction.action.Disable();
        }
        else
        {
            confirmSelectionAction.action.Enable();
        }
    }

    private void CheckPhotosSelected()
    {
        bool allAnomaliesCorrectlySelected = true;

        for (int i = 0; i < notebookController.cameraShots.Count; i++)
        {
            PhotoController photoController = notebookController.cameraShots[i].GetComponent<PhotoController>();
            if (photoController != null && photoController.isSelected)
            {
                finalShots.Add(photoController.hasAnomaly);
            }
        }

        for (int i = 0; i < LevelManager.instance.CurrentAnomaliesOnLevel; i++)
        {
            if (finalShots[i] == false)
            {
                //allAnomaliesCorrectlySelected = false;
            }
        }

        if (allAnomaliesCorrectlySelected)
        {
            StartCoroutine(FadeToBlackCoroutine(() =>
            {
                LevelManager.instance.PassLevel();
                selectingFinalShots = false;
                finalShots.Clear(); // Clear the list for the next level
                Debug.Log("All anomalies have been correctly selected. Proceeding to next level.");

                // Start fade-out after fade-to-black
                StartCoroutine(FadeOutCoroutine(() =>
                {
                    Debug.Log("Fade-out completed.");
                }));
            }));
        }
    }

    private void ArrangePhotosOnDesk()
    {
        if (notebookController == null || notebookController.cameraShots == null || notebookController.cameraShots.Count == 0)
        {
            Debug.LogWarning("NotebookController or photos are not properly set.");
            return;
        }

        if (escritorio == null)
        {
            Debug.LogWarning("Escritorio GameObject is not assigned.");
            return;
        }

        // Define grid parameters
        int photosPerPage = 9; // 3x3 grid
        int columns = 3; // Number of columns in the grid
        float spacing = 0.25f; // Spacing between photos
        float rowSpacing = 0.25f; // Spacing between rows
        Vector3 deskPosition = escritorio.transform.position; // Get the position of the desk
        Vector3 startPosition = deskPosition + new Vector3(-0.1f, 0.2f, -0.1f); // Offset to start the grid above the desk

        // Hide all photos first
        for (int i = 0; i < notebookController.cameraShots.Count; i++)
        {
            notebookController.cameraShots[i].SetActive(false);
            notebookController.cameraShots[i].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

        int startIdx = notebookController.currentPage * photosPerPage;
        int endIdx = Mathf.Min(startIdx + photosPerPage, notebookController.cameraShots.Count);

        for (int i = startIdx; i < endIdx; i++)
        {
            int localIndex = i - startIdx;
            int row = localIndex / columns;
            int column = localIndex % columns;

            // Calculate position relative to the desk
            Vector3 position = startPosition
                + escritorio.transform.right * (column * spacing) // Offset horizontally
                - escritorio.transform.forward * (row * rowSpacing);  // Offset vertically

            notebookController.cameraShots[i].SetActive(true);
            notebookController.cameraShots[i].transform.position = position;

            // Align photos to face upward
            notebookController.cameraShots[i].transform.rotation = Quaternion.Euler(90, 0, 90);
        }
    }


    private IEnumerator FadeToBlackCoroutine(System.Action onComplete)
    {
        float duration = 1.5f; // Duration of the fade effect
        float elapsedTime = 0f;

        Color startColor = fadeToBlackImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fully opaque black

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeToBlackImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        fadeToBlackImage.color = endColor;

        // Call the completion callback
        onComplete?.Invoke();
    }

    private IEnumerator FadeOutCoroutine(System.Action onComplete)
    {
        float duration = 1.5f; // Duration of the fade effect
        float elapsedTime = 0f;

        Color startColor = fadeToBlackImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fully transparent

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeToBlackImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        fadeToBlackImage.color = endColor;

        // Call the completion callback
        onComplete?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            selectingFinalShots = true;
        }
    }
}
