using UnityEngine;

public class FinishPoint : MonoBehaviour
{    
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gọi hàm hoàn thành cấp độ từ GameManager
          // SceneController.Instance.NextLevel();
        }
    }
}
