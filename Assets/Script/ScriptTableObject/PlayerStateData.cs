using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "PlayerStateData", menuName = "Player/Player State Data")]
public class PlayerStateData : ScriptableObject
{
    [Header("State Info")]
    public PlayerState state;         // Enum để dễ filter nếu cần (optional, dùng cho debug)
    public string stateName = "Normal"; // Để debug/UI: "Normal Dough", "Ball", "Thin", "Super Thin"

    [Header("Visual")]
    public Sprite sprite;             // Sprite cho state này
    public string animationTrigger;   // Trigger name trong Animator (rỗng nếu không dùng)

    [Header("Movement")]
    public float moveSpeed = 5f;      // Tốc độ di chuyển cơ bản
    public float dashCooldown = 1f;   // Cooldown dash cho state này

    [Header("Physics")]
    public Vector2 hitboxSize = Vector2.one;  // Kích thước BoxCollider2D (nhỏ cho SuperThin)
    public float colliderOffsetY = 0f;

   
}

public enum PlayerState
{
    Normal = 0,
    Ball = 1,
    Thin = 2,
    SuperThin = 3
}

