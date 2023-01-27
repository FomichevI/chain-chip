using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager S;
    [SerializeField] private AudioSource _effectAs;
    [SerializeField] private AudioSource _unificationAs;
    [SerializeField] private AudioSource _musicAs;
    [SerializeField] private AudioSource _uiEffectsAs;

    [SerializeField] private AudioClip _explosiveAc;
    [SerializeField] private AudioClip _lightningHitAc;
    [SerializeField] private AudioClip _flyAc;
    [SerializeField] private AudioClip _fillingAc;
    [SerializeField] private AudioClip _cristalBreakAc;

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    public void PlayUnification()
    {
        _unificationAs.Play();
    }

    public void PlayExplosive()
    {
        _effectAs.PlayOneShot(_explosiveAc);
    }
    public void PlayLightningHit()
    {
        _effectAs.PlayOneShot(_lightningHitAc);
    }
    public void PlayFly()
    {
        _uiEffectsAs.PlayOneShot(_flyAc);
    }
    public void PlayFilling()
    {
        _uiEffectsAs.PlayOneShot(_fillingAc);
    }
    public void PlayCristalBreak()
    {
        _effectAs.PlayOneShot(_cristalBreakAc);
    }
    public void SetMusicVolume(float volume)
    {
        _musicAs.volume = volume;
    }
    public void SetSoundsVolume(float volume)
    {
        _effectAs.volume = volume;
        _unificationAs.volume = volume;
        _uiEffectsAs.volume = volume;
    }
}
