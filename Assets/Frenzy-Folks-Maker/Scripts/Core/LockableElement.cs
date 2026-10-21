using ITISKIRUHERE;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(HoverableElement))]
[RequireComponent(typeof(XRGrabInteractable))]
public class LockableElement : MonoBehaviour
{
    private static AudioClip _generatedLockClip;
    private static AudioClip _generatedUnlockClip;
    private float _lockSoundVolume = 1f;
    private float _unlockSoundVolume = 1f;
    
    //TODO: Make it in the menu
    private InputAction _rightTriggerAction = CreateDefaultAction();
    
    private bool _isLocked;
    private XRGrabInteractable _grabInteractable;
    private HoverableElement _hoverableElement;
    private AdvancedOutline _currentOutline;
    private AudioSource _audioSource;
    
    public bool IsLocked { get => _isLocked; }

    private static InputAction CreateDefaultAction()
    {
        var action = new InputAction("Lock", InputActionType.Button);
        action.AddBinding("<XRController>{RightHand}/triggerPressed");
        action.AddBinding("<XRController>{RightHand}/{TriggerButton}");
        return action;
    }

    private static AudioClip GenerateLockClip()
    {
        return GenerateToneClip("GeneratedLockSound", 900f, 300f);
    }

    private static AudioClip GenerateUnlockClip()
    {
        return GenerateToneClip("GeneratedUnlockSound", 300f, 900f);
    }

    private static AudioClip GenerateToneClip(string clipName, float startFrequency, float endFrequency)
    {
        const int sampleRate = 44100;
        const float duration = 0.2f;

        int sampleCount = (int)(sampleRate * duration);
        float[] data = new float[sampleCount];
        float phase = 0f;

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float progress = (float)i / sampleCount;

            float frequency = Mathf.Lerp(startFrequency, endFrequency, progress);
            phase += 2f * Mathf.PI * frequency / sampleRate;

            float envelope = Mathf.Exp(-t * 25f);
            data[i] = Mathf.Sin(phase) * envelope * 0.6f;
        }

        AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
    
    public void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _hoverableElement = GetComponent<HoverableElement>();

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f;
        
        _grabInteractable.selectEntered.AddListener(OnSelectEntered);
        
        _rightTriggerAction.Enable();
        _rightTriggerAction.performed += OnTriggerPressed;
    }

    public void OnDestroy()
    {
        _rightTriggerAction.performed -= OnTriggerPressed;
        _rightTriggerAction.Disable();

        if (_grabInteractable != null)
            _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }
    
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        if (!_grabInteractable.isHovered) return;

        SetLocked(!IsLocked);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!_isLocked) return;

        _grabInteractable.interactionManager?.CancelInteractableSelection(
            (IXRSelectInteractable)_grabInteractable);
    }

    public void SetLocked(bool isLocked)
    {
        _isLocked = isLocked;
        if (IsLocked)
        {
            PlayLockSound();

            AdvancedOutline existingOutline = GetComponent<AdvancedOutline>();
            if (existingOutline != null)
            {
                DestroyImmediate(existingOutline);
            }

            if (_grabInteractable.isSelected)
            {
                _grabInteractable.interactionManager?.CancelInteractableSelection(
                    (IXRSelectInteractable)_grabInteractable);
            }

            _currentOutline = gameObject.AddComponent<AdvancedOutline>();
            if (_currentOutline == null)
            {
                Debug.LogError($"{name} : AddComponent<AdvancedOutline> a renvoyé null", this);
                return;
            }
            _currentOutline.OutlineMode = AdvancedOutline.Mode.OutlineAll;
            _currentOutline.OutlineColor = Color.red;
        }
        else
        {
            PlayUnlockSound();

            if (_currentOutline != null)
            {
                DestroyImmediate(_currentOutline);
                _currentOutline = null;
            }

            if (_grabInteractable.isHovered)
            {
                _hoverableElement.ToggleOutline(true);
            }
        }
    }

    private void PlayLockSound()
    {
        if (_audioSource == null) return;
        
        if (_generatedLockClip == null)
            _generatedLockClip = GenerateLockClip();

        _audioSource.PlayOneShot(_generatedLockClip, _lockSoundVolume);
    }

    private void PlayUnlockSound()
    {
        if (_audioSource == null) return;
        
        if (_generatedUnlockClip == null)
            _generatedUnlockClip = GenerateUnlockClip();

        _audioSource.PlayOneShot(_generatedUnlockClip, _unlockSoundVolume);
    }
}