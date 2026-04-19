using UnityEngine;

public class BaseEnemy : Character
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnCharacterContact(Character other)
    {
        if (isTagger && !other.isTagger)
        {
            other.isTagged = true;
        }
    }
}
