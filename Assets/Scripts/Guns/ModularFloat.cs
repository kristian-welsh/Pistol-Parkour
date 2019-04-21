/* Represents a floating point number that wraps back around to zero past a maximum value
 */
public class ModularFloat
{
    private float value;
    private float maximum;

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

    /* Value of the float as a percentage of its maximum
     */
    public float Percent
    {
        get
        {
            return value / maximum;
        }
    }

    /* convenience method to add to the value more easily
     */
    public void Add(float num)
    {
        // use the setter to get the wrapping
        Value = value + num;
    }
}
