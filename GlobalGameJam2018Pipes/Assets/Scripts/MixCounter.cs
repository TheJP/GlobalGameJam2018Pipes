using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/**
 * Class stores all possible mixtures from a given set of ingredients, 
 * together with their number of occurrences. 
 * This can be printed to a file, or mixed with a new set of ingredients. 
 * 
 * Later it will also be possible to mix it with the contents of a second MixCounter 
 * (to simulate mixing 2 and 2 ingredients and then mixing these two results).
 * This is necessary because our mixing is not commutative: mixing (2 + 1) + 1 
 * ingredients is not the same as mixing 2 + (1 + 1).
 */ 
public class MixCounter {

    private readonly List<ColoredMaterial> primaryIngred;
    private readonly List<ColoredMaterial> allIngred;
    private readonly string docPath;

    private readonly MaterialColor[] primaryColors = { MaterialColor.Red, MaterialColor.Yellow, MaterialColor.Blue };    // start with primary colors
    private readonly Material[] primaryMaterials = { Material.Fluid, Material.Herbs, Material.Powder, Material.Vapor };  // start without paste

    private MixerScript mixerScript;
    private Dictionary<ColoredMaterial, int> mixDict;
    private int total;

    /**
     * param name="docPath" path for the output files
     */
    MixCounter(string docPath)
    {
        mixerScript = new MixerScript();
        this.docPath = docPath;

        primaryIngred = new List<ColoredMaterial>();
        foreach (MaterialColor color in primaryColors)
            foreach (Material mat in primaryMaterials)
                primaryIngred.Add(new ColoredMaterial(mat, color));

        allIngred = new List<ColoredMaterial>();
        foreach (MaterialColor color in Enum.GetValues(typeof(MaterialColor)))
            foreach (Material mat in Enum.GetValues(typeof(Material)))
                allIngred.Add(new ColoredMaterial(mat, color));
    }

    public List<ColoredMaterial> PrimaryIngredients
    {
        get { return primaryIngred; }
    }

    public List<ColoredMaterial> AllIngredients
    {
        get { return allIngred; }
    }

    /**
     * Mix each occurrence in this instance with the given material list and store the new 
     * occurrences. 
     * 
     * param name="matList"   materials to mix into this MixCounter 
     */
    public void Mix(List<ColoredMaterial> matList)
    {
        Debug.Log($"MixCounter: Mix with {matList.Count} materials");

        if (mixDict == null)
        {
            // if this is the first material list added to the counter, just add them all with occurrence 1
            mixDict = InitCounter(allIngred);
            foreach (ColoredMaterial mat in matList)
                mixDict[mat] = 1;
            total = matList.Count;
            Debug.Log($"MixCounter: initalized with new Matlist, length {matList.Count}");
        }
        else
        {
            // temporarily store the result in a separate dict
            Dictionary<ColoredMaterial, int> newMixDict = InitCounter(allIngred);
            int newTotal = 0;

            foreach (ColoredMaterial mat in matList)
            {
                int newOccur = MixIntoTable(mat, 1, mixDict, newMixDict);
                newTotal += newOccur;
            }
            ListToDebug(newMixDict, "after mixing with a material list");

            // forget the old dict and keep the new one, same for total
            mixDict = newMixDict;
            total = newTotal;
            Debug.Log("MixCounter: Mix finished - " + total);

        }
    }

    /**
     * Mix each occurrence in this instance with the occurrences in the given MixCountermaterial list
     * and store the new occurrences.
     * Note that this cannot be done when this MixCounter is empty. 
     * 
     * param name="matList"   materials to mix into this MixCounter 
     */
    public void Mix(MixCounter counter)
    {
        Debug.Log("MixCounter: Mix " + counter.mixDict.Count);

        if (mixDict == null)
            throw new System.InvalidOperationException("MixCounter must not be empty when mixing with an other MixCounter");            

        // temporarily store the result in a separate dict
        Dictionary<ColoredMaterial, int> newMixDict = InitCounter(allIngred);
        int newTotal = 0;

        foreach (KeyValuePair<ColoredMaterial, int> entry in counter.mixDict)
        {
            int newOccur = MixIntoTable(entry.Key, entry.Value, mixDict, newMixDict);
            newTotal += newOccur;                
        }
        ListToDebug(newMixDict, "after mixing with a second MixCounter");

        // forget the old dict and keep the new one, same for total
        mixDict = newMixDict;
        total = newTotal;
        Debug.Log("MixCounter: Mix finished - " + total);
    }

    /**
     * Mix a material (and its number of occurrences) with the given mix table 
     * and use the result as the new content of the class.
     */
    private int MixIntoTable(ColoredMaterial mat, int numOccur, Dictionary<ColoredMaterial, int> mix, Dictionary<ColoredMaterial, int> newMix)
    {
        int counter = 0;
        foreach (KeyValuePair<ColoredMaterial, int> entry in mix)
        {
            // mix materials and multiply occurrences
            ColoredMaterial newMat = mixerScript.Mix(mat, entry.Key);
            int newOccur = numOccur * entry.Value;
            // store it in newMix
            newMix[newMat] += newOccur;
            counter += newOccur;
        }
        return counter;
    }

