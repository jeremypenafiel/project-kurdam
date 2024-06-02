using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize : MonoBehaviour
{
    [SerializeField] int chance;
    // Start is called before the first frame update
    void Start()
    {
        int randomNumber= UnityEngine.Random.Range(0, 100);
        gameObject.SetActive(randomNumber >= chance);

    }


}
