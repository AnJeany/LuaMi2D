using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    //public static SceneController Instance;

    //public void NextLevel()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void LoadScene(string sceneName)
    //{
    //    SceneManager.LoadSceneAsync(sceneName);
    //}

    public static string targetSpawn;

    void Start()
    {
        if (string.IsNullOrEmpty(targetSpawn)) return;

        GameObject spawn = GameObject.Find(targetSpawn);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (spawn && player)
            player.transform.position = spawn.transform.position;
    }
}
