using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager : PersistentSingleton<SoundManager>
{
    [SerializeField] private AudioSource _musicSource, _effectsSource, _2DeffectsSource;
    [SerializeField] private AudioMixer _audioMixer;

    //provisional, esto seran los sliders en el menu de opciones
    [SerializeField, Range(0.0001f, 1)] private float _masterVolume;
    [SerializeField, Range(0.0001f, 1)] private float _soundFXVolume;
    [SerializeField, Range(0.0001f, 1)] private float _musicVolume;
    [SerializeField, Range(0.0001f, 1)] private float _ambienceVolume;


    public void InitializeVolume(GameObject optionsMenu)
    {


        SetMasterVolume(_masterVolume);
        SetSoundFXVolume(_soundFXVolume);
        SetMusicVolume(_musicVolume);
        SetAmbienceVolume(_ambienceVolume);

        Slider[] sliders = optionsMenu.GetComponentsInChildren<Slider>();

        foreach (Slider sl in sliders)
        {
            if (sl.name == "MasterSlider")
            {
                sl.value = _masterVolume;
            }
            else if (sl.name == "MusicSlider")
            {
                sl.value = _musicVolume;
            }
            else if (sl.name == "SFXSlider")
            {
                sl.value = _soundFXVolume;
            }
            else if (sl.name == "AmbienceSlider")
            {
                sl.value = _ambienceVolume;
            }
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in audiosource
        GameObject g = ObjectPoolingManager.Instance.SpawnObject(_effectsSource.gameObject, spawnTransform.position, spawnTransform.rotation, PoolType.AudioSources);
        AudioSource audioSource = g.GetComponent<AudioSource>();

        //asign audio clip
        audioSource.clip = audioClip;
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;
        StartCoroutine(ReturnObjectToPool(clipLength, g));
    }

    public void Play2DRandomSoundFXClip(AudioClip[] clips, float volume)
    {
        int rand = Random.Range(0, clips.Length);

        //spawn in audiosource
        GameObject g = ObjectPoolingManager.Instance.SpawnObject(_2DeffectsSource.gameObject, Vector3.zero, Quaternion.identity, PoolType.AudioSources);
        AudioSource audioSource = g.GetComponent<AudioSource>();

        //asign audio clip
        audioSource.clip = clips[rand];
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();
        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        //ObjectPoolingManager.Instance.ReturnObjectToPool(g); 
        //Destroy(audioSource.gameObject, clipLength); //CANVIAR
        StartCoroutine(ReturnObjectToPool(clipLength, g));
    }

    public void Play2DSoundFXClip(AudioClip audioClip, float volume)
    {
        //spawn in audiosource
        GameObject g = ObjectPoolingManager.Instance.SpawnObject(_2DeffectsSource.gameObject, Vector3.zero, Quaternion.identity, PoolType.AudioSources); ;
        AudioSource audioSource = g.GetComponent<AudioSource>();

        //asign audio clip
        audioSource.clip = audioClip;
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;
        // Destroy(audioSource.gameObject, clipLength);


        StartCoroutine(ReturnObjectToPool(clipLength, g));
    }


    IEnumerator ReturnObjectToPool(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);

        ObjectPoolingManager.Instance.ReturnObjectToPool(obj);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        //spawn in audiosource
        GameObject g = ObjectPoolingManager.Instance.SpawnObject(_effectsSource.gameObject, spawnTransform.position, spawnTransform.rotation, PoolType.AudioSources);
        AudioSource audioSource = g.GetComponent<AudioSource>();

        //asign audio clip
        audioSource.clip = audioClip[rand];
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();
        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        //ObjectPoolingManager.Instance.ReturnObjectToPool(g); 
        //Destroy(audioSource.gameObject, clipLength); //CANVIAR
        StartCoroutine(ReturnObjectToPool(clipLength, g));
    }

    private void Update()
    {
        /*SetMasterVolume(_masterVolume);
        SetSoundFXVolume(_soundFXVolume);
        SetMusicVolume(_musicVolume);
        SetAmbienceVolume(_ambienceVolume);*/
    }

    public void SetMasterVolume(float level)
    {
        _masterVolume = level;
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20);
        //magia matematica para hacer que sea lineal y no logaritmico
    }

    public void SetSoundFXVolume(float level)
    {
        _soundFXVolume = level;
        _audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {

        _musicVolume = level;
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    }

    public void SetAmbienceVolume(float level)
    {
        _ambienceVolume = level;
        _audioMixer.SetFloat("AmbienceVolume", Mathf.Log10(level) * 20);
    }

}
