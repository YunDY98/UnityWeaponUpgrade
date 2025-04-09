using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
    #region Inspector
    // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
    [SpineAnimation]
    public string runAnimationName;

    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string walkAnimationName;

    [SpineAnimation]
    public string atkAnimationName_1;

    [SpineAnimation]
    public string hitAnimationName;

    [SpineAnimation]
    public string deathAnimationName;

    [SpineAnimation]
    public string stunAnimationName;

    [SpineAnimation]
    public string skillAnimationName_1;
    [SpineAnimation]
    public string skillAnimationName_2;
    [SpineAnimation]
    public string skillAnimationName_3;

    #endregion
   
    
    PState pState;
    SkeletonAnimation skeletonAnimation;

    // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
       
        pState = PState.Run;
    }

    void Update()
    {
        
        
        
        switch(pState)
        {
            case PState.Idle: 
                Idle();
                break;
            case PState.Run:
                Run();
                break;

        }
      

    }

    public void SetAnim(string animName)
    {
        string current = skeletonAnimation.AnimationName;

        if(current != animName)
            spineAnimationState.SetAnimation(0, animName, true);

    }

    public void Run()
    {
        SetAnim(runAnimationName);
        if(!GameManager.Instance.IsMove)
        {
            pState = PState.Idle;
        }
    }
    public void Idle()
    {
        SetAnim(idleAnimationName);
    }

    public void Attack()
    {
        SetAnim(atkAnimationName_1);
    }





    
}


public enum PState
{
    Idle,
    Run,
    Die,
    Damaged,
    Attack,
    Buff = 20,

   
}



   

