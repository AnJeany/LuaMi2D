using UnityEngine;

public static class GlobalHelper 
{
    public static string GenerateUniqueID(GameObject obj) // [05:14]
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}";
    }
}
