using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator _transition;
    [SerializeField]
    private float _transitionTime = 1f;

    public void LoadNextLevel()
    {
        Debug.Log("LoadLevel");
        StartCoroutine(LoadLevel("ConnectFour"));
    }
    public void ReLoadLevel()
    {
        Debug.Log("ReLoadLevel : " + SceneManager.GetActiveScene().name);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public IEnumerator LoadLevel(string sceneName)
    {
        _transition.SetTrigger("StartTransition");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneName);
    }
 
}
