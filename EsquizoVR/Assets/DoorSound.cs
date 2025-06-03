using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorSound : MonoBehaviour
{
    public HingeJoint hinge;
    private AudioSource audioSource;

    private float previousAngle;
    public float movementThreshold = 0.2f;
    public float stopDelay = 0.3f;

    private float timeSinceLastMovement = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();

        previousAngle = hinge.angle;
    }

    void Update()
    {
        float currentAngle = hinge.angle;
        float delta = Mathf.Abs(currentAngle - previousAngle);

        if (delta > movementThreshold)
        {
            timeSinceLastMovement = 0f;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            timeSinceLastMovement += Time.deltaTime;

            if (audioSource.isPlaying && timeSinceLastMovement > stopDelay)
                audioSource.Stop();
        }

        previousAngle = currentAngle;
    }
}
