using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenGrowth = 15.0f;
    [SerializeField]
    private float maxStages = 4;

    private float growthTimer = 0.0f;
    bool isGrowing = false;
    int growthState = 1;

    // Start is called before the first frame update
    void Start()
    {
        isGrowing = true;
        GetComponent<SpriteRenderer>().sprite = CropManager.instance.cropSprites[growthState-1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrowing)
            return;

        growthTimer += Time.deltaTime;

        if (growthTimer >= timeBetweenGrowth)
        {
            growthTimer = 0.0f;
            Grow();
        }
    }

    void Grow()
    {
        if (growthState + 1 > maxStages)
        {
            isGrowing = false;
            return;
        }

        growthState++;
        GetComponent<SpriteRenderer>().sprite = CropManager.instance.cropSprites[growthState-1];
    }
}
