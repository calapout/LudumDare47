using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bytes;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        HideMain();
    }

    private void HideMain()
    {
        /*Start game*/
        this.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void Options()
    {
        HideMain();
    }

    public void Quit()
    {
        /*Quit game*/
        Application.Quit();
    }

    public void ReturnToMain(Data data)
    {
        this.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
    }

}
