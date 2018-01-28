using GlobalGameJam2018Networking;
using UnityEngine;

public class ItemSink : MonoBehaviour
{
    public GlobalGameJam2018Networking.Pipe remotePipe;
    public GlobalGameJam2018Networking.PipesNetwork pipesNetwork;

    public int row;
    public int column;

    public void ProcessSinkItem(ItemBehaviour item)
    {
        if(pipesNetwork != null)
        {
            SendIngredient(item.material);
        }
        
        Destroy(item.gameObject);
    }

    private void SendIngredient(ColoredMaterial material)
    {
        ItemType itemType;
        switch(material.Material)
        {
            case Material.Fluid:
                itemType = ItemType.Liquid;
                break;
            case Material.Herbs:
                itemType = ItemType.Herb;
                break;
            default:
            case Material.Paste:
                return;
            case Material.Powder:
                itemType = ItemType.Powder;
                break;
            case Material.Vapor:
                itemType = ItemType.Steam;
                break;
        }

        IngredientColour ingredientColour;
        switch (material.Color)
        {
        case MaterialColor.Black:
            ingredientColour = IngredientColour.Black;
            break;
        case MaterialColor.Blue:
            ingredientColour = IngredientColour.Blue;
            break;
        case MaterialColor.Green:
            ingredientColour = IngredientColour.Green;
            break;
        case MaterialColor.Orange:
            ingredientColour = IngredientColour.Orange;
            break;
        case MaterialColor.Red:
            ingredientColour = IngredientColour.Red;
            break;
        case MaterialColor.Violet:
            ingredientColour = IngredientColour.Violet;
            break;
        case MaterialColor.Yellow:
            ingredientColour = IngredientColour.Yellow;
            break;
        default:
            return;
        }

        var ingredient = new Ingredient(itemType, ingredientColour);
        pipesNetwork.SendIngredient(ingredient, remotePipe);
    }
}
