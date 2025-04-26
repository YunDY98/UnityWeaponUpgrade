using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;
    

    void Awake()
    {
        Instance = this;
        Init();
        
    }
   
    void Update()
    {
       
       
    }
    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i=0; i < sfxPlayers.Length; ++i) 
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }


    }
    public void PlayBgm(bool isPlay)
    {
        if(isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public AudioSource PlaySfx(Sfx sfx)
    {
        for(int i =0; i<sfxPlayers.Length;++i)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;
            if(sfxPlayers[loopIndex].isPlaying)
                continue;
            channelIndex = loopIndex;
            if(Sfx.LongClick == sfx)
                sfxPlayers[loopIndex].loop = true;
            else
                sfxPlayers[loopIndex].loop = false;

            
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            return sfxPlayers[loopIndex];
        }
            
        return null;
    }





       
}
public enum Sfx
{
    Attack,
    OneClick,
    LongClick,

   
}
