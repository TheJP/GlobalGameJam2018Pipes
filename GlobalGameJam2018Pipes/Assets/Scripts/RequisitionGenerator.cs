using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class RequisitionGenerator : MonoBehaviour
    {

        public GameObject fluidContainerPrefab;
        public GameObject vaporContainerPrefab;
        public GameObject powderContainerPrefab;
        public GameObject herbsContainerPrefab;
        public GameObject pasteContainerPrefab;

        private System.Random random = new System.Random();
        private Array colors = Enum.GetValues(typeof(MaterialColor));
        private Array materials = Enum.GetValues(typeof(Material));
        private int scoreForTask; //Not needed yet. Just an idea.

        private ColoredMaterial currentTask;
        public ColoredMaterial CurrentTask { get { return currentTask; } }

        private void Start()
        {
            StartGenerator();
        }

        public void StartGenerator()
        {
            StartCoroutine(GeneratingTasks());
        }

        public void StopGenerator()
        {
            StopCoroutine(GeneratingTasks());
        }

        public void ClearCurrentTask()
        {
            currentTask = null;
        }

        private IEnumerator GeneratingTasks()
        {
            if (currentTask == null)
            {
                currentTask = GenerateTask();
                DisplayMeshRepresenation();
            }
            yield return null;
        }

        private void DisplayMeshRepresenation()
        {
            GameObject item = null;
            switch (currentTask.Material)
            {
                case Material.Fluid:
                    item = Instantiate(fluidContainerPrefab, transform);
                    break;
                case Material.Herbs:
                    item = Instantiate(herbsContainerPrefab, transform);
                    break;
                case Material.Paste:
                    item = Instantiate(pasteContainerPrefab, transform);
                    break;
                case Material.Powder:
                    item = Instantiate(powderContainerPrefab, transform);
                    break;
                case Material.Vapor:
                    item = Instantiate(vaporContainerPrefab, transform);
                    break;
            }
            if (item != null)
            {
                item.GetComponent<ItemBehaviour>().enabled = false;
                item.GetComponentInChildren<MeshRenderer>().material.color = ConvertMaterialColor(currentTask.Color);
            }
        }

        private ColoredMaterial GenerateTask()
        {
            MaterialColor materialColor = (MaterialColor)colors.GetValue(random.Next(colors.Length));
            Material material = (Material)materials.GetValue(random.Next(materials.Length));

            ColoredMaterial nextTask = new ColoredMaterial(material, materialColor);

            return nextTask;
        }

        private static Color ConvertMaterialColor(MaterialColor materialColor)
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
                    return new Color(1, 0, 1);
                case MaterialColor.Black:
                    return Color.black;
                default:
                    return Color.magenta;
            }
        }
    }
}
