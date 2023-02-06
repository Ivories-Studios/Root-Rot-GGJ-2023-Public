using System.Collections;
using JSAM;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : Singleton<Manager>
{
    public void MuteUnMute()
    {
        PlayerOptions.IsMuted = !PlayerOptions.IsMuted;
        AudioManager.SetMasterChannelMute(PlayerOptions.IsMuted);
    }

    public void MuteUnMuteMusic()
    {
        PlayerOptions.IsMusicMuted = !PlayerOptions.IsMusicMuted;
        AudioManager.SetMusicChannelMute(PlayerOptions.IsMusicMuted);
    }

    public void LoadScene(string scene)
    {
        int r = -1;
        if (FSTManager.Instance != null)
        {
            r = Random.Range(0, FSTManager.Instance.panels.Count);
            while (r == FSTManager.Instance.currentPanelIndex)
            {
                r = Random.Range(0, FSTManager.Instance.panels.Count);
            }
        
            FSTManager.Instance.PanelAnim(r);
        }

        StartCoroutine(LoadSceneCoroutine(scene, r));
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneCoroutine(string scene, int r)
    {
        float secondsToWait = r switch
        {
            0 => 0.95f,
            1 => 1.12f,
            2 => 1.1f,
            3 => 2.2f,
            4 => 1.04f,
            5 => 1.46f,
            6 => 1.05f,
            7 => 1.16f,
            8 => 1.25f,
            9 => 1.96f,
            10 => 1.86f,
            _ => 0
        };
        yield return new WaitForSecondsRealtime(secondsToWait);
        SceneManager.LoadScene(scene);
    }
}
