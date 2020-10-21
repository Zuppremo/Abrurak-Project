using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIReticle : MonoBehaviour
{
    [SerializeField] private float showDuration = 0.25F;
    [SerializeField] private float inScreenTime = 1.0F;
    [SerializeField] private float hideDuration = 0.25F;

    private RectTransform myTransform;
    private Image myImage;
    private Gun gun = default;

    private void Awake()
    {
        myTransform = transform as RectTransform;
        myImage = GetComponent<Image>();
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0);
        gun = FindObjectOfType<Gun>();
        gun.Fired += OnGunFired;
    }

    private void OnDestroy()
    {
        gun.Fired -= OnGunFired;
    }

    private void OnGunFired(bool hasDamaged)
    {
        myImage.transform.DOKill();
        myImage.transform.localScale = Vector3.one;
        myTransform.position = Input.mousePosition;

        myImage.DOKill();
        myImage.DOFade(1, showDuration).OnComplete(() =>
        {
            myImage.DOFade(0, hideDuration).SetDelay(inScreenTime);

            if (hasDamaged)
                myImage.transform.DOScale(1.25F, inScreenTime / 2);
        });
    }
}
