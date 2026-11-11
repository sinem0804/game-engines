using UnityEngine;

public class SpiralRuneCollectible : MonoBehaviour
{
    [Header("Collection Effect")]
    public GameObject collectEffectPrefab;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        WitchMovement witch =
            other.GetComponentInParent<WitchMovement>();

        if (witch != null)
        {
            collected = true;

            // Unlock Magic Leap
            witch.magicLeapUnlocked = true;

            // Spawn magical particles
            if (collectEffectPrefab != null)
            {
                GameObject effect = Instantiate(
                    collectEffectPrefab,
                    transform.position,
                    Quaternion.identity
                );

                ParticleSystem particles =
                    effect.GetComponent<ParticleSystem>();

                if (particles != null)
                    particles.Play();

                Destroy(effect, 2f);
            }

            Debug.Log("Rune of Courage collected! Magic Leap unlocked!");

            // Remove the rune, but keep the particles
            Destroy(gameObject);
        }
    }
}
