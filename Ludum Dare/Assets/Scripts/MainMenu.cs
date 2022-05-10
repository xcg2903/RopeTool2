using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuParent;
    [SerializeField]
    private GameObject optionsMenuParent;
    [SerializeField]
    private GameObject wControls;
    [SerializeField]
    private GameObject aControls;
    [SerializeField]
    private GameObject sControls;
    [SerializeField]
    private GameObject dControls;
    [SerializeField]
    private GameObject spaceControls;
    [SerializeField]
    private GameObject controlTutorials;
    [SerializeField]
    private Slider slider;

    public void Start()
    {
        mainMenuParent.SetActive(true);
        optionsMenuParent.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Scene");
    }
    public void OpenOptions()
    {
        mainMenuParent.SetActive(false);
        optionsMenuParent.SetActive(true);
        controlTutorials.SetActive(true);
    }
    public void CloseOptions()
    {
        mainMenuParent.SetActive(true);
        optionsMenuParent.SetActive(false);

        KeyboardPress("none");
    }
    public void KeyboardPress(string input)
    {
        wControls.SetActive(false);
        aControls.SetActive(false);
        sControls.SetActive(false);
        dControls.SetActive(false);
        spaceControls.SetActive(false);
        controlTutorials.SetActive(false);

        switch (input)
        {
            case "w":
                wControls.SetActive(true);
                break;
            case "a":
                aControls.SetActive(true);
                break;
            case "s":
                sControls.SetActive(true);
                break;
            case "d":
                dControls.SetActive(true);
                break;
            case "space":
                spaceControls.SetActive(true);
                break;
            default:
                break;
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AdjustVolume()
    {
        AudioListener.volume = slider.value;
    }
}
