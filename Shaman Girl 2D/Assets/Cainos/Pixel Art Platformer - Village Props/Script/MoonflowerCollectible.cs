using UnityEngine;

public class MoonflowerCollectible : MonoBehaviour
{
    public static bool hasMoonflower = false;

    private void Awake()
    {
        hasMoonflower = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        WitchMovement witch =
            other.GetComponentInParent<WitchMovement>();

        if (witch != null && !hasMoonflower)
        {
            hasMoonflower = true;

            // Find the flower attached to the witch
            Transform carriedFlower =
                witch.transform.Find("CarriedMoonflower");

            if (carriedFlower != null)
            {
                carriedFlower.gameObject.SetActive(true);
            }

            Debug.Log("Moonflower collected! Return to the fox.");

            // Remove the flower from the platform
            Destroy(gameObject);
        }
    }
}
