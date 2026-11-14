using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Attach once to the gameplay Main Camera, not to the disappearing collectibles.
[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public class RuneCollectionPostProcess : MonoBehaviour
{
    [SerializeField] private Color magicalTint = new Color(0.78f, 0.72f, 1f, 1f);
    [SerializeField, Range(0f, 1f)] private float strength = 0.45f;

    private const float Attack = 0.08f;
    private const float Duration = 0.5f;
    private Volume pulseVolume;
    private VolumeProfile runtimeProfile;
    private ColorAdjustments colorAdjustments;
    private float elapsed = Duration;
    private float startingWeight;

    private void Awake()
    {
        // A separate runtime Volume preserves all existing profile assets and overrides.
        var volumeObject = new GameObject("Rune Collection Volume (Runtime)");
        volumeObject.transform.SetParent(transform, false);
        volumeObject.layer = gameObject.layer;
        pulseVolume = volumeObject.AddComponent<Volume>();
        pulseVolume.isGlobal = true;
        pulseVolume.priority = 100f;
        pulseVolume.weight = 0f;

        runtimeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        runtimeProfile.name = "Rune Collection Profile (Runtime)";
        colorAdjustments = runtimeProfile.Add<ColorAdjustments>(false);
        colorAdjustments.colorFilter.Override(magicalTint);
        pulseVolume.profile = runtimeProfile;
    }

    private void OnEnable()
    {
        if (pulseVolume != null)
            pulseVolume.enabled = true;
    }

    [ContextMenu("Test Rune Pulse (Play Mode)")]
    public void Play()
    {
        if (!Application.isPlaying || !isActiveAndEnabled || pulseVolume == null)
            return;

        colorAdjustments.colorFilter.value = magicalTint;
        // Restart from the current strength: close pickups neither jump nor stack.
        startingWeight = pulseVolume.weight;
        elapsed = 0f;
    }

    private void Update()
    {
        if (elapsed >= Duration)
            return;

        // Finish fading even if gameplay is paused during the pulse.
        elapsed += Time.unscaledDeltaTime;
        pulseVolume.weight = elapsed < Attack
            ? Mathf.Lerp(startingWeight, strength, Mathf.SmoothStep(0f, 1f, elapsed / Attack))
            : Mathf.Lerp(strength, 0f, Mathf.SmoothStep(0f, 1f, (elapsed - Attack) / (Duration - Attack)));
    }

    private void OnDisable()
    {
        elapsed = Duration;
        if (pulseVolume != null)
        {
            pulseVolume.weight = 0f;
            pulseVolume.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (pulseVolume != null)
            Destroy(pulseVolume.gameObject);
        if (colorAdjustments != null)
            Destroy(colorAdjustments);
        if (runtimeProfile != null)
            Destroy(runtimeProfile);
    }
}
