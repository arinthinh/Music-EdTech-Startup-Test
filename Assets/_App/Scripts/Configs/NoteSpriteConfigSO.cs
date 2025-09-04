
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteSpriteConfigSO", menuName = "SO/NoteSpriteConfigSO", order = 0)]
public class NoteSpriteConfigSO : ScriptableObject
{
    public NoteSpriteConfig[] NoteSpriteConfigs;
    
    public Sprite GetSprite(NoteType noteType)
    {
        foreach (var config in NoteSpriteConfigs)
        {
            if (config.NoteType == noteType)
            {
                return config.Sprite;
            }
        }
        Debug.LogWarning($"Sprite for NoteType {noteType} not found!");
        return null;
    }
}

[Serializable]
public class NoteSpriteConfig
{
    public NoteType NoteType;
    public Sprite Sprite;
}
