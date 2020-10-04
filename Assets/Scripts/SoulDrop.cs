using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bytes;

public class SoulDrop : MonoBehaviour
{
    public string tagToFollow = "Player";
    public string eventNameToDispatch = "playerGainsSoul";

    public Rigidbody2D rg;
    private Vector3 initialScale;
    float timeOffset;
    Transform target;
    public Vector2 speedBetween = new Vector2(5, 9);
    float randomSpeed = 2;
    private void Start()
    {
        randomSpeed = Random.Range(speedBetween.x, speedBetween.y);
        timeOffset = Random.Range(1f, 10f);
        rg.AddForce(new Vector2(Utils.PositiveOrNegative(Random.Range(1, 5)), Utils.PositiveOrNegative(Random.Range(1, 5))), ForceMode2D.Impulse);
        initialScale = transform.localScale;

        Animate.Delay(Random.Range(2f, 5f), GoToPlayer);
    }
    private void Update()
    {
        transform.localScale = initialScale + (initialScale * (Mathf.Sin(timeOffset + Time.time * 2f))) / 5f;

        if (target != null)
        {
            Vector2 dir = target.position - this.transform.position;
            dir.Normalize();
            float dis = Vector2.Distance(target.position, this.transform.position);
            rg.velocity = (dir * randomSpeed) / Mathf.Clamp(dis, 0.1f, 2.5f);
        }
    }

    public void GoToPlayer()
    {
        target = GameObject.FindGameObjectWithTag(tagToFollow)?.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision?.tag == tagToFollow)
        {
            //Bytes.EventManager.Dispatch(eventNameToDispatch, null);
            Destroy(this.gameObject);
        }
    }
}
