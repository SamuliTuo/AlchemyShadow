using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTween : MonoBehaviour
{
    public float rotationsPerMinute;
    Vector3 startScale;
    [Space(10)]
    public float amplitude = 0.2f;
    public float period = 0.2f;
    public float scaleChange = 0.1f;

    public SpriteRenderer rend;

    protected void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
        print("startscale " + startScale);
    }

    protected void Update()
    {
        transform.Rotate(0, 0, 6.0f * rotationsPerMinute * Time.deltaTime);

        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        transform.localScale = startScale + (startScale * distance * scaleChange);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeRingColor(ringColor_yellow);
            GameManager.Instance.PartyManager.PlayerIsInFlagRange();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeRingColor(ringColor_orange);
            GameManager.Instance.PartyManager.PlayerIsInFlagRange();
        }
    }

    public Color ringColor_yellow;
    public Color ringColor_orange;

    void ChangeRingColor(Color c)
    {
        rend.color = c;
    }
}