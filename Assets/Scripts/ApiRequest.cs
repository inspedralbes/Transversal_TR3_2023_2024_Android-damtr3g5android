using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;

public class ApiRequest : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRenderer;
    int actualSprite = 0;
    private Sprite[] sprites;
    [SerializeField]private String skinName;
    [SerializeField]private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GetRequest", "http://localhost:3450/imagen/mainCharacter/"+skinName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //spriteRenderer.sprite = sprites[actualSprite];
        actualSprite++;
        if (actualSprite == 9) actualSprite = 0;
    }
    IEnumerator GetRequest(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Point;
            Rect[] regions = defineRegions();
            sprites = MakeMultiSprite(tex, 48, regions);
            Sprite[] walkUpSprites = new Sprite[9];
            for (int i = 0; i < 9; i++)
            {
                walkUpSprites[i] = sprites[i + (9 * 0)];
            }
            Sprite[] walkLeftSprites = new Sprite[9];
            for (int i = 0; i < 9; i++)
            {
                walkLeftSprites[i] = sprites[i + (9 * 1)];
            }
            Sprite[] walkDownSprites = new Sprite[9];
            for (int i = 0; i < 9; i++)
            {
                walkDownSprites[i] = sprites[i + (9 * 2)];
            }
            Sprite[] walkRightSprites = new Sprite[9];
            for (int i = 0; i < 9; i++)
            {
                walkRightSprites[i] = sprites[i + (9 * 3)];
            }
            

            // Create an array to hold keyframes for sprites
            createAnimationClip(walkUpSprites);

        }
    }

    private void createAnimationClip(Sprite[] walkUpSprites)
    {
        AnimationClip animationClip = new AnimationClip();
        animationClip.frameRate = 24; // Set your desired frame rate
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[walkUpSprites.Length];

        // Calculate time based on frame rate
        float frameRate = animationClip.frameRate;
        float totalTime = walkUpSprites.Length / frameRate;

        // Add keyframes for each sprite
        for (int i = 0; i < walkUpSprites.Length; i++)
        {
            float time = i / frameRate; // Adjusted time calculation

            // Create ObjectReferenceKeyframe for each sprite
            ObjectReferenceKeyframe spriteKeyframe = new ObjectReferenceKeyframe();
            spriteKeyframe.time = time;
            spriteKeyframe.value = walkUpSprites[i];

            keyframes[i] = spriteKeyframe;
        }

        // Create an animation curve using the keyframes
        AnimationUtility.SetObjectReferenceCurve(animationClip, new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        }, keyframes);

        // Save the AnimationClip
        string path = "Assets/Animations/"; // Set your desired save path
        AssetDatabase.CreateAsset(animationClip, path + "walkUpAnimation.anim");

        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, animationClip));
        }
        aoc.ApplyOverrides(anims);
        animator.runtimeAnimatorController = aoc;
    }

    private Rect[] defineRegions()
    {
        Rect[] regions = new Rect[36];
        //walk up
        for(int i = 0; i<9; i++)
        {
            regions[i] = new Rect(i * 64, 768, 64, 64);
        }
        //walk left
        for(int i = 0; i<9; i++)
        {
            regions[i+9] = new Rect(i * 64, 702, 64, 64);
        }
        //walk down
        for(int i = 0; i<9; i++)
        {
            regions[i+18] = new Rect(i * 64, 640, 64, 64);
        }
        //walk right
        for (int i = 0; i < 9; i++)
        {
            regions[i+27] = new Rect(i * 64, 576, 64, 64);
        }
        return regions;
    }

    public Sprite[] MakeMultiSprite(
                 Texture2D spritesheet,
                 float pixelsPerUnit,
                 params Rect[] regions)
    {

        var sprites = new Sprite[regions.Length];

        for (int i = 0; i < sprites.Length; i++)
            sprites[i] = Sprite.Create(
                           spritesheet,
                           regions[i],
                           new Vector2(0.5f, 0.5f),
                           pixelsPerUnit);

        return sprites;
    }

}