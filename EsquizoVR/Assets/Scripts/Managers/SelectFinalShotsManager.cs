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
    [SerializeField] private InputActionReference triggerSelectAction;

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

    void Start()
    {
        cameraTransform = Camera.main.transform;
        selectingFinalShots = false;
    }

    void Update()
    {
        if (selectingFinalShots)
        {
            maxSelections = LevelManager.instance.CurrentAnomaliesOnLevel;
            ArrangePhotosOnDesk();
            notebookController.transform.rotation = Quaternion.identity;
        }

        if (currentSelections == maxSelections)
        {
            confirmSelectionAction.action.Enable();
            CheckPhotosSelected();
        }
        else
        {
            confirmSelectionAction.action.Disable();
        }
    }

    private void CheckPhotosSelected()
    {
        bool allAnomaliesCorrectlySelected = true;
        finalShots.Clear();

        foreach (var shot in notebookController.cameraShots)
        {
            PhotoController photoController = shot.GetComponent<PhotoController>();
            if (photoController != null && photoController.isSelected)
            {
                finalShots.Add(photoController.hasAnomaly);
            }
        }

        for (int i = 0; i < LevelManager.instance.CurrentAnomaliesOnLevel; i++)
        {
            if (!finalShots[i])
            {
                allAnomaliesCorrectlySelected = false;
            }
        }

        StartCoroutine(FadeToBlackCoroutine(() =>
        {
            if (allAnomaliesCorrectlySelected)
            {
                LevelManager.instance.PassLevel();
            }
            else
            {
                LevelManager.instance.RestartLevel(); // You must ensure this method exists
            }

            selectingFinalShots = false;
            finalShots.Clear();

            StartCoroutine(FadeOutCoroutine(() => { }));
        }));
    }

    private void ArrangePhotosOnDesk()
    {
        if (notebookController == null || notebookController.cameraShots == null || notebookController.cameraShots.Count == 0 || escritorio == null)
            return;

        int photosPerPage = 9;
        int columns = 3;
        float spacing = 0.25f;
        float rowSpacing = 0.25f;
        Vector3 deskPosition = escritorio.transform.position;
        Vector3 startPosition = deskPosition + new Vector3(-0.1f, 0.2f, -0.1f);

        for (int i = 0; i < notebookController.cameraShots.Count; i++)
        {
            GameObject photo = notebookController.cameraShots[i];
            photo.SetActive(false);
            photo.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        }

        int startIdx = notebookController.currentPage * photosPerPage;
        int endIdx = Mathf.Min(startIdx + photosPerPage, notebookController.cameraShots.Count);

        for (int i = startIdx; i < endIdx; i++)
        {
            GameObject photo = notebookController.cameraShots[i];
            int localIndex = i - startIdx;
            int row = localIndex / columns;
            int col = localIndex % columns;

            Vector3 pos = startPosition + escritorio.transform.right * (col * spacing) - escritorio.transform.forward * (row * rowSpacing);
            photo.SetActive(true);
            photo.transform.position = pos;
            photo.transform.rotation = Quaternion.Euler(90, 0, 90);

            PhotoController controller = photo.GetComponent<PhotoController>();
            if (controller != null)
            {
                controller.Initialize(triggerSelectAction);
            }
        }
    }

    private IEnumerator FadeToBlackCoroutine(System.Action onComplete)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;

        Color startColor = fadeToBlackImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeToBlackImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        fadeToBlackImage.color = endColor;
        onComplete?.Invoke();
    }

    private IEnumerator FadeOutCoroutine(System.Action onComplete)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;

        Color startColor = fadeToBlackImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeToBlackImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        fadeToBlackImage.color = endColor;
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