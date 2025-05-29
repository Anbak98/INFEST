using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Bgms
{    
    Main
}

public enum Sfxs
{
    Click,
    Fire,
    Hit,
    Damage,
    Siren,
    ZombieSpawn,    
    BowmeterAttack1,
    BowmeterAttack2,
    ZombieAttack,
    GritaScream,
    RageFang_Roaring,
    RageFang_Rush
}

[Serializable]
public class BgmClip
{
    public Bgms type;
    public AudioClip clip;
}

[Serializable]
public class SfxClip
{
    public Sfxs type;
    public List<AudioClip> clips;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            float master = PlayerPrefs.GetFloat("Master", 0.5f);
            float bgm = PlayerPrefs.GetFloat("Bgm", 0.5f);
            float sfx = PlayerPrefs.GetFloat("Sfx", 0.5f);
            SetMaster(master);
            SetBgm(bgm);
            SetSfx(sfx);

            PlayBgm(Bgms.Main);
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    [SerializeField] private List<BgmClip> bgmClips;
    [SerializeField] private List<SfxClip> sfxClips;

    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;

    [SerializeField] private AudioMixer audioMixer;

    public void PlayBgm(Bgms bgm)
    {
        foreach (var bgmClip in bgmClips)
        {
            if(bgmClip.type == bgm)
            {
                audioBgm.clip = bgmClip.clip;
                audioBgm.Play();
                return;
            }
        }
    }

    public void StopBgm()
    {
        audioBgm.Stop();
    }

    public void PlaySfx(Sfxs sfx)
    {
        foreach (var sfxClip in sfxClips)
        {
            if (sfxClip.type == sfx && sfxClip.clips.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, sfxClip.clips.Count);
                audioSfx.PlayOneShot(sfxClip.clips[index]);
                return;
            }
        }
    }

    public void StopSfx()
    {
        audioSfx.Stop();
    }

    public void SetMaster(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }

    public void SetBgm(float value)
    {
        audioMixer.SetFloat("Bgm", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }

    public void SetSfx(float value)
    {
        audioMixer.SetFloat("Sfx", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }
}