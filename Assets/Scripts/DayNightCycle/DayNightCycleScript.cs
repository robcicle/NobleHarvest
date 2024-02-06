using UnityEngine;
using static CropTest;
//using UnityEngine.U2D; //TO BE REMOVED WHEN LIGHTING IS DECIDED HERE FOR REFERENCE

public class DayNightCycleScript : MonoBehaviour
{
    [System.Serializable]
    public struct DayAndNightMark
    {
        public float timeRatio; //Stores time of the mark. meaning when the mark is supposed to be triggered.

        //Could be used to set the values of a light to a specific colour/intensity depending on the the mark. Eg Low intensity during night hours high during day
        public Color color;
        public float intensity;
        //Integer for the level of growth in a crop. This should probably be a variable in whatever object contains all the crop stuff but this is just for testing


    }

    [SerializeField] private DayAndNightMark[] _marks; //Array for the daynightmarks

    [SerializeField] private float _cycleLength = 24; // Length of cycle in seconds


    //TO BE REMOVED WHEN LIGHTING IS DECIDED HERE FOR REFERENCE
    //[SerializeField] private Light2D _light; //To be used when lighting is added into scene, Dont know what kind of lighting we are using.
    [SerializeField] private SpriteRenderer _square;
    [SerializeField] public CropStruct _crop;

    [SerializeField] private int _maxCropGrowth = 3;



    //Variables for calculating mark time.
    private float _currentCycleTime;
    private int _currentMarkIndex, _nextMarkIndex;
    private float _currentMarkTime, _nextMarkTime;

    private const float _TIME_CHECK_EPSILON = 0.1f; //Used to check if the elapsed time is close to a mark due to floating points not being able to be compared like integers. May need to be changed if marks are closer together than 0.1 seconds

    // Start is called before the first frame update
    void Start()
    {
        _currentMarkIndex = -1;
        _CycleMarks();
    }

    // Update is called once per frame
    void Update()
    {
        _currentCycleTime = (_currentCycleTime + Time.deltaTime) % _marks.Length; //Tracks the amount of time passed since the last cycle, Wraps back to 0 once all marks complete

        if (Mathf.Abs(_currentCycleTime - _nextMarkTime) < _TIME_CHECK_EPSILON) //Checks to see if the current cycle has reached the next mark by finding the difference then checking if its small than the _TIME_CHECK_EPSILON const 
        {
            DayAndNightMark next = _marks[_nextMarkIndex];

            if (_crop.harvestable == false)
            {
                _crop.cropGrowthLevel++;
            }



            if (_crop.cropGrowthLevel >= _maxCropGrowth)
            {
                _crop.harvestable = true;
            }
            _square.color = next.color; //To test square
            //_light.color = next.color; //TO BE REMOVED WHEN LIGHTING IS DECIDED HERE FOR REFERENCE
            //_light.intensity = next.intensity; //TO BE REMOVED WHEN LIGHTING IS DECIDED HERE FOR REFERENCE
            _CycleMarks();
        }
    }

    //When called increments the marker index by 1 then normalizes its time to cycle length time.
    private void _CycleMarks()
    {
        _currentMarkIndex = (_currentMarkIndex + 1) % _marks.Length; //If the current mark reaches the the last mark, it will be reset to 0
        _nextMarkIndex = (_currentMarkIndex + 1) % _marks.Length; //If the next mark reaches the the last mark, it will be reset to 0
        _currentMarkTime = _marks[_currentMarkIndex].timeRatio * _cycleLength; //get the actual time value for current mark
        _nextMarkTime = _marks[_nextMarkIndex].timeRatio * _cycleLength; //get the actual time value for next mark
    }




}
