using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private Image hP1 = default;
	[SerializeField] private Image hP2 = default;
	[SerializeField] private Image hP3 = default;
	public int startingHealth = 3;
	public int currentHealth;
	public Image damageImage = default;
	public float flashSpeed;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

	private bool isDamaged = false;

	private void Awake()
	{
		currentHealth = startingHealth;
	}

	private void Update()
    {
		if (isDamaged)
			damageImage.color = flashColour;
		else
			damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

		isDamaged = false;
    }

	public void TakeDamage(int amount)
	{
		currentHealth -= amount;
		if (currentHealth == 2)
			Destroy(hP1);
		else if (currentHealth == 1)
			Destroy(hP2);
		else if (currentHealth == 0)
			Destroy(hP3);
		isDamaged = true;
		//currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);
	}
}
