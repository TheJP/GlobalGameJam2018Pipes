﻿using System;
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

        //private readonly System.Random random = new System.Random(System.DateTime.Today.DayOfYear);
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
            Debug.Log("Clear current Task" + currentTask.ToString());
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
            displayingItem = null;
            switch (currentTask.Material)
            {
                case Material.Fluid:
                    displayingItem = Instantiate(fluidContainerPrefab, transform);
                    break;
                case Material.Herbs:
                    displayingItem = Instantiate(herbsContainerPrefab, transform);
                    break;
                case Material.Paste:
                    Debug.Log(pasteContainerPrefab);
                    displayingItem = Instantiate(pasteContainerPrefab, transform);
                    break;
                case Material.Powder:
                    displayingItem = Instantiate(powderContainerPrefab, transform);
                    break;
                case Material.Vapor:
                    displayingItem = Instantiate(vaporContainerPrefab, transform);
                    break;
                default:
                    Debug.Log("Item of current Task is unknown: " + currentTask.ToString());
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
            MaterialColor materialColor = (MaterialColor)colors.GetValue(UnityEngine.Random.Range(0, colors.Length));
            Material material = (Material)materials.GetValue(UnityEngine.Random.Range(0, materials.Length));

            ColoredMaterial nextTask = new ColoredMaterial(material, materialColor);

            return nextTask;
        }

    }
}
