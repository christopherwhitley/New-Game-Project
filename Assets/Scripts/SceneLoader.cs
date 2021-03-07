using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneLoader : MonoBehaviour
{

    private void Awake()
    {

        int sceneLoaderCount = FindObjectsOfType<SceneLoader>().Length;
        if (sceneLoaderCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

        }
    }
    Player player;

    private void Start() //called third
    {
        player = FindObjectOfType<Player>();
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        
    }
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnEnable() //called first
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //called second
    {
        player = FindObjectOfType<Player>();
        player.SetStartPoint();
        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        vcam.LookAt = player.gameObject.transform;
        vcam.Follow = player.gameObject.transform;

    }
}
