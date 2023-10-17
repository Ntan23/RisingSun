using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance {get; private set;}
    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [SerializeField] private Sound[] sounds;
    private GameManager gm;

    private void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null) return;

        s.source.Play();
    }

    private void Stop(string name)
    { 
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null) return;

        s.source.Stop();
    }

    void Start() 
    {
        gm = GameManager.instance;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixer;
        }

        Play("BGM1");
    } 
    
    // public void SetSFXVolume(float volume)
    // {
    //     foreach(Sound s in sounds) 
    //     {
    //         if(s.name == "BGM") continue;
    //         if(s.name != "BGM") s.source.volume = volume;
    //     }
    // }

    // public void SetBGMVolume(float volume)
    // {
    //     foreach(Sound s in sounds) 
    //     {
    //         if(s.name == "BGM")
    //         {
    //             s.source.volume = volume;
    //             break;
    //         }         
    //     }
    // }

    // public void StopAllSFX()
    // {
    //     foreach(Sound s in sounds) 
    //     {
    //         if(s.name != "BGM") s.source.Stop();
    //     }
    // }
    public void StopBGM1() => Stop("BGM1");
    public void PlayBGM2() => Play("BGM2");

    public void PlayClickSFX()
    {
        if(gm.GetDifficultyIndex() == 1) Play("Click1");
        if(gm.GetDifficultyIndex() == 2) Play("Click2");
    }

    public void PlayPopUpSFX() => Play("PopUp");
    public void PlayHitSFX() => Play("EnergyHit");
    public void PlayWarningSFX() => Play("Warning");
    public void StopWarningSFX() => Stop("Warning");
    public void PlayGlitchCutsceneAudio() => Play("GlitchCutscene");
    public void StopGlitchCutsceneAudio() => Stop("GlitchCutscene");
} 

