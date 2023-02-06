using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private PlayerUIEvents _playerUIEvents;
    [SerializeField] private SwapButtonSprites _pauseButton;
    [SerializeField] private SwapButtonSprites _muteButton;
    [SerializeField] private SwapButtonSprites _muteMusicButton;
    [SerializeField] private UITweener _menuPanel;
    [SerializeField] private UITweener _pauseMarker;
    [SerializeField] private GameObject _pauseFade;
    [SerializeField] private GameObject _deadPanel;
    
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
    }

    private void OnEnable()
    {
        _playerUIEvents.OnMenuPress.AddListener(OpenMenu);
        _playerUIEvents.OnDead.AddListener(ActivateDeadPanel);
    }

    private void OnDisable()
    {
        _playerUIEvents.OnMenuPress.RemoveListener(OpenMenu);
        _playerUIEvents.OnDead.RemoveListener(ActivateDeadPanel);
    }

    private void ActivateDeadPanel()
    {
        _deadPanel.SetActive(true);
    }

    public void OpenMenu()
    {
        if (GameState.IsPaused)
        {
            _pauseButton.SwapSprites();
            GameState.IsPaused = false;
            Time.timeScale = 1;
            _menuPanel.easeType = LeanTweenType.easeOutQuad;
            _menuPanel.to = new Vector3(-150, -77.5f, 0);
            _menuPanel.Play();

            _pauseMarker.easeType = LeanTweenType.easeOutQuad;
            _pauseMarker.to = new Vector3(-80, 75, 0);
            _pauseMarker.Play();

            _pauseFade.SetActive(false);
        }
        else
        {
            _pauseButton.SwapSprites();
            GameState.IsPaused = true;
            Time.timeScale = 0;
            _menuPanel.easeType = LeanTweenType.easeInQuad;
            _menuPanel.to = new Vector3(5, -77.5f, 0);
            _menuPanel.Play();

            _pauseMarker.easeType = LeanTweenType.easeInQuad;
            _pauseMarker.to = new Vector3(-80, -80, 0);
            _pauseMarker.Play();

            _pauseFade.SetActive(true);
        }
    }
}
