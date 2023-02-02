using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _effectAs;
    [SerializeField] private AudioSource _unificationAs;
    [SerializeField] private AudioSource _musicAs;
    [SerializeField] private AudioSource _uiEffectsAs;

    [SerializeField] private AudioClip _explosiveAc;
    [SerializeField] private AudioClip _lightningHitAc;
    [SerializeField] private AudioClip _flyAc;
    [SerializeField] private AudioClip _fillingAc;
    [SerializeField] private AudioClip _cristalBreakAc;

    private void OnEnable()
    {
        EventAggregator.UnificationSound.AddListener(PlayUnification);
        EventAggregator.DestroyCristal.AddListener(PlayCristalBreak);
        EventAggregator.DestroyFire.AddListener(PlayExplosive);
        EventAggregator.UseLightning.AddListener(PlayLightningHit);
        EventAggregator.SetMusicVolume.AddListener(SetMusicVolume);
        EventAggregator.SetSoundsVolume.AddListener(SetSoundsVolume);
        EventAggregator.ThrowChip.AddListener(PlayFly);
        EventAggregator.SkillFilled.AddListener(PlayFilling);
    }
    private void OnDisable()
    {
        EventAggregator.UnificationSound.RemoveListener(PlayUnification);
        EventAggregator.DestroyCristal.RemoveListener(PlayCristalBreak);
        EventAggregator.DestroyFire.RemoveListener(PlayExplosive);
        EventAggregator.UseLightning.RemoveListener(PlayLightningHit);
        EventAggregator.SetMusicVolume.RemoveListener(SetMusicVolume);
        EventAggregator.SetSoundsVolume.RemoveListener(SetSoundsVolume);
        EventAggregator.ThrowChip.RemoveListener(PlayFly);
        EventAggregator.SkillFilled.RemoveListener(PlayFilling);
    }

    public void PlayUnification()
    {
        _unificationAs.Play();
    }

    public void PlayExplosive(Vector3 pos)
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
    public void PlayFilling(eChipColors color)
    {
        _uiEffectsAs.PlayOneShot(_fillingAc);
    }
    public void PlayCristalBreak(Vector3 pos)
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
