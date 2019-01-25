public class ModularFloat
{
    private float value;
    private float maximum;

    /** A float that wraps back around to 0 once it reaches a maximum value*/
    public ModularFloat(float maximum, float value = 0)
    {
        this.value = value;
        this.maximum = maximum;
    }

    public float Value
    {
        set
        {
            this.value = value;
            this.value %= maximum;
        }
    }

    public float Percent
    {
        get
        {
            return value / maximum;
        }
    }

    public void Add(float num)
    {
        Value = value + num;
    }
}
