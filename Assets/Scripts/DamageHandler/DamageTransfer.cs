using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DamageTransfer : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject damageRoot = default;
    [SerializeField, Range(1, 10)] private int damageMultiplier = 1;
    [SerializeField] private UnityEvent OnDamaged = default;
    [SerializeField] private bool disableAfterDamage = true;

    private IDamageable rootDamageable = default;
    private Collider myCollider;

    private void Awake()
    {
        if (damageRoot == null)
            damageRoot = transform.root.gameObject;

        if (damageRoot != null)
            rootDamageable = damageRoot.GetComponent<IDamageable>();

        myCollider = GetComponent<Collider>();
    }

    public void DoDamage(int damage)
    {
        if (rootDamageable != null)
            rootDamageable.DoDamage(damage * damageMultiplier);
        else
            Debug.LogWarning($"Root damage for {name} is null!", this);

        if (disableAfterDamage)
            Disable();

        OnDamaged?.Invoke();
    }

    public void Disable()
    {
        enabled = false;
        myCollider.enabled = false;
    }
}
