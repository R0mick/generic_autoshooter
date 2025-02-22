using _Scripts.Guns;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    
    [Header("Music Clips")]
    [SerializeField] AudioClip firstLevelMusicClip;
    [SerializeField] AudioClip menuMusicClip;
    [SerializeField] AudioClip bossMusicClip;
    [SerializeField] AudioClip gameOverLoseMusicClip;
    [SerializeField] AudioClip gameOverWinMusicClip;
    
    [Header("Audio Clips")]
    [SerializeField] AudioClip revolverShootClip;
    [SerializeField] AudioClip revolverReloadClip;
    [SerializeField] AudioClip pistolShootClip;
    [SerializeField] AudioClip pistolReloadClip;
    [SerializeField] AudioClip uziShootClip;
    [SerializeField] AudioClip uziReloadClip;
    [SerializeField] AudioClip shotgunShootClip;
    [SerializeField] AudioClip shotgunReloadClip;
    [SerializeField] AudioClip machinegunShootClip;
    [SerializeField] AudioClip machinegunReloadClip;
    [SerializeField] AudioClip shotgunSemiAutoShootClip;
    [SerializeField] AudioClip shotgunSemiAutoReloadClip;
    
    
    [SerializeField] AudioClip drawWeaponClip;
    
    [SerializeField] AudioClip slimeDeath;
    [SerializeField] AudioClip slimeTakesDamage;
    
    [SerializeField] AudioClip heroDamaged;
    
    public enum MusicTracks
    {
        MenuMusic,
        FirstLevelMusic,
        GameOverLoseMusic,
        GameOverWinMusic,
        BossMusic,
    }
    
    public float SoundVolume
    {
        get => sfxSource.volume;
        set => sfxSource.volume = value;
    }
    
    public float MusicVolume
    {
        get => musicSource.volume;
        set => musicSource.volume = value;
    }
    
    
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SoundVolume = 0.4f;
        MusicVolume = 0.3f;
        
    }
    
    public void PlayMusic(MusicTracks track){

        switch (track)
        {
            case (MusicTracks.MenuMusic):
            {
                musicSource.clip = menuMusicClip;
                musicSource.Play();
                break;
            }
            case (MusicTracks.FirstLevelMusic):
            {
                musicSource.clip = firstLevelMusicClip;
                musicSource.Play();
                break;
            }
            case (MusicTracks.GameOverLoseMusic):
            {
                musicSource.clip = gameOverLoseMusicClip;
                musicSource.Play();
                break;
            }
            case (MusicTracks.GameOverWinMusic):
            {
                musicSource.clip = gameOverWinMusicClip;
                musicSource.Play();
                break;
            }
            case (MusicTracks.BossMusic):
            {
                musicSource.clip = bossMusicClip;
                musicSource.Play();
                break;
            }
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PlayGunShootClip(string gunName)
    {
        switch (gunName)
        {
            case "Revolver":
            {
                sfxSource.PlayOneShot(revolverShootClip);
                break;
            }
            case "Pistol":
            {
                sfxSource.PlayOneShot(pistolShootClip);
                break;
            }
            case "Uzi":
            {
                sfxSource.PlayOneShot(uziShootClip);
                break;
            }
            case "Shotgun":
            {
                sfxSource.PlayOneShot(shotgunShootClip);
                break;
            }
            case "Machinegun":
            {
                sfxSource.PlayOneShot(machinegunShootClip);
                break;
            }
            case "ShotgunSemiAuto":
            {
                sfxSource.PlayOneShot(shotgunSemiAutoShootClip);
                break;
            }
        }
    }
    
    public void PlayGunReloadClip(string gunName)
    {
        switch (gunName)
        {
            case "Revolver":
            {
                sfxSource.PlayOneShot(revolverReloadClip);
                break;
            }
            case "Pistol":
            {
                sfxSource.PlayOneShot(pistolReloadClip);
                break;
            }
            case "Uzi":
            {
                sfxSource.PlayOneShot(uziReloadClip);
                break;
            }
            case "Shotgun":
            {
                sfxSource.PlayOneShot(shotgunReloadClip);
                break;
            }
            case "Machinegun":
            {
                sfxSource.PlayOneShot(machinegunReloadClip);
                break;
            }
            case "ShotgunSemiAuto":
            {
                sfxSource.PlayOneShot(shotgunSemiAutoReloadClip);
                break;
            }
        }
    }
    
    public void PlayEnemyDeathClip(string enemyName) //todo new sounds ?
    {
        switch (enemyName)
        {
            case "SlimeS":
            {
                sfxSource.PlayOneShot(slimeDeath);
                break;
            }
            case "SlimeM":
            {
                sfxSource.PlayOneShot(slimeDeath);
                break;
            }
            case "SlimeL":
            {
                sfxSource.PlayOneShot(slimeDeath);
                break;
            }
            case "SlimeXL":
            {
                sfxSource.PlayOneShot(slimeDeath);
                break;
            }
        }
    }
    
    public void PlayEnemyTakesDamageClip(string enemyName)//todo implement
    {
        switch (enemyName)
        {
            case "SlimeS":
            {
                sfxSource.PlayOneShot(slimeTakesDamage);
                break;
            }
            case "SlimeM":
            {
                sfxSource.PlayOneShot(slimeTakesDamage);
                break;
            }
            case "SlimeL":
            {
                sfxSource.PlayOneShot(slimeTakesDamage);
                break;
            }
        }
    }

    public void PlayHeroDamagedClip()
    {
        sfxSource.PlayOneShot(heroDamaged);
    }
    
    public void DrawWeaponClip()
    {
        sfxSource.PlayOneShot(drawWeaponClip);
    }

    public void PauseSounds()
    {
        sfxSource.Pause();
    }
    public void ResumeSounds()
    {
        sfxSource.UnPause();
    }
}
