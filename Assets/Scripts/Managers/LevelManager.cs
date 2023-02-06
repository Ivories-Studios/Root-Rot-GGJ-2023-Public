
using JSAM;
using UnityEngine;

public class LevelManager : Manager
{
    public new static LevelManager Instance { get; private set; }
    private Music _lastTrack;
    private bool _waitOneFrame = true;

    protected override void Awake()
    {
        GameState.IsPaused = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (AudioManager.IsMusicPlaying(_lastTrack) || GameState.IsPaused)
        {
            _waitOneFrame = true;
            return;
        }
        if (_waitOneFrame)
        {
            _waitOneFrame = false;
            return;
        }
        Music track = (Music)Random.Range(1, 9);
        while (track == _lastTrack)
        {
            track = (Music)Random.Range(1, 9);
        }
        _lastTrack = track;
        AudioManager.PlayMusic(track);
    }

    private void OnDestroy()
    {
        AudioManager.StopMusic();
        AudioManager.StopAllSounds();
    }
}