    private Dictionary<ColoredMaterial, int> InitCounter(List<ColoredMaterial> ingredients)
    {
        Dictionary<ColoredMaterial, int> dict = new Dictionary<ColoredMaterial, int>();
        foreach (ColoredMaterial colMat in ingredients)
        {
            dict.Add(colMat, 0);
        }
        Debug.Log($"InitCounter: finished, have {dict.Count} elements");
        return dict;
    }

    private void ListToDebug (Dictionary<ColoredMaterial, int> dict, String logTitle)
    {
        Debug.Log("mixDict " + logTitle + ":");
        foreach (KeyValuePair<ColoredMaterial, int> entry in dict)
            Debug.Log($"- key {entry.Key}, value {entry.Value}");
    }

    /**
     * param name="filename" filename (without path) for result, file will be crated inside docPath from constructor
     */
    public void WriteToFile(string filename)
    {
        string outpath = this.docPath + @"\" + filename;
        Debug.Log("WriteToFile into " + outpath);

        // output in a cvs file like
        // total: 231, Fluid, Vapor, Powder, Herbs, Paste
        // Red,1,5,3,5,0 
        // Orange,0,2,5,5,2
        // ...
        // Black,2,5,2,12,23

        using (StreamWriter outputFile = new StreamWriter(outpath, false)) // false -> do not append
        {
            // output in a cvs file like
            // total: 231, Fluid, Vapor, Powder, Herbs, Paste
            // Red,1,5,3,5,0 
            // Orange,0,2,5,5,2
            // ...
            // Black,2,5,2,12,23

            // write title line
            outputFile.Write("total: ");
            outputFile.Write(this.total);
            outputFile.Write(",");
            foreach (Material mat in Enum.GetValues(typeof(Material)))
            {
                outputFile.Write(mat);
                outputFile.Write(",");
            }   // this leaves an unneccessary comma at the end - don't care
            outputFile.WriteLine();

            // write a line for each color
            foreach (MaterialColor color in Enum.GetValues(typeof(MaterialColor)))
            {
                outputFile.Write(color);
                outputFile.Write(",");
                // for each material, get the mix and and write the number
                foreach (Material mat in Enum.GetValues(typeof(Material)))
                {
                    outputFile.Write(mixDict[new ColoredMaterial(mat, color)]);
                    outputFile.Write(",");
                }   // this leaves an unneccessary comma at the end - don't care
                outputFile.WriteLine();
            }

            // then sum up per color
            outputFile.WriteLine();
            foreach (MaterialColor color in Enum.GetValues(typeof(MaterialColor)))
            {
                    // sum up values for all materials
                    int totalForColor = 0;
                foreach (Material mat in Enum.GetValues(typeof(Material)))
                    totalForColor += mixDict[new ColoredMaterial(mat, color)];
                outputFile.Write(color);
                outputFile.Write(",");
                outputFile.WriteLine(totalForColor);
            }

            outputFile.WriteLine();

            // then sum up per material
            outputFile.WriteLine();
            foreach (Material mat in Enum.GetValues(typeof(Material)))
            {
                // sum up values for all colors
                int totalForMat = 0;
                foreach (MaterialColor color in Enum.GetValues(typeof(MaterialColor)))
                    totalForMat += mixDict[new ColoredMaterial(mat, color)];
                outputFile.Write(mat);
                outputFile.Write(",");
                outputFile.WriteLine(totalForMat);
            }
        }
    }

  
    public static void WriteMixOccurrenceTables()
    {
        string path = Directory.GetCurrentDirectory();
        // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        MixCounter mixCounter = new MixCounter(path + @"\..");

        mixCounter.Mix(mixCounter.PrimaryIngredients);
        mixCounter.WriteToFile("occurrences_1.cvs");

        mixCounter.Mix(mixCounter.PrimaryIngredients);
        mixCounter.WriteToFile("occurrences_2.cvs");

        mixCounter.Mix(mixCounter.PrimaryIngredients);
        mixCounter.WriteToFile("occurrences_2+1.cvs");

        mixCounter.Mix(mixCounter.PrimaryIngredients);
        mixCounter.WriteToFile("occurrences_2+1+1.cvs");

        
        // To calculate the statistics when mixing 2+2 ingredients, we could clone
        // the mixCounter above at a status when it has 2 ingredients in it. But I'm
        // too lazy to implement cloning, so I do the same mixing again, twice.
        
        mixCounter = new MixCounter(path + @"\..");
        mixCounter.Mix(mixCounter.PrimaryIngredients);
        mixCounter.Mix(mixCounter.PrimaryIngredients);

        MixCounter mixCounter2 = new MixCounter(path + @"\..");
        mixCounter2.Mix(mixCounter2.PrimaryIngredients);
        mixCounter2.Mix(mixCounter2.PrimaryIngredients);
        
        mixCounter.Mix(mixCounter2);
        mixCounter.WriteToFile("occurrences_2+2.cvs");
    }

}
