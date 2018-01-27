//using System;
//using UnityEngine;

//public class MixerScript : MonoBehaviour
//{

//    public ColoredMaterial Mix(ColoredMaterial mat1, ColoredMaterial mat2)
//    {
//        // if one of the materials is herbs, the other color is ignored
//        MaterialColor newColor;
//        if (mat1.material == Material.Herbs)
//        {
//            newColor = mat1.color;
//        }
//        else if (mat2.material == Material.Herbs)
//        {
//            newColor = mat2.color;
//        }
//        else (mat1.material == Material.Herbs || mat2.material == Material.Herbs) {
//            // in all other cases, colors are mixed
//            newColor = mixColor(mat1.color, mat2.color);
//        }

//        // mix materials
//        Material newMaterial = mixMaterial(mat1.material, mat2.material);

//        ColoredMaterial coloredMaterial = new ColoredMaterial();
//        coloredMaterial.color = newColor;
//        coloredMaterial.material = newMaterial;
//        return coloredMaterial;
//    }


//    private MaterialColor mixColor(MaterialColor color1, MaterialColor color2)
//    {

//        // if the two colors are same, the mix stays same
//        if (color1 == color2)
//            return color1;

//        // otherwise depends if primary color or not

//        bool isPrimaryColor1 = isPrimaryColor(color1);
//        bool isPrimaryColor2 = isPrimaryColor(color2);

//        // if both colors are not primary (but not same), we get the magic color
//        if (!isPrimaryColor1 && !isPrimaryColor2)
//            return MaterialColor.Black;

//        // if both colors are primary colors, mix them
//        // could be done via Lookuptable
//        if (isPrimaryColor1 && isPrimaryColor2)
//        {
//            switch (color1)
//            {
//                case MaterialColor.Red:
//                    if (color2 == MaterialColor.Yellow)
//                        return MaterialColor.Orange;
//                    if (color2 == MaterialColor.Blue)
//                        return MaterialColor.Violet;
//                    return MaterialColor.Black;
//                    break;
//                case MaterialColor.Yellow:
//                    if (color2 == MaterialColor.Red)
//                        return MaterialColor.Orange;
//                    if (color2 == MaterialColor.Blue)
//                        return MaterialColor.Green;
//                    return MaterialColor.Black;
//                    break;
//                case MaterialColor.Blue:
//                    if (color2 == MaterialColor.Red)
//                        return MaterialColor.Violet;
//                    if (color2 == MaterialColor.Yellow)
//                        return MaterialColor.Green;
//                    return MaterialColor.Black;
//                    break;
//                default:
//                    return MaterialColor.Black;
//            }
//        }

//        // if only one color is primary, the primary color wins
//        if (isPrimaryColor1)
//        {
//            return color1;
//        }
//        else
//            return color2;
//    }


//    private Material mixMaterial(Material mat1, Material mat2)
//    {

//        // if both materials are same, the mix stays the same
//        if (mat1 == mat2)
//            return mat1;

//        // herbs only change the color of the other material, so return the other material
//        if (mat1 == Material.Herbs)
//            return mat2;
//        if (mat2 == Material.Herbs)
//            return mat1;

//        // otherwise mix
//        switch (mat1)
//        {
//            case (Material.Powder):
//                if (mat2 == Material.Vapor)
//                    return Material.Fluid;
//                if (mat1 == Material.Fluid)
//                    return Material.Paste;
//                else
//                    return Material.Powder; // should not occur, just to be sure
//                break;

//            case (Material.Fluid):
//                if (mat2 == Material.Powder)
//                    return Material.Paste;
//                else
//                    return Material.Fluid;
//                break;

//            case (Material.Vapor):
//                return Material.Fluid;
//                break;

//            case (Material.Paste):
//                return Material.Paste;
//                break;
//            default:
//                throw new NotImplementedException();
//                break;
//        }
//    }


//    private bool isPrimaryColor(MaterialColor color)
//    {

//        if (color == MaterialColor.Red)
//            return true;
//        if (color == MaterialColor.Blue)
//            return true;
//        if (color == MaterialColor.Yellow)
//            return true;
//        return false;
//    }


//}