using UnityEngine;

[CreateAssetMenu(fileName = "SpriteAnimation", menuName = "Custom/Sprite Animation")]
public class SOSpriteAnimation : ScriptableObject
{
    public SpriteFrame[] frames;
    public bool flippedX = false;
    public bool flippedY = false;
    public bool looping = false;
    public bool keepDirection = false;
}

[System.Serializable]
public class SpriteFrame
{
    public Sprite sprite;
    public float frametime = 0.1f;
}