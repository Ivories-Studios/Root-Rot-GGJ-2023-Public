using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootBar : MonoBehaviour
{
    private Slider slider;
    private RootMovementController rootController;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        rootController = GameObject.FindObjectOfType<RootMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = rootController.maxRootLength;
        slider.value = rootController.maxRootLength - rootController.rootLengthLeft;
    }
}
