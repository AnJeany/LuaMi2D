using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    // Lưu đối tượng IInteractable trong phạm vi [10:25]
    private IInteractable _interactableInRange = null;
    public GameObject InteractionIcon; // Icon để hiển thị trên đầu người chơi [10:38]

    void Start()
    {
        InteractionIcon.SetActive(false); // Ẩn icon khi bắt đầu [10:44]
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision) // [11:10]
    {
        // Kiểm tra đối tượng có IInteractable và có thể tương tác không [11:16]
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            _interactableInRange = interactable; // [11:40]
            InteractionIcon.SetActive(true); // Hiển thị icon [11:47]
        }
    }

    public void OnInteract(InputAction.CallbackContext context) // [12:37]
    {
        if (context.performed) // Chỉ thực hiện khi hành động vừa được kích hoạt [12:56]
        {
            // Gọi phương thức Interact trên đối tượng trong phạm vi (nếu có)
            _interactableInRange?.Interact(); // [13:02]

            // FIX: Ẩn icon ngay sau khi tương tác nếu không thể tương tác nữa [22:56]
            if (_interactableInRange != null && !_interactableInRange.CanInteract())
            {
                InteractionIcon.SetActive(false); // [23:08]
            }   
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // [11:54]
    {
        // Kiểm tra đối tượng thoát ra có phải là đối tượng đang được chọn không [12:00]
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == _interactableInRange)
        {
            _interactableInRange = null; // [12:17] 
            InteractionIcon.SetActive(false); // Ẩn icon [12:23]
        }
    }
}



