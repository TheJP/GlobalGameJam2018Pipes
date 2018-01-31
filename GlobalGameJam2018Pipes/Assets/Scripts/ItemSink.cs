using Assets.Scripts;
using GlobalGameJam2018Networking;
using UnityEngine;

public class ItemSink : MonoBehaviour
{
    public GlobalGameJam2018Networking.Pipe remotePipe;
    public GlobalGameJam2018Networking.PipesNetwork pipesNetwork;

    public int row;
    public int column;

    public bool SinglePlayer { get; set; }


    private RequisitionGenerator requisitionGenerator;

    private void Start()
    {
        if (SinglePlayer)
        {
            requisitionGenerator = GetComponentInChildren<RequisitionGenerator>();
        }
    }

    public void ProcessSinkItem(ItemBehaviour item)
    {
        if (pipesNetwork != null)
        {
            SendIngredient(item.material);
        }

        if (SinglePlayer)
        {
            ColoredMaterial currentTask = requisitionGenerator.CurrentTask;
            if (item.material.Color == currentTask.Color && item.material.Material == currentTask.Material) //TODO: Implement Equials in ColoredMaterial
            {
                requisitionGenerator.ClearCurrentTask();
                //TODO: Add Money and/or Points. Maybe depending on the difficulti of the Item/Task
            }
            else
            {
                Debug.Log($"Wanted Item was {currentTask} but received {item.material}! \nPunish Player with electrical shocks. Bzzzzzzz!!!!");
                //TODO: Maybe decrease score or money when wrong Item.
            }
        }

        Destroy(item.gameObject);
    }

    private void SendIngredient(ColoredMaterial material)
    {
        ItemType itemType;
        switch (material.Material)
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
