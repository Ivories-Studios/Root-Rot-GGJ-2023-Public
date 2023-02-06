using System.Collections;
using JSAM;
using UnityEngine;

public class MainMenuManager : Manager
{
    [SerializeField] private UITweener _controlsPanel;
    [SerializeField] private SwapButtonSprites _muteButton;
    [SerializeField] private SwapButtonSprites _muteMusicButton;
    private bool _isControlsPanelOpen;

    protected override void Awake()
    {
        base.Awake();
        GameState.Load();
        GameState.IsPaused = false;
        Time.timeScale = 1;
    }

    private void Start()
    {
        if (PlayerOptions.IsMuted)
        {
            _muteButton.SwapSprites();
        }
        if (PlayerOptions.IsMusicMuted)
        {
            _muteMusicButton.SwapSprites();
        }
        OpenControlsPanel();
        StartCoroutine(LateStart());
    }

    public void OpenControlsPanel()
    {
        if (_isControlsPanelOpen)
        {
            _isControlsPanelOpen = false;
            _controlsPanel.to = new Vector3(-400, 0, 0);
            _controlsPanel.easeType = LeanTweenType.easeInBack;
            _controlsPanel.Play();
        }
        else
        {
            _isControlsPanelOpen = true;
            _controlsPanel.to = new Vector3(550, 0, 0);
            _controlsPanel.easeType = LeanTweenType.easeOutBack;
            _controlsPanel.Play();
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<AudioPlayerMusic>().Play();
    }
}
