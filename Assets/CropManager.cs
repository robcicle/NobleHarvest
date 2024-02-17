using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager instance = null;

    public Sprite[] cropSprites;

    private void Awake()
    {
        // Assert if there is already a controller.
        Debug.Assert(instance == null,
            "Multiple instances of singleton has already been created",
            this.gameObject
            );

        // Handle of the first controller created.
        instance = this;
    }
}
