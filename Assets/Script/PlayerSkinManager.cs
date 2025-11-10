using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkinManager : MonoBehaviour
{

    public enum PlayerSkinType
    {
        Default,
        SkinA,
        SkinB,
        SkinC
    }

    [System.Serializable]
    public class SkinData
    {
        public PlayerSkinType skinType;
        public Sprite sprite;
    }

    [Header("Danh sách skin")]
    public List<SkinData> skins = new List<SkinData>();

    private SpriteRenderer spriteRenderer;
    private Dictionary<PlayerSkinType, Sprite> skinMap = new Dictionary<PlayerSkinType, Sprite>();
    private PlayerSkinType currentSkin = PlayerSkinType.Default;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        

        // Map các skin để tra nhanh
        foreach (var skin in skins)
        {
            if (!skinMap.ContainsKey(skin.skinType))
                skinMap.Add(skin.skinType, skin.sprite);
        }

        //ChangeSkin(PlayerSkinType.Default);
    }

    private void Start()
    {
       
    }

    public void ChangeSkin(PlayerSkinType newSkin)
    {
        if (!skinMap.ContainsKey(newSkin))
        {
            Debug.LogWarning($"Không tìm thấy skin: {newSkin}");
            return;
        }

        currentSkin = newSkin;
        spriteRenderer.sprite = skinMap[newSkin];
    }

    public PlayerSkinType GetCurrentSkin()
    {
        return currentSkin;
    }

   
}
