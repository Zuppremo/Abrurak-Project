using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
	[SerializeField] private Image progressBar;

	void Start()
    {
		StartCoroutine(LoadAsyncOperation());
    }
	
	public IEnumerator LoadAsyncOperation()
	{
		AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);

		while(gameLevel.progress < 1)
		{
			progressBar.fillAmount = gameLevel.progress;
			yield return new WaitForEndOfFrame();
		}

	}
}
