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

        if (witch != null && GameManager.Instance != null)
        {
            collected = true;

            // Add one rune to the collection
            GameManager.Instance.CollectRune(witch);

            // Award points
            GameManager.Instance.AddScore(points);

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

            Destroy(gameObject);
        }
    }
}
