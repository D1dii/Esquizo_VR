using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIPopupManager : MonoBehaviour
{
    public static UIPopupManager Instance;

    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private float followDistance = 1.5f;
    [SerializeField] private float typeSpeed = 0.025f;
    [SerializeField] private float popupDuration = 3f;

    private Transform cameraTransform;
    private GameObject currentPopup;
    private Coroutine typeCoroutine;

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
    }

    public void ShowPopup(string key, string message)
    {
        if (popupPrefab == null || cameraTransform == null) return;

        if (currentPopup != null)
        {
            currentPopup.transform.DOKill();
            Destroy(currentPopup);
            currentPopup = null;
        }

        currentPopup = Instantiate(popupPrefab);
        currentPopup.transform.localScale = Vector3.zero;
        currentPopup.transform.position = cameraTransform.position + cameraTransform.forward * followDistance;
        currentPopup.transform.LookAt(cameraTransform);
        currentPopup.transform.Rotate(0, 180f, 0);
        currentPopup.transform.DOScale(0.002f, 0.4f).SetEase(Ease.OutBack);

        TMP_Text text = currentPopup.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = "";
            if (typeCoroutine != null) StopCoroutine(typeCoroutine);
            typeCoroutine = StartCoroutine(TypeTextEffect(text, message));
        }

        currentPopup.AddComponent<VRPopupFollower>().Init(cameraTransform, followDistance);
        StartCoroutine(AutoHidePopup(currentPopup));
    }

    private IEnumerator TypeTextEffect(TMP_Text textMesh, string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textMesh.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private IEnumerator AutoHidePopup(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);
        if (popup != null && popup == currentPopup)
        {
            popup.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => Destroy(popup));
            currentPopup = null;
        }
    }

    public void HidePopup()
    {
        if (currentPopup == null) return;

        currentPopup.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => Destroy(currentPopup));
        currentPopup = null;
    }
}

public class VRPopupFollower : MonoBehaviour
{
    private Transform targetCamera;
    private float distance;

    public void Init(Transform cameraTransform, float followDistance)
    {
        targetCamera = cameraTransform;
        distance = followDistance;
    }

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        transform.position = targetCamera.position + targetCamera.forward * distance;
        transform.LookAt(targetCamera);
        transform.Rotate(0, 180f, 0);
    }
}
