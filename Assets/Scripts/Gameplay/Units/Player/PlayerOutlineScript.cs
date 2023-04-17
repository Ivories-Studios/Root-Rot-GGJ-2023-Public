using System.Collections;
using UnityEngine;

public class PlayerOutlineScript : MonoBehaviour
{
    [HideInInspector] public bool Highlight = false;

    [SerializeField] private Color highlightColor;
    [SerializeField] private Color damageColor;
    [SerializeField] private int damageTimes;
    [SerializeField] private float damageSpeed;

    private bool takingDamage = false;
    private bool damage = false;

    private Material mat;

    private Color normalCol;
    private Color normalEdgeCol;


    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        normalCol = mat.GetColor("_OutlineColor");
        normalEdgeCol = mat.GetColor("_EdgeHighlightColor");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (damage)
        {
            mat.SetColor("_OutlineColor", damageColor);
            mat.SetColor("_EdgeHighlightColor", damageColor);
        }
        else if (Highlight && !takingDamage)
        {
            mat.SetColor("_OutlineColor", highlightColor);
        }
        else
        {
            mat.SetColor("_OutlineColor", normalCol);
            mat.SetColor("_EdgeHighlightColor", normalEdgeCol);
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(DamageRoutine());
    }

    IEnumerator DamageRoutine()
    {
        takingDamage= true;
        for (int i = 0; i < damageTimes; i++)
        {
            damage = true;
            yield return new WaitForSeconds(damageSpeed);

            damage = false;
            yield return new WaitForSeconds(damageSpeed);
        }
        takingDamage= false;
    }
}
