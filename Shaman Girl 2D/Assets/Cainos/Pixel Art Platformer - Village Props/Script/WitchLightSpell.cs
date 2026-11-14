using UnityEngine;

public class WitchLightSpell : MonoBehaviour
{
    public Animator witchAnimator;

    private bool lightUnlocked = false;

    private void Update()
    {
        if (lightUnlocked && Input.GetKeyDown(KeyCode.F))
        {
            witchAnimator.SetTrigger("CastLight");
            Debug.Log("Light Spell cast!");
        }
    }

    public void UnlockLightSpell()
    {
        lightUnlocked = true;
        Debug.Log("LIGHT SPELL UNLOCKED! Press F to cast.");
    }
}
