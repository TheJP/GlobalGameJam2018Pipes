using System.Collections.Generic;
using UnityEngine;

public class MixerScript {
    
    private Dictionary<MaterialColor, Dictionary<MaterialColor, MaterialColor>> mixTable;

    public MixerScript ()
    {
        mixTable = new Dictionary<MaterialColor, Dictionary<MaterialColor, MaterialColor>>();
        // Stores all mix possibilities. Rules:
        // - primary colors (red, yellow, blue) mix as usual (e.g. red + yellow = orange)
        // - secondary color (orange, green, violet) with primary color which is already in the mix: primary color wins
        // - secondary color with primary color which is not yet in the mix: black
        // - black and other color: black

        // mix with red
        var mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Red },
            { MaterialColor.Orange, MaterialColor.Red },
            { MaterialColor.Yellow, MaterialColor.Orange },
            { MaterialColor.Green,  MaterialColor.Black },
            { MaterialColor.Blue,   MaterialColor.Violet },
            { MaterialColor.Violet, MaterialColor.Red },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Red, mix);

        // mix with orange
        mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Red },
            { MaterialColor.Orange, MaterialColor.Orange },
            { MaterialColor.Yellow, MaterialColor.Yellow },
            { MaterialColor.Green,  MaterialColor.Black },
            { MaterialColor.Blue,   MaterialColor.Black },
            { MaterialColor.Violet, MaterialColor.Black },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Orange, mix);

        // mix with yellow
        mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Orange },
            { MaterialColor.Orange, MaterialColor.Yellow },
            { MaterialColor.Yellow, MaterialColor.Yellow },
            { MaterialColor.Green,  MaterialColor.Yellow },
            { MaterialColor.Blue,   MaterialColor.Green },
            { MaterialColor.Violet, MaterialColor.Black },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Yellow, mix);

        // mix with green
        mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Black },
            { MaterialColor.Orange, MaterialColor.Black },
            { MaterialColor.Yellow, MaterialColor.Yellow },
            { MaterialColor.Green,  MaterialColor.Green },
            { MaterialColor.Blue,   MaterialColor.Blue },
            { MaterialColor.Violet, MaterialColor.Black },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Green, mix);

        // mix with blue
        mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Violet },
            { MaterialColor.Orange, MaterialColor.Black },
            { MaterialColor.Yellow, MaterialColor.Green },
            { MaterialColor.Green,  MaterialColor.Blue },
            { MaterialColor.Blue,   MaterialColor.Blue },
            { MaterialColor.Violet, MaterialColor.Blue },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Blue, mix);

        // mix with violet
        mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Red },
            { MaterialColor.Orange, MaterialColor.Black },
            { MaterialColor.Yellow, MaterialColor.Black },
            { MaterialColor.Green,  MaterialColor.Black },
            { MaterialColor.Blue,   MaterialColor.Blue },
            { MaterialColor.Violet, MaterialColor.Violet },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Violet, mix);

        // mix with black
        mix = new Dictionary<MaterialColor, MaterialColor>
        {
            { MaterialColor.Red,    MaterialColor.Black },
            { MaterialColor.Orange, MaterialColor.Black },
            { MaterialColor.Yellow, MaterialColor.Black },
            { MaterialColor.Green,  MaterialColor.Black },
            { MaterialColor.Blue,   MaterialColor.Black },
            { MaterialColor.Violet, MaterialColor.Black },
            { MaterialColor.Black,  MaterialColor.Black }
        };
        mixTable.Add(MaterialColor.Black, mix);
    }

    public ColoredMaterial Mix (ColoredMaterial mat1, ColoredMaterial mat2) {

        // if one of the materials is herbs, the other color is ignored
        MaterialColor newColor = MaterialColor.Black;
        if (mat1.Material == Material.Herbs) {
            newColor = mat1.Color; 
        }
        else if (mat2.Material == Material.Herbs) {
            newColor = mat2.Color;
        }
        else /*if (mat1.Material != Material.Herbs && mat2.Material != Material.Herbs)*/ {
            // in all other cases, colors are mixed
            newColor = MixColor (mat1.Color, mat2.Color);
        }

        // mix Materials
        Material newMaterial = MixMaterial (mat1.Material, mat2.Material);

        return new ColoredMaterial (newMaterial, newColor);
    }


    private MaterialColor MixColor (MaterialColor color1, MaterialColor color2) {

        // if the two colors are same, the mix stays same
        if (color1 == color2)  
            return color1;

        return mixTable[color1][color2];
    }


    private Material MixMaterial (Material mat1, Material mat2) {

        // if both Materials are same, the mix stays the same
        if (mat1 == mat2)
            return mat1;

        // herbs only change the color of the other material, so return the other material
        if (mat1 == Material.Herbs)
            return mat2;
        if (mat2 == Material.Herbs)
            return mat1;
        
        // otherwise mix
        switch (mat1) {
        case(Material.Powder):
            if (mat2 == Material.Vapor)
                return Material.Fluid;
            if (mat1 == Material.Fluid)
                return Material.Paste;
            else 
                return Material.Powder; // should not occur, just to be sure

        case(Material.Fluid):
            if (mat2 == Material.Powder)
                return Material.Paste;
            else
                return Material.Fluid;

        case(Material.Vapor):
            return Material.Fluid;

        case(Material.Paste):
            return Material.Paste;

        default:
            return Material.Paste;
        }
    }

    public static Color ConvertMaterialColor(MaterialColor materialColor)
    {
        switch (materialColor)
        {
            case MaterialColor.Red:
                return Color.red;
            case MaterialColor.Yellow:
                return Color.yellow;
            case MaterialColor.Blue:
                return Color.blue;
            case MaterialColor.Green:
                return Color.green;
            case MaterialColor.Orange:
                return new Color(1, 0xa0 / 255.0f, 0);
            case MaterialColor.Violet:
                return new Color(0x88 / 255.0f, 0, 1);
            case MaterialColor.Black:
                return Color.black;
            default:
                return Color.magenta;
        }
    }
}