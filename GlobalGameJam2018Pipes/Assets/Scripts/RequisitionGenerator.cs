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

        private readonly System.Random random = new System.Random();
        private readonly Array colors = Enum.GetValues(typeof(MaterialColor));
        private readonly Array materials = Enum.GetValues(typeof(Material));
        private int scoreForTask; //Not needed yet. Just an idea.

        private GameObject displayingItem;

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
            Destroy(displayingItem);
            currentTask = null;
        }

        private IEnumerator GeneratingTasks()
        {
            while (true)
            {
                if (currentTask == null)
                {
                    currentTask = GenerateTask();
                    DisplayMeshRepresenation();
                }
                yield return null;
            }
        }

        private void DisplayMeshRepresenation()
        {
            GameObject displayingItem = null;
            switch (currentTask.Material)
            {
                case Material.Fluid:
                    displayingItem = Instantiate(fluidContainerPrefab, transform);
                    Debug.Log(("Item Fluid: " + displayingItem));
                    break;
                case Material.Herbs:
                    displayingItem = Instantiate(herbsContainerPrefab, transform);
                    Debug.Log(("Item Herbs: " + displayingItem));
                    break;
                case Material.Paste:
                    Debug.Log(pasteContainerPrefab);
                    displayingItem = Instantiate(pasteContainerPrefab, transform);
                    Debug.Log(("Item Paste: " + displayingItem));
                    break;
                case Material.Powder:
                    displayingItem = Instantiate(powderContainerPrefab, transform);
                    Debug.Log(("Item Powder: " + displayingItem));
                    break;
                case Material.Vapor:
                    displayingItem = Instantiate(vaporContainerPrefab, transform);
                    Debug.Log(("Item Vapor: " + displayingItem));
                    break;
            }
            if (displayingItem != null)
            {
                displayingItem.GetComponent<ItemBehaviour>().enabled = false;

                var particleSystem = GetComponentInChildren<ParticleSystem>();
                if (particleSystem != null)
                {
                    var main = particleSystem.main;
                    main.startColor = MixerScript.ConvertMaterialColor(currentTask.Color);
                }
                else
                {
                    var meshRenderer = GetComponentInChildren<MeshRenderer>();
                    meshRenderer.material.color = MixerScript.ConvertMaterialColor(currentTask.Color);
                }
            }
        }

        private ColoredMaterial GenerateTask()
        {
            MaterialColor materialColor = (MaterialColor)colors.GetValue(random.Next(colors.Length));
            Material material = (Material)materials.GetValue(random.Next(materials.Length));

            ColoredMaterial nextTask = new ColoredMaterial(material, materialColor);

            return nextTask;
        }

    }
}
