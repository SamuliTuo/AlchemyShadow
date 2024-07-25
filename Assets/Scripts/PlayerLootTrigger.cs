using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Loot"))
        {
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
            if (thing == null)
            {
                yield break;
            }
            perc = t * t;
            thing.transform.position = Vector3.Lerp(startPos, transform.position, perc);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
