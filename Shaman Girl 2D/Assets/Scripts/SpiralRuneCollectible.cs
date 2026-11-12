using UnityEngine;

public class SpiralRuneCollectible : MonoBehaviour
{
    [Header("Collection Effect")]
    public GameObject collectEffectPrefab;

    [Header("Score")]
    public int points = 100;

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

            // Award points and update the highscore
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(points);
            }

            // Play the magical collection effect
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

            Debug.Log("Rune collected! Magic Leap unlocked!");

            Destroy(gameObject);
        }
    }
}
