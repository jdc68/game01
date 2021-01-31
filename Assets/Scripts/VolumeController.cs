using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    private Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<Bloom>(out bloom);
    }

    void FixedUpdate()
    {
        bloom.intensity.value = Mathf.PingPong(Time.time * 2, 8);
        vignette.intensity = new UnityEngine.Rendering.ClampedFloatParameter(Mathf.PingPong(Time.time * 2, 1), 0, 1, true);
    }
}
