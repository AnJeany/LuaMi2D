using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    // Thuộc tính getter/private setter [03:42]
    public bool IsOpened { get; private set; }
    // ID duy nhất dùng để lưu trạng thái [04:02]
    public string ChestID { get; private set; }

    public GameObject ItemPrefab; // Mẫu vật phẩm để thả [04:13]
    public Sprite openSprite; // Sprite của rương khi đã mở [04:19]

    void Start()
    {
        // Sử dụng toán tử null-coalescing assignment để tạo ID nếu chưa có [06:07]
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }
    public void Interact() // [06:31]
    {
        if (!CanInteract()) // [06:33]
        {
            return;
        }
        OpenChest(); // [06:45]
    }

    private void OpenChest() // [06:45]
    {
        SetOpened(true); // [07:44]

        if (ItemPrefab != null) // [07:49]
        {
            // Tạo vật phẩm và đặt vị trí bên dưới rương
            GameObject droppedItem = Instantiate(ItemPrefab,
                                               transform.position + Vector3.down, // [08:03]
                                               Quaternion.identity);
            
        }
    }

    // Dùng để thay đổi Sprite và cập nhật trạng thái IsOpened
    public void SetOpened(bool opened) // [06:54]
    {
        IsOpened = opened;
        // Cách viết tối ưu (thực hiện gán rồi kiểm tra) [07:26]
        if (IsOpened =  opened)
        {
            GetComponent<SpriteRenderer>().sprite = openSprite; // [07:18]
        }
    }
}

