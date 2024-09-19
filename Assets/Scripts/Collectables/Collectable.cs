using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Statistics _statistics;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IUpgradable upgradable))
            return;
        DoActionOnPick(upgradable);
    }

    protected virtual void DoActionOnPick( IUpgradable upgradable)
    {
        upgradable.Upgrade(_statistics);
    }
}

public class Coin : Collectable
{

}



