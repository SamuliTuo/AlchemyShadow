using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootTrigger : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("loot trigger hit something");
        if (collision.CompareTag("Loot"))
        {
            //print("loot trigger hit loot");
            var script = collision.GetComponent<ExperienceInstance>();
            if (script.looted == false)
            {
                script.looted = true;
                StartCoroutine(LootAThing(collision.gameObject));
            }
        }
    }

    IEnumerator LootAThing(GameObject thing)
    {
        float t = 0;
        float perc = 0;
        Vector3 startPos = thing.transform.position;
        while (t < 1)
        {
            if (player == null)
            {
                yield break;
            }
            if (thing == null)
            {
                yield break;
            }
            perc = t * t;
            thing.transform.position = Vector3.Lerp(startPos, player.transform.position, perc);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
