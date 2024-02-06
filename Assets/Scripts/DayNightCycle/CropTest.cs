using UnityEngine;

public class CropTest : MonoBehaviour
{
    [System.Serializable]
    public struct CropStruct
    {
        public int cropGrowthLevel;
        public bool harvestable;
    }



    [SerializeField] private CropStruct _crops;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
