using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public Animator fade;

	public void SwapLevel()
	{
		StartCoroutine(ChangeLevel());
	}

	public IEnumerator ChangeLevel()
	{
		fade.Play("FadeOut");
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(1);
	}
}
