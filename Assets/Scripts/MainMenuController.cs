﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bytes;

public class TryOpenCloseMenuData : Bytes.Data
{
    public TryOpenCloseMenuData(bool shopOpened) { this.ShopOpened = shopOpened; }
    public bool ShopOpened { get; private set; }
}

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] Player player;
    [SerializeField] AudioSource sounds;
    [SerializeField] AudioSource music;

    bool firstTimeOpened = false;
    bool menuIsOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        player.DisableMovement();

        EventManager.AddEventListener("tryOpenCloseMenu", (Bytes.Data d) => 
        {
            if (firstTimeOpened) { return; }

            TryOpenCloseMenuData data = (TryOpenCloseMenuData)d;
            if(!data.ShopOpened) HandleMenu();
        });

        HandleMenu();
        firstTimeOpened = true;
    }

    public void HandleMenu()
    {
        menuIsOpened = !menuIsOpened;

        if (menuIsOpened)
        {
            SetMenuOpened(true);
        }
        else
        {
            if (options.activeInHierarchy)
            {
                GoToMenu();
                menuIsOpened = true;
                return;
            }
            SetMenuOpened(false);
        }
    }

    public void SetMenuOpened(bool val)
    {
        Time.timeScale = (val) ? 0f : 1f;
        if (val) player.DisableMovement(); else player.EnableMovement();
        menu.SetActive(val);
    }

    public void PlayGame()
    {
        firstTimeOpened = false;
        HandleMenu();
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
