using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetScene;
    public string targetSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SceneController.targetSpawn = targetSpawnPoint;
        SceneManager.LoadScene(targetScene);
    }
}
