using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtons : MonoBehaviour
{
    public GameObject player;
    public bool enabled = true;

    public List<Vector3> Locations;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (!enabled) return;

        int index = 1;

        foreach (var item in Locations)
        {
            if (GUI.Button(new Rect(600, 100 + 20 * index, 100, 20), index.ToString()))
            {
                player.GetComponent<RootMovementController>().ClearAllRoots();
                player.transform.position = Locations[index - 1];
            }
            index++;
        }
        
    }

}
