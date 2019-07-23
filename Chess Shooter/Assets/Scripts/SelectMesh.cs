using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMesh : MonoBehaviour
{
    public Mesh[] meshes;
    public Material[] materials;
    Player player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GetComponent<Renderer>().material = materials[player.getWaveNumber()];
        //Debug.Log("Level: " + player.getWaveNumber());
        GetComponent<MeshFilter>().mesh = meshes[Random.Range(1, meshes.Length)];
    }

}
