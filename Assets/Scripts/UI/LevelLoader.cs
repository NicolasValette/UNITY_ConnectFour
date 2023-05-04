using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator _transition;
    [SerializeField]
    private float _transitionTime = 1f;
    [SerializeField]
    private string _mainSceneName = "ConnectFour";

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(_mainSceneName));
    }
    public void ReLoadLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public IEnumerator LoadLevel(string sceneName)
    {
        _transition.SetTrigger("StartTransition");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneName);
    }
 
}
