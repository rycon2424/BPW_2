using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public float minRange;
    public float maxRange;
    [Space]
    public float currentRange;
    public Light l;

    int goHigher = 0;

    void Start()
    {
        l.range = Random.Range(minRange, maxRange);
        goHigher = Random.Range(0, 2);
        StartCoroutine(Flickering());
    }
    
    IEnumerator Flickering()
    {
        while (true)
        {
            if (goHigher == 0 && currentRange > minRange)
            {
                currentRange -= 0.25f;
            }
            else if (goHigher == 0)
            {
                goHigher = 1;
            }
            if (goHigher == 1 && currentRange < maxRange)
            {
                currentRange += 0.25f;
            }
            else if (goHigher == 1)
            {
                goHigher = 0;
            }
            l.range = currentRange;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
