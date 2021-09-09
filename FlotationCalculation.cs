/*
public class FlotationCalculation {
    public static FlotationParameters Controller;
    public float SecondsForNextSampling = 3;
    public float SecondsSinceLastSampling = 0;
    public static bool NextSamplingIsReady = false;

    void Awake()
    {
        Controller = new FlotationParameters();
        Controller.SetInitialVariables();
        TimeManager.LevelSeconds = 0;
    }

    private void Update()
    {
        if (TimeManager.SecondPassed) {
            SecondsSinceLastSampling++;
        }
        if (SecondsSinceLastSampling >= SecondsForNextSampling) {
            NextSamplingIsReady = true;
            SecondsSinceLastSampling = 0;
        } else{
            NextSamplingIsReady = false;
        }
    }
}

*/
