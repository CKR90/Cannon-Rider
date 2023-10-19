using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    [SerializeField] private List<AudioClip> SoundData;
    [SerializeField] private List<AudioClip> MusicData;
    [SerializeField] private List<AudioClip> TalkData;

    private List<AudioSource> SFXSource = new List<AudioSource>();
    private AudioSource musicSource;
    private AudioSource EndGameSource;
    private AudioSource MenuSource;
    private List<AudioSource> talkSource = new List<AudioSource>();

    private int SFXSourceCount = 10;
    private int SFXSequencer = 0;

    private int TalkSourceCount = 2;
    private int TalkSequencer = 0;

    private float SFXVolume = 1f;
    private float musicVolume = .5f;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSFX();
        InitializeMusic();
        InitializeTalk();
    }
    private void Start()
    {
        PlayMenuMusic();
    }
    private void InitializeSFX()
    {
        for (int i = 0; i < SFXSourceCount; i++)
        {
            AudioSource a = gameObject.AddComponent<AudioSource>();
            a.playOnAwake = false;
            SFXSource.Add(a);
        }
    }
    private void InitializeMusic()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        EndGameSource = gameObject.AddComponent<AudioSource>();
        EndGameSource.playOnAwake = false;
        EndGameSource.loop = true;

        MenuSource = gameObject.AddComponent<AudioSource>();
        MenuSource.playOnAwake = false;
        MenuSource.loop = true;
    }
    private void InitializeTalk()
    {
        for (int i = 0; i < SFXSourceCount; i++)
        {
            AudioSource a = gameObject.AddComponent<AudioSource>();
            a.playOnAwake = false;
            talkSource.Add(a);
        }
    }

    public void PlayRandomMusic()
    {
        PlayMusic(MusicList.Random);
    }
    public void Play(SFXList sound, float volume = 1f)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        SFXSource[SFXSequencer].clip = SoundData[(int)sound];
        SFXSource[SFXSequencer].volume = volume * SFXVolume;
        SFXSource[SFXSequencer].Play();
        SFXSequencer = (SFXSequencer + 1) % SFXSourceCount;
    }
    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp(volume, 0f, 1f);
    }
    public float GetSFXVolume()
    {
        return SFXVolume;
    }
    public void StopSFX()
    {
        foreach (var s in SFXSource)
        {
            s.Stop();
        }
    }
    public void PlayEndGameMusic()
    {
        EndGameSource.volume = 0.5f;
        EndGameSource.clip = MusicData[(int)MusicList.RockSport];
        EndGameSource.Play();
    }
    public void StopEndGameMusic()
    {
        EndGameSource.DOFade(0f, 5f).OnComplete(() => EndGameSource.Stop());
    }
    public void PlayMenuMusic()
    {
        if (MenuSource.isPlaying) return;

        MenuSource.volume = 0.5f;
        MenuSource.clip = MusicData[(int)MusicList.RockWestern];
        MenuSource.Play();
    }
    public void StopMenuMusic()
    {
        MenuSource.DOFade(0f, 3f).OnComplete(() => MenuSource.Stop());
    }
    public void PlayMusic(MusicList music)
    {
        if (musicSource.isPlaying) return;

        if (music == MusicList.Random)
        {
            music = (MusicList)UnityEngine.Random.Range(0, Enum.GetNames(typeof(MusicList)).Length - 1);
        }

        if (musicSource.isPlaying && musicSource.clip == MusicData[(int)music]) return;

        musicSource.DOFade(0f, 1f).OnComplete(() =>
        {

            musicSource.clip = MusicData[(int)MusicList.WesternSwing];
            musicSource.Play();
            musicSource.DOFade(musicVolume, 2f);

        });
    }
    public void StopMusic(bool fastStop = false)
    {
        if (fastStop)
        {
            musicSource.volume = 0f;
            musicSource.Stop();
        }
        else
        {
            musicSource.DOFade(0f, 1f).OnComplete(() => musicSource.Stop());
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, 0f, 1f);
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void PlayTalk(TalkList talk, float volume = 1f)
    {
        if (talk == TalkList.Random)
        {
            talk = (TalkList)UnityEngine.Random.Range(0, Enum.GetNames(typeof(TalkList)).Length - 1);
        }

        talkSource[TalkSequencer].clip = TalkData[(int)talk];
        talkSource[TalkSequencer].volume = volume * SFXVolume;
        talkSource[TalkSequencer].Play();
        TalkSequencer = (TalkSequencer + 1) % TalkSourceCount;
    }
}


public enum SFXList
{
    Comic_Head_Impact,
    Comic_Squeak,
    Crash_Debris_1,
    Evil_Laugh_2,
    Flute_Glock_Harp,
    Ghost_Hover_1,
    Mouthpiece_Fart_2,
    Music_Orchestral_2,
    Music_Vibraphone_Chord_3,
    Rope_Twirl_Fast,
    Rubber_Band_Stretch_3,
    Strings_Ascend_3,
    Suction_Plop_2,
    Suction_Plop_7,
    Tire_Hit,
    Trombone_Tired,
    Trumped_Muted,
    Trumpet_Muted_Charge,
    Violin_Scrape_15,
    Violin_Stab_Fast,
    Window_Hit_2,
    Wood_Block_Clip,
    etfx_explosion_nuke,
    Head_Explode,
    Explosion_Body_Grenade_2,
    Crash_BoxEffect1,
    Crash_BoxEffect2,
    Crash_BallRoll,
    Crash_Spring1,
    Crash_Bubble1,
    Crash_Bubble2,
    Crash_Bubble3,
    Crash_HammerHit,
    Box_Shuffle,
    CoinSound,
    Beep,
    electronic_02,
    Flame_Burst,
    WinSound1,
    WinSound2,
    VictoryAward,
    CartoonSlap,
    ButtonClick,
    CoinFalling,
    Denied
}
public enum MusicList
{
    WesternSwing,
    RockSport,
    RockWestern,
    Random
}
public enum TalkList
{
    Voice_Clip_Male_10,
    Voice_Clip_Male_28,
    Voice_Clip_Male_60,
    Voice_Clip_Male_119,
    Voice_Clip_Male_120,
    Voice_Clip_Male_394,
    Male_Hurt_01,
    Male_Hurt_02,
    Male_Hurt_03,
    Male_Hurt_04,
    Best_Time_Female,
    Random
}

