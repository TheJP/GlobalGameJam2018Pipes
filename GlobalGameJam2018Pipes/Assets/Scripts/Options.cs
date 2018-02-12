using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Collect settings for the game.
 */
[Serializable]
public class Options
{
    [SerializeField] private ProbabilityOptions<MaterialColor> sourceColorProbab;
    [SerializeField] private ProbabilityOptions<MaterialColor> sinkColorProbab;
    [SerializeField] private ProbabilityOptions<Material> sourceMaterialProbab;
    [SerializeField] private ProbabilityOptions<Material> sinkMaterialProbab;
    
    public Options()
    {
        // init with default values
        
        sourceColorProbab = new ProbabilityOptions<MaterialColor>();
        sourceColorProbab.SetProbability(MaterialColor.Red, 10);
        sourceColorProbab.SetProbability(MaterialColor.Orange, 0);
        sourceColorProbab.SetProbability(MaterialColor.Yellow, 10);
        sourceColorProbab.SetProbability(MaterialColor.Green, 0);
        sourceColorProbab.SetProbability(MaterialColor.Blue, 10);
        sourceColorProbab.SetProbability(MaterialColor.Violet, 0);
        sourceColorProbab.SetProbability(MaterialColor.Black, 0);
        sourceColorProbab.AutoRecalc = true;

        sinkColorProbab = new ProbabilityOptions<MaterialColor>();
        sinkColorProbab.SetProbability(MaterialColor.Red, 10);
        sinkColorProbab.SetProbability(MaterialColor.Orange, 3);
        sinkColorProbab.SetProbability(MaterialColor.Yellow, 10);
        sinkColorProbab.SetProbability(MaterialColor.Green, 3);
        sinkColorProbab.SetProbability(MaterialColor.Blue, 10);
        sinkColorProbab.SetProbability(MaterialColor.Violet, 3);
        sinkColorProbab.SetProbability(MaterialColor.Black, 1);
        sinkColorProbab.AutoRecalc = true;

    }

    public void SetProbability(MaterialColor col, int rawProbability, bool forSource)
    {
        if (forSource)
            sourceColorProbab.SetProbability(col, rawProbability);
        else
            sinkColorProbab.SetProbability(col, rawProbability);
    }

    public void SetProbability(Material mat, int rawProbability, bool forSource)
    {
        if (forSource)
            sourceMaterialProbab.SetProbability(mat, rawProbability);
        else
            sinkMaterialProbab.SetProbability(mat, rawProbability);
    }

    /**
     * Get the raw probability number (for displaying in options dialog).
     * @param forSource true to get for source, false to get for sink
     */
    public int GetProbabilityRaw(MaterialColor col, bool forSource)
    {
        int value = 0;
        if (forSource)
            value = sourceColorProbab.GetProbabilityRaw(col);
        else
            value = sinkColorProbab.GetProbabilityRaw(col);
        Debug.Log($"options requested (for source? {forSource}): raw probab for {col}, returns {value}.");
        return value;
    }

    /**
     * Get the raw probability number (for displaying in options dialog).
     * @param forSource true to get for source, false to get for sink
     */
    public int GetProbabilityRaw(Material mat, bool forSource)
    {
        int value = 0;
        if (forSource) 
            value = sourceMaterialProbab.GetProbabilityRaw(mat);
        else
            value = sinkMaterialProbab.GetProbabilityRaw(mat);
        Debug.Log($"options requested (for source? {forSource}): raw probab for {mat}, returns {value}.");
        return value;
    }

    /**
     * Get the correct color for a random number between [0, 1].
     * @param forSource true to get for source, false to get for sink
     */
    public MaterialColor GetColorForRandom (float randValue, bool forSource)
    {
        if (forSource)
            return sourceColorProbab.GetForRandom(randValue);
        else
            return sinkColorProbab.GetForRandom(randValue);
    }

    /**
     * Get the correct material for a random number between [0, 1].
     * @param forSource true to get for source, false to get for sink
     */
    public Material GetMaterialForRandom (float randValue, bool forSource)
    {
        if (forSource)
            return sourceMaterialProbab.GetForRandom(randValue);
        else
            return sinkMaterialProbab.GetForRandom(randValue);
    }
 
}

[Serializable]
internal class ProbabilityOptions<T>
{
    [SerializeField] private Boolean autoRecalc;
    [SerializeField] private Dictionary<T, int> probabRaw;
    [SerializeField] private Dictionary<T, float> probab;
    [SerializeField] private Dictionary<T, float> probabDivider;
    // Dividers divide the interval [0, 1[ according to the probability.
    // E.g. if raw probabilities are 1, 2, 0, 3, 4, 0, 0, then the probabilities are
    // 0.1, 0.2, 0, 0.3, 0.4, 0, 0 and the dividers are 0.1, 0.3, 0.3, 0.6, 1, 1, 1.

    
    /**
     * New object, with autoRecalc set to false. Fill it with default values
     * for all possible keys, then set autoRecalc to true.
     */
    public ProbabilityOptions()
    {
        // init with default values
        probabRaw = new Dictionary<T, int>();
        probab = new Dictionary<T, float>();
        probabDivider = new Dictionary<T, float>();
        autoRecalc = false;
    }

    public bool AutoRecalc
    {
        get { return autoRecalc; }
        set
        {
            // recalculate when autoRecalc is switched on
            if (!autoRecalc && value) this.Recalculate();
            autoRecalc = value;
        }
    }

    /**
     * Recalculate the internal values. If autoRecalc is true, this will be called
     * wherever a probability is change via SetProbability().
     */
    private void Recalculate ()
    {
        int sum = 0;
        foreach (int value in probabRaw.Values)
            sum += value;
        //Debug.Log($"Sum of raw probabilities is {sum}");

        if (sum <= 0)
            throw new System.InvalidOperationException("probabilities must have a sum > 0");

        float divider = 0;
        foreach (KeyValuePair<T, int> entry in probabRaw)
        {
            T color = entry.Key;
            probab[color] = (float)probabRaw[color] / sum;
            //Debug.Log($"Probability of {entry.Key} set to {probab[color]}");
            divider += probab[color];
            probabDivider[color] = divider;
            //Debug.Log($"Probability divider of {entry.Key} set to {probabDivider[color]}");
        }
    }

    /**
     * Set a probability (as raw integer value). 
     */
    public void SetProbability(T key, int rawProbability)
    {
        if (rawProbability < 0)
            throw new System.InvalidOperationException("Probabilities must be greater or equal 0");

        probabRaw[key] = rawProbability;
        if (autoRecalc)  Recalculate();
    }

    /**
     * Get raw probability value for this color (used from options dialog).
     */
    public int GetProbabilityRaw (T key)
    {
        return probabRaw[key];
    }

    /**
     * Get probability for this color.
     * To get the correct color for a random value between [0, 1],
     * use #GetColorForRandom().
     */
    public float GetProbability(T key)
    {
        return probab[key];
    }

    /**
     * Get the correct T for a random number between [0, 1].
     */
    public T GetForRandom (float randValue)
    {
        foreach (KeyValuePair<T, float> entry in probabDivider)
        {
            if (!(randValue < entry.Value)) continue;
            //Debug.Log($"Color for random number {randValue} is {entry.Key}");
            return entry.Key;
        }
        throw new InvalidOperationException("This should never happen...");
    }

}
