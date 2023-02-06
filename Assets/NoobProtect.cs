using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobProtect : MonoBehaviour
{
    public GameObject textForNoobs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
            timer += Time.deltaTime;

        if (timer > 25f)
        {
            textForNoobs.SetActive(true);
        }
    }

    public float timer = 0;
    bool counting;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        counting = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        counting = false;
    }
}
