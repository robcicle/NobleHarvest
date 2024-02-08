using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Singleton instance
    public static ItemManager instance = null;

    // All potential items
    public ItemSO[] itemSOs;  // Array containing all potential items

    private void Awake()
    {
        // Assert if there is already a controller.
        Debug.Assert(instance == null,
            "Multiple instances of singleton has already been created",  // Assertion message for multiple instances
            this.gameObject  // Object associated with the assertion
            );

        // Handle of the first manager created.
        instance = this;  // Assign this instance as the singleton instance
    }
}