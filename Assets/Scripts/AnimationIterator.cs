using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationIterator : MonoBehaviour
{
    [SerializeField] float BufferTime = 0.2f;

    [SerializeField]
    [Tooltip("These animations will play in order")]
    List<AnimatedObject> ObjectsToAnimate = null;
    [SerializeField] bool PlayLastEndAnimation = false;

    [Serializable]
    public class AnimatedObject
    {
        public Animator Animator;
        public string StartAnimationTrigger;
        public string EndAnimationTrigger;
        [Tooltip("Optional")] public GameObject ShowOnStartAnimationComplete;
    }

    private void Start()
    {
        StartCoroutine(WaitForInput());
    }

    // play start animaiton, waits for input, then plays exit animation 
    // UNLESS it is the last in the order and "Play Lasy End Animation" is unchecked
    IEnumerator WaitForInput()
    {
        foreach (var obj in ObjectsToAnimate)
        {
            obj.Animator.SetTrigger(obj.StartAnimationTrigger);
            yield return new WaitForEndOfFrame(); // we wait for end of frame because otherwise the current animator state info will return the previous state
            yield return new WaitForSeconds(obj.Animator.GetCurrentAnimatorStateInfo(0).length + BufferTime);

            if (obj.ShowOnStartAnimationComplete != null) obj.ShowOnStartAnimationComplete.SetActive(true);

            if (ObjectsToAnimate[ObjectsToAnimate.Count - 1] == obj && !PlayLastEndAnimation)
            {
                continue;
            }

            yield return new WaitUntil(() => Input.anyKeyDown);
            if (obj.ShowOnStartAnimationComplete != null) obj.ShowOnStartAnimationComplete.SetActive(false);

            obj.Animator.SetTrigger(obj.EndAnimationTrigger);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(obj.Animator.GetCurrentAnimatorStateInfo(0).length + BufferTime);
        }
    }
}
