using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void CH_Death()
    {
        FindObjectOfType<PlayerController>().Ch_Count();
        Destroy(gameObject);
    }

    public void Gem_Death()
    {
        FindObjectOfType<PlayerController>().Gem_Count();
        Destroy(gameObject);
    }
}
