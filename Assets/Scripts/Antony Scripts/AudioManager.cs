using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource[] music;

    [SerializeField] private AudioSource[] sfx;

    [SerializeField] private int levelMusicToPlay;

    private int currentTrack;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(levelMusicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    currentTrack++;
        //    PlayMusic(currentTrack);
        //    PlaySFX(0);
        //}
    }

    public void PlayMusic(int musicToPlay)
    {
        foreach (AudioSource musicTracks in music)
        {
            musicTracks.Stop();
        }

        music[musicToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }
}
