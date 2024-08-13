using System.Collections;
using UnityEngine;

public class RandomAnimationTrigger : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public float minWaitTime = 1f; // Minimum wait time
    public float maxWaitTime = 5f; // Maximum wait time

    private void Start()
    {
        StartCoroutine(AnimationSequence());
    }

    private IEnumerator AnimationSequence()
    {
        while (true)
        {
            float randomTime = Random.Range(minWaitTime, maxWaitTime);
            print("picked " + randomTime);
            // Wait for a random time between minWaitTime and maxWaitTime
            yield return new WaitForSeconds(randomTime);

            // Choose animation state 1 or 2 randomly
            int animState = Random.Range(1, 3);
            animator.SetInteger("AnimState", animState);
            print("picked " + animState);

            // Wait for the animation to finish playing
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Reset animation state to 0
            animator.SetInteger("AnimState", 0);

            // Wait again for the animation to fully finish if it has exit time
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
