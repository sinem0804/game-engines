using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject howToPlayPanel;

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1_ForgottenVillage_New");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit button clicked!");
    }
}
