using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Renderer))]
public class Alert : MonoBehaviour
{
    public Material alertedMaterial;

    void OnEnable()
    {
        Renderer boxRenderer = GetComponent<Renderer>();
        boxRenderer.material = alertedMaterial;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
