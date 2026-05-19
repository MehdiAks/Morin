using UnityEngine;

public class CubeTest : MonoBehaviour
{
    // un entier int nommé compterur initialisé à 1
    // un boolen "bool nommé compteurActive initialisé à true
    // un float nommé timer initialisé à 0
    //une chaine de caractère string nommé nomCube initialisé à "Cube 1"

    public int compteur = 1;
    public bool compteurActive = true;
    public float timer = 0;
    public string nomCube = "Cube 1";

    public GameObject sphere;
    private bool sphereDestroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(compteur < 400){
            sphere.GetComponent<SphereTest>().MoveSphere();
            compteur++;
        } else if(sphereDestroyed == false){
            Destroy(sphere);
            sphereDestroyed = true;
        }

    }
}
