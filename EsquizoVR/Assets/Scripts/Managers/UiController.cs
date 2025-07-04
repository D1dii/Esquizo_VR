using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private float followDistance = 1.5f;
    [SerializeField] private float typeSpeed = 0.025f;
    [SerializeField] private float popupDuration = 3f;
    [SerializeField] private SpriteRenderer fadeToBlackImage;

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

    public void ShowPopup(string key, string message, bool hasToTransition)
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
        StartCoroutine(AutoHidePopup(currentPopup, hasToTransition));
    }

    private IEnumerator TypeTextEffect(TMP_Text textMesh, string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textMesh.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private IEnumerator AutoHidePopup(GameObject popup, bool transitionAfter)
    {
        yield return new WaitForSeconds(popupDuration);
        if (popup != null && popup == currentPopup)
        {
            popup.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(popup);
                currentPopup = null;

                if (transitionAfter)
                {
                    GameManager.instance.currentLevel++;
                    FadeToBlack(() => SceneManager.LoadScene("SampleScene"));
                }
            });
        }
    }

    public void HidePopup()
    {
        if (currentPopup == null) return;

        currentPopup.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => Destroy(currentPopup));
        currentPopup = null;
    }

    public void FadeToBlack(System.Action onComplete)
    {
        StartCoroutine(FadeToBlackCoroutine(onComplete));
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

    public void FadeOut(System.Action onComplete)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
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