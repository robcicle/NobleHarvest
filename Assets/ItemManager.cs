using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance = null;

    // All potential items
    public ItemSO[] itemSOs;
    // Keep track of current state
    [SerializeField]

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
