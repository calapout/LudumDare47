using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Bytes;

public class GoToBossEndCinematic : MonoBehaviour
{
    public Animator endAnim;

    bool cameraShouldFollowBoss = false;
    public Transform boss;
    void Start()
    {
        EventManager.AddEventListener("bossDefeated", (Bytes.Data d) => {
            cameraShouldFollowBoss = true;
            boss = GameObject.FindObjectOfType<BossAI>().transform;

            Animate.Delay(4f, () => {
                PlayEndAnim();
            });

        });
    }
    void Update()
    {
        if (boss != null && cameraShouldFollowBoss)
        {
            Vector3 newvec = Vector3.Lerp(this.transform.position, boss.transform.position, Time.deltaTime * 0.5f);
            newvec.z = -10f;
            this.transform.position = newvec;
        }   
    }
    private void PlayEndAnim()
    {
        Utils.PlayAnimatorClip(endAnim, "endAnim", ()=> {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
