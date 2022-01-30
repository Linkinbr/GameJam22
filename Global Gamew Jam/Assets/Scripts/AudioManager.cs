using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioConfig;

    [Header("Array de Áudios")]

    public AudioClip[] listAudio; //Colocar todos os áudios nesta lista, estes deverão ser chamados pelo ID

    public AudioClip[] listTrilha; //Colocar todos as trilhas sonoras nesta lista, estas deverão ser chamadas pelo ID

    [Header("Setup")]

    public AudioSource Music;
    public AudioSource FX;
    public AudioMixer master;

    public void PlayMusic(int value)
    {
        Music.clip = listTrilha[value];
        Music.Play();
    }
    
    public void PlayMusicDelayed(int value, int delay)
    {
        Music.clip = listTrilha[value];
        Music.PlayDelayed(delay);
    }

    public void PauseMusic()
    {
        Music.Stop();
    }

    public void PlayFX(int value)
    {
        FX.clip = listAudio[value];
        FX.Play();
    }

    public void PlayFXOneShot(int value)
    {
        FX.clip = listAudio[value];
        FX.PlayOneShot(FX.clip,1f);
    }

    public void PauseFX()
    {
        FX.Stop();
    }

    void Start()
    {
        if (audioConfig != null)
        {
            Debug.LogError("Outro BuildManager já está ativo!");
        }
        
        audioConfig = this;
    }
    
    // CONTROLE DE VOLUME //

    public void SetMasterVolume(float volume)
    {
        HUDManager.hUDManager.masterVolume = volume;
        master.SetFloat("Master", Mathf.Log10 (volume) * 20);
    }
}
