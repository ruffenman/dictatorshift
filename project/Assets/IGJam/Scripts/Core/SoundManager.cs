using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
    public const string MUSIC_DINNER_PARTY = "DinnerParty";
    public const string MUSIC_PAUSE = "Pause";
    public const string MUSIC_TITLE_MENU = "TitleMenu";
    public const string MUSIC_STAGE01 = "Stage01";

    public const string SFX_BREAK = "Break";
    public const string SFX_BREAK_BIG = "BreakBig";
    public const string SFX_COLLECT_ITEM = "CollectItem";
    public const string SFX_EXPLOSION = "Explosion";
    public const string SFX_GET_HIT = "GetHit";
    public const string SFX_JUMP = "Jump";
    public const string SFX_LAND = "Land";
    public const string SFX_LASER_BEAM = "LaserBeam";
    public const string SFX_MENU_CURSOR = "MenuCursor";
    public const string SFX_SHOOT = "Shoot";
    public const string SFX_TELEPORT = "Teleport";

    public void PlayMusic(string name)
    {
        PlayMusic(name, false);
    }

    public void PlayMusic(string name, bool forceRestart)
    {
        bool shouldStartNew = false;
        // If there is current music and either forcing restart or it is not the same as requested
        if (null != mCurrentMusic && (forceRestart || mCurrentMusic.name != name))
        {
            mCurrentMusic.Stop();
            Destroy(mCurrentMusic.gameObject);
            shouldStartNew = true;
        }
        // If there is no current music
        else if (null == mCurrentMusic)
        {
            shouldStartNew = true;
        }

        AudioSource musicSource = mMusicByName[name];
        // if requested music found and should start new music
        if (null != musicSource && shouldStartNew)
        {
            mCurrentMusic = Instantiate(musicSource) as AudioSource;
            DontDestroyOnLoad(mCurrentMusic);
            mCurrentMusic.name = musicSource.name;
            if (JamGame.instance.isMusicEnabled)
            {
                mCurrentMusic.Play();
            }
        }
        // If should start new but source was null
        else if (shouldStartNew)
        {
            Debug.LogError(new System.Text.StringBuilder("Could not find music source with name: ").Append(name).ToString());
        }
    }

    public AudioSource PlaySfx(string name)
    {
        return PlaySfx(name, false);
    }

    public AudioSource PlaySfx(string name, bool loop)
    {
        AudioSource currentSfx = null;
        if (JamGame.instance.isSFXEnabled)
        {
            // if no current source for that sfx exists
            if (!mCurrentSfx.ContainsKey(name))
            {
                AudioSource sfxSource = mSfxByName[name];
                if (null != sfxSource)
                {
                    currentSfx = Instantiate(sfxSource) as AudioSource;
                    DontDestroyOnLoad(currentSfx);
                    currentSfx.name = sfxSource.name;

                    mCurrentSfx[name] = currentSfx;
                }
                // source was null
                else
                {
                    Debug.LogError(new System.Text.StringBuilder()
                                     .Append("Could not find sfx source with name: ").Append(name).ToString());
                }
            }
            else
            {
                currentSfx = mCurrentSfx[name];
            }

            if (null != currentSfx)
            {
                currentSfx.Play();
                currentSfx.loop = loop;
            }
            else
            {
                Debug.LogError(new System.Text.StringBuilder()
                                 .Append("Could not find valid current sfx with name: ").Append(name).ToString());
            }
        }
        return currentSfx;
    }

    private void ClearSfx()
    {
        foreach (AudioSource currentSfx in mCurrentSfx.Values)
        {
            currentSfx.Stop();
            Destroy(currentSfx.gameObject);
        }
        mCurrentSfx.Clear();
    }

    private void Awake()
    {
        mMusicByName = new Dictionary<string, AudioSource>();
        foreach (AudioSource musicSource in mMusicSources)
        {
            mMusicByName[musicSource.name] = musicSource;
        }

        mSfxByName = new Dictionary<string, AudioSource>();
        foreach (AudioSource sfxSource in mSfxSources)
        {
            mSfxByName[sfxSource.name] = sfxSource;
        }
        mCurrentSfx = new Dictionary<string, AudioSource>();
    }

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void Destroy()
    {

    }

    [SerializeField]
    private List<AudioSource> mMusicSources;
    [SerializeField]
    private List<AudioSource> mSfxSources;

    private Dictionary<string, AudioSource> mMusicByName;
    private AudioSource mCurrentMusic;

    private Dictionary<string, AudioSource> mSfxByName;
    private Dictionary<string, AudioSource> mCurrentSfx;
}
