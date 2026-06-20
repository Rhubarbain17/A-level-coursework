using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameObject Menu;
    private GameObject SMenu;
    public AudioMixer audiomixer;
    public static bool issprint = true;
    public Toggle sprinton;
    private void Start()
    {
        Menu = GameObject.Find("Menu");
        SMenu = GameObject.Find("Setting");
        SMenu.SetActive(false);
    }
    //Starts the game
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
    //Opens the settings menu
    public void SetGame()
    {
        SMenu.SetActive(true);
        Menu.SetActive(false);
    }

    //Exits the game
    public void ExitGame()
    {
        Application.Quit();
    }
    //Exits the Settings Menu
    public void Back()
    {
        Menu.SetActive(true);
        SMenu.SetActive(false);
    }
    //Checks if the player wants the sprint bar enabled
    public void Sprinte()
    {
        issprint = sprinton.isOn;
    }
    //Checks if the player wants to change thier volume
    public void VolumeChange(float volume)
    {
        audiomixer.SetFloat("volume", volume);
    }
}

