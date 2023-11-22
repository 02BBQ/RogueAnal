using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text money;

    private bool loading = false;
    public Transgender transgender;

    private void Start()
    {
        if (money) Init();
    }
    public void Init()
    {
        money.text = GameManager.Instance.Money.ToString();
    }
    public void ChangeScene(string nameOfScene)
    {
        if (loading) return;
        StartCoroutine(LoadLevel(nameOfScene));
    }
    private IEnumerator LoadLevel(string sceneName)
    {
        loading = true;
        while (transgender.maskAmount < 1f)
        {
            yield return null;
        }
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Loading the Scene");
            yield return null;
        }
        loading = false;
        SceneManager.LoadScene(sceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
