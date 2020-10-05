using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public float speed = 0.02f;
    public Text txt;
    public void SetDmg(int val)
    {
        txt.text = val.ToString();
    }
    private void Update()
    {
        transform.position += new Vector3(0, speed, 0);
    }
}
