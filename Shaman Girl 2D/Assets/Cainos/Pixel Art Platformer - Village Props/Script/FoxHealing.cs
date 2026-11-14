using UnityEngine;

public class FoxHealing : MonoBehaviour
{
    public GameObject flowerAtFox;
    public SpriteRenderer foxSprite;
    public Sprite awakeFoxSprite;

    public float interactionDistance = 1.5f;

    private Transform player;
    private bool isHealed = false;

    private void Start()
    {
        GameObject witch = GameObject.Find("Player_Witch");

        if (witch != null)
            player = witch.transform;

        if (flowerAtFox != null)
            flowerAtFox.SetActive(false);
    }

    private void Update()
    {
        if (player == null || isHealed)
            return;

        float distance = Vector2.Distance(
            player.position,
            transform.position
        );

        if (distance <= interactionDistance &&
            MoonflowerCollectible.hasMoonflower &&
            Input.GetKeyDown(KeyCode.E))
        {
            HealFox();
        }
    }

    private void HealFox()
    {
        isHealed = true;
        MoonflowerCollectible.hasMoonflower = false;

        // Remove flower from witch's hands
        Transform carriedFlower =
            player.Find("CarriedMoonflower");

        if (carriedFlower != null)
            carriedFlower.gameObject.SetActive(false);

        // Give flower to fox
        if (flowerAtFox != null)
            flowerAtFox.SetActive(true);

        // Wake up the fox!
        if (foxSprite != null && awakeFoxSprite != null)
        {
            foxSprite.sprite = awakeFoxSprite;
            foxSprite.color = Color.white;
        }
       WitchLightSpell lightSpell = player.GetComponent<WitchLightSpell>();

       if (lightSpell != null)
       {
        lightSpell.UnlockLightSpell();
    }
        Debug.Log("The fox has awakened! Light power unlocked!");
    }
}
