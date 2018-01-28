public class MixerScript {
    
    public ColoredMaterial Mix (ColoredMaterial mat1, ColoredMaterial mat2) {

        // if one of the materials is herbs, the other color is ignored
        MaterialColor newColor = MaterialColor.Black;
        if (mat1.Material == Material.Herbs) {
            newColor = mat1.Color; 
        }
        else if (mat2.Material == Material.Herbs) {
            newColor = mat2.Color;
        }
        else if (mat1.Material == Material.Herbs || mat2.Material == Material.Herbs) {
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

        // otherwise depends if primary color or not

        bool isPrimaryColor1 = IsPrimaryColor (color1);
        bool isPrimaryColor2 = IsPrimaryColor (color2);

        // if both colors are not primary (but not same), we get the magic color
        if (!isPrimaryColor1 && !isPrimaryColor2)
            return MaterialColor.Black;

        // if both colors are primary colors, mix them
        // could be done via Lookuptable
        if (isPrimaryColor1 && isPrimaryColor2) {
            switch (color1) {
            case MaterialColor.Red: 
                if (color2 == MaterialColor.Yellow)
                    return MaterialColor.Orange;
                if (color2 == MaterialColor.Blue)
                    return MaterialColor.Violet;
                return MaterialColor.Black; 
            case MaterialColor.Yellow:
                if (color2 == MaterialColor.Red)
                    return MaterialColor.Orange;
                if (color2 == MaterialColor.Blue)
                    return MaterialColor.Green;
                return MaterialColor.Black; 
            case MaterialColor.Blue:
                if (color2 == MaterialColor.Red)
                    return MaterialColor.Violet;
                if (color2 == MaterialColor.Yellow)
                    return MaterialColor.Green;
                return MaterialColor.Black; 
            default:
                return MaterialColor.Black;
            }
        }

        // if only one color is primary, the primary color wins
        if (isPrimaryColor1) {
            return color1;
        } else
            return color2;
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


    private bool IsPrimaryColor(MaterialColor color) {

        if (color == MaterialColor.Red)
            return true;
        if (color == MaterialColor.Blue)
            return true;
        if (color == MaterialColor.Yellow)
            return true;
        return false;
    }


}