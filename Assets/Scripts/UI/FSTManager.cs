using System.Collections.Generic;
using UnityEngine;

public class FSTManager : PersistentSingleton<FSTManager>
{
    [Header("STYLE PARENTS")]
    public List<GameObject> panels = new List<GameObject>();

    [Header("SETTINGS")]
    public int currentPanelIndex = 0;

    // [Header("PANEL ANIMS")]
    private string panelFadeIn = "Panel Open";
    private string panelFadeOut = "Panel Close";

    private GameObject currentPanel;
    private GameObject nextPanel;

    private Animator currentPanelAnimator;
    public Animator nextPanelAnimator;

    private void Start ()
    {
        currentPanel = panels[currentPanelIndex];
        currentPanelAnimator = currentPanel.GetComponent<Animator>();

        nextPanel = panels[currentPanelIndex];
        nextPanelAnimator = nextPanel.GetComponent<Animator>();
    }

    public void PanelAnim(int newPanel)
    {
        currentPanel = panels[currentPanelIndex];

        currentPanelIndex = newPanel;
        nextPanel = panels[currentPanelIndex];

        currentPanelAnimator = currentPanel.GetComponent<Animator>();
        nextPanelAnimator = nextPanel.GetComponent<Animator>();

        currentPanelAnimator.Play(panelFadeOut);
        nextPanelAnimator.Play(panelFadeIn);
    }
}