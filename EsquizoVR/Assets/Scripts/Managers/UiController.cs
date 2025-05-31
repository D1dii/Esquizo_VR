using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UIPopupManager : MonoBehaviour
{
    public static UIPopupManager Instance;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private float popupDuration = 5f;
    [SerializeField] private float followDistance = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        ShowPopup("Welcome to the VR Experience!");
    }

    public void ShowPopup(string message)
    {
        if (popupPrefab == null || cameraTransform == null)
        {
            Debug.LogWarning("PopupPrefab or CameraTransform not assigned");
            return;
        }

        GameObject popup = Instantiate(popupPrefab);
        popup.transform.localScale = Vector3.zero;

        Vector3 targetPos = cameraTransform.position + cameraTransform.forward * followDistance;
        popup.transform.position = targetPos;
        popup.transform.LookAt(cameraTransform);
        popup.transform.Rotate(0, 180f, 0); // Flip to face user correctly

        popup.transform.DOScale(0.002f, 0.4f).SetEase(Ease.OutBack);

        var text = popup.GetComponentInChildren<TMPro.TMP_Text>();
        if (text != null)
        {
            text.text = message;
        }

        popup.AddComponent<VRPopupFollower>().Init(popupDuration, followDistance);
    }
}

public class VRPopupFollower : MonoBehaviour
{
    private Transform targetCamera;
    private float duration;
    private float distance;
    private float timer = 0f;

    public void Init(float popupDuration, float followDistance)
    {
        duration = popupDuration;
        distance = followDistance;
    }

    private void Start()
    {
        Camera cam = Camera.main;
        if (cam != null)
            targetCamera = cam.transform;
    }

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        timer += Time.deltaTime;
        transform.position = targetCamera.position + targetCamera.forward * distance;
        transform.LookAt(targetCamera);
        transform.Rotate(0, 180f, 0);

        if (timer >= duration)
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));
            enabled = false;
        }
    }
}