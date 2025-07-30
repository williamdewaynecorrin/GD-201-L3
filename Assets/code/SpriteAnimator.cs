using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public string defaultAnim = "default";
    public SOSpriteAnimation[] animations;

    private Dictionary<string, SOSpriteAnimation> animationDict;
    private SOSpriteAnimation currentAnimation = null;
    private SpriteRenderer spriteRenderer;
    private Vector3 standardScale;

    private float animationTimer;
    private int frameIndex;
    private bool finished;

    void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animationDict = new Dictionary<string, SOSpriteAnimation>();
        foreach(SOSpriteAnimation anim in animations)
        {
            if(animationDict.ContainsKey(anim.name))
            {
                Debug.LogErrorFormat("Failed to add animation {0} to animationDict: an animation with that name already exists", anim.name);
                continue;
            }

            animationDict.Add(anim.name, anim);
        }

        standardScale = transform.localScale;

        PlayAnimation(defaultAnim, true);   
    }

    void Update()
    {
        // -- don't continue if we have no valid current animation
        if (currentAnimation == null)
            return;
        
        // -- don't continue if our current animation isn't a looping one and we have already played it
        if (!currentAnimation.looping && finished)
            return;

        // -- count timer down and set frames when ready
        animationTimer -= Time.deltaTime;
        if(animationTimer <= 0.0f)
        {
            ++frameIndex;
            if(frameIndex >= currentAnimation.frames.Length)
            {
                if (currentAnimation.looping)
                    frameIndex = 0;
                else
                {
                    finished = true;
                    return;
                }
            }

            animationTimer = currentAnimation.frames[frameIndex].frametime;
            spriteRenderer.sprite = currentAnimation.frames[frameIndex].sprite;
        }
    }

    public void PlayAnimation(string animName, bool resetIfCurrent)
    {
        if(!animationDict.ContainsKey(animName))
        {
            Debug.LogErrorFormat("Failed to play animation {0}: that animation does not exist in the animationDict", animName);
            return;
        }

        if (currentAnimation != null && currentAnimation.name == animName && !resetIfCurrent)
            return;

        PlayAnimationInternal(animationDict[animName]);
    }

    private void PlayAnimationInternal(SOSpriteAnimation animation)
    {
        currentAnimation = animation;
        frameIndex = 0;
        finished = false;

        animationTimer = currentAnimation.frames[frameIndex].frametime;
        spriteRenderer.sprite = currentAnimation.frames[frameIndex].sprite;

        if (!currentAnimation.keepDirection)
        {
            Vector3 newScale = standardScale;
            if (currentAnimation.flippedX)
                newScale.x *= -1.0f;
            if (currentAnimation.flippedY)
                newScale.y *= -1.0f;

            transform.localScale = newScale;
        }
    }
}
