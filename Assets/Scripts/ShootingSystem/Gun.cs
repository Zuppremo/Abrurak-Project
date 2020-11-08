using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Action<bool> Fired;
    public Action ReloadRequired;
    public Action<float> ReloadStarted;

    [SerializeField] private int maxBullets = 5;
	[SerializeField] private int damagePerBullet = 1;
    [SerializeField] private float shootRange = 100.0f;
    [SerializeField] private float reloadDuration = 0.5F;
    [SerializeField] private GameObject impactParticles = default;
	[SerializeField] private float fireRate = 1.5f;
	[SerializeField] private float nextFire = 0;
	
    private bool isReloading = false;

    public int CurrentBullets { get; private set; }
	public float ReloadDuration => reloadDuration;

    private void Awake()
    {
        Reload();
    }

    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        ProccessAndroidInput();
#else
        ProccessEditorOrWindowsInput();
#endif
    }

    private void ProccessAndroidInput()
    {
        if (Input.touchCount > 0 && Time.time > nextFire)
        {
			nextFire = Time.time + fireRate;
            if (Input.touchCount == 1)
                TryShoot();
            else if (Input.touchCount > 1)
                TryReload();
        }
    }

    private void ProccessEditorOrWindowsInput()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
            TryShoot();
		}

        if (Input.GetButtonDown("Fire2"))
            TryReload();
    }

    private void TryShoot()
    {
        if (isReloading)
            return;

        if (CurrentBullets > 0)
            Shoot();
        else
            ReloadRequired?.Invoke();
    }

    private void TryReload()
    {
        if (CurrentBullets >= maxBullets || isReloading)
            return;

        isReloading = true;
        Invoke(nameof(Reload), reloadDuration);
        ReloadStarted?.Invoke(reloadDuration);
    }

    private void Reload()
    {
        CurrentBullets = maxBullets;
        isReloading = false;
    }

	private void Shoot()
    {
        CurrentBullets--;
        bool hasDamaged = TryMakeDamage();
        Fired?.Invoke(hasDamaged);
    }

    private bool TryMakeDamage()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, shootRange))
        {
            IDamageable damageable = null;


			if (hit.collider.TryGetComponent(out damageable))
            {
                damageable.DoDamage(damagePerBullet);
                return true;
            }

            GameObject particlesGo = Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(particlesGo, 2f);
        }

        return false;
    }
}
