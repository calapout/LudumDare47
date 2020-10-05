using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] Player player;
    [SerializeField] AudioSource sounds;
    [SerializeField] AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        player.DisableMovement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        menu.SetActive(false);
        player.EnableMovement();
    }

    public void GoToOptions()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }

    public void GoToMenu()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void SoundChanged(Slider slider)
    {
        if (slider.tag == "sound")
        {
            sounds.volume = slider.value;
            return;
        }

        music.volume = slider.value;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
