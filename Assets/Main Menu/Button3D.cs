using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Button3D : MonoBehaviour
{
    [SerializeField] private Vector3 _3dSpeed;
    [SerializeField] private bool useTdirection = false;
    [SerializeField] private Transform OnScreenT;
    [SerializeField] private bool hasOffscreen = true;
    [SerializeField] private float[] moveSteps = { 0.05f, 0.1f };

    [SerializeField] private AudioClip clickAudio;
    [SerializeField] private AudioClip highlightAudio;
    [SerializeField] private bool loopAudio = false;

    // Highlight adjustment properties
    [SerializeField] private float scaleModifier = 1.25f, scRate = 1, pcRate = 1, rcRate = 1;
    [SerializeField] private Vector3 posOffset, rotOffset;

    private Vector3 originalScale, originalRotation, originalPos, targetScale, targetRot, targetPos;
    private Vector3 offScreenPos;
    private AudioSource audioSource;
    [HideInInspector] public Rigidbody rb;

    public bool shouldAnim = true;
    public bool onScreen = false;

    [Header("Events")]
    public UnityEvent onMouseEnter;
    public UnityEvent onMouseExit;
    public UnityEvent onMouseDown;
    public UnityEvent onMouseOver;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        InitializeTransforms();
        InitializeEvents();
    }

    private void InitializeTransforms()
    {
        offScreenPos = transform.position;
        originalScale = transform.localScale;
        originalRotation = OnScreenT != null ? OnScreenT.eulerAngles : transform.eulerAngles;
        originalPos = OnScreenT != null ? OnScreenT.position : transform.position;

        targetScale = originalScale * scaleModifier;
        targetRot = originalRotation + rotOffset;
        targetPos = originalPos + posOffset;

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        _3dSpeed = useTdirection ? transform.TransformDirection(_3dSpeed) : _3dSpeed;
    }

    private void InitializeEvents()
    {
        onMouseEnter ??= new UnityEvent();
        onMouseExit ??= new UnityEvent();
        onMouseDown ??= new UnityEvent();
    }

    private void FixedUpdate()
    {
        if (hasOffscreen)
        {
            OffscreenBehaviour();
        }
        rb.angularVelocity = shouldAnim ? _3dSpeed : rb.angularVelocity * 0.9f;
    }

    private void OffscreenBehaviour()
    {
        if (onScreen)
        {
            transform.position = Vector3.Lerp(transform.position, OnScreenT.position, moveSteps[0]);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, offScreenPos, moveSteps[1]);
        }
    }

    private void OnMouseEnter()
    {
        onMouseEnter.Invoke();
        PlayAudio(highlightAudio, loop: true);
        MouseOverBehaviour(true); // Activate hover effects
    }

    private void OnMouseExit()
    {
        onMouseExit.Invoke();
        if (audioSource.clip == highlightAudio)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
        ResetTransforms();
    }

    private void OnMouseDown()
    {
        onMouseDown.Invoke();
        PlayAudio(clickAudio, loop: false);
    }

    // Ensure onMouseOver triggers the desired behavior continuously while the mouse hovers
    private void OnMouseOver()
    {
        onMouseOver.Invoke(); // Invoke the onMouseOver event if needed
        MouseOverBehaviour(true); // Ensure hover effects are continuously applied
    }

    private void PlayAudio(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        if (audioSource.isPlaying)
        {
            audioSource.time = 0;
        }
        else
        {
            audioSource.Play();
        }
    }

    private void ResetTransforms()
    {
        transform.eulerAngles = originalRotation;
        MouseOverBehaviour(false); // Deactivate hover effects when the mouse exits
    }

    public void MouseOverBehaviour(bool activate)
    {
        if (activate)
        {
            // Apply hover effects, such as scaling the button up
            shouldAnim = false;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scRate);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRot, rcRate);
        }
        else
        {
            // Revert to the original state when the mouse is no longer over the button
            transform.localScale = originalScale;
            shouldAnim = true;
        }
    }


    public void SetButtonSpeed(Vector3 speed3D)
    {
        _3dSpeed = speed3D;
    }
}
