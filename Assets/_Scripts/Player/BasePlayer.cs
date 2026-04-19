using UnityEngine;

public class BasePlayer : Character
{
    [SerializeField] Renderer rend;

    protected override void Start()
    {
        base.Start();

        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
    }

    protected override void OnCharacterContact(Character other)
    {
        if (isTagger && !other.isTagger)
        {
            other.isTagged = true;
            MeshRenderer mr = other.GetComponent<MeshRenderer>();
            mr.material.color = Color.red;
        }
    }

}