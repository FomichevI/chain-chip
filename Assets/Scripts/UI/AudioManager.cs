using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager S;
    [SerializeField] private AudioSource effectAs;
    [SerializeField] private AudioSource unificationAs;
    [SerializeField] private AudioSource musicAs;
    [SerializeField] private AudioSource UiEffectsAs;

    [SerializeField] private AudioClip explosiveAc;
    [SerializeField] private AudioClip lightningHitAc;
    [SerializeField] private AudioClip flyAc;
    [SerializeField] private AudioClip fillingAc;
    [SerializeField] private AudioClip cristalBreakAc;

    private void Awake()
    {
        if (S == null)
            S = this;
    }

    public void PlayUnification()
    {
        unificationAs.Play();
    }

    public void PlayExplosive()
    {
        effectAs.PlayOneShot(explosiveAc);
    }
    public void PlayLightningHit()
    {
        effectAs.PlayOneShot(lightningHitAc);
    }
    public void PlayFly()
    {
        UiEffectsAs.PlayOneShot(flyAc);
    }
    public void PlayFilling()
    {
        UiEffectsAs.PlayOneShot(fillingAc);
    }
    public void PlayCristalBreak()
    {
        effectAs.PlayOneShot(cristalBreakAc);
    }
    public void SetMusicVolume(float volume)
    {
        musicAs.volume = volume;
    }
    public void SetSoundsVolume(float volume)
    {
        effectAs.volume = volume;
        unificationAs.volume = volume;
        UiEffectsAs.volume = volume;
    }
}
