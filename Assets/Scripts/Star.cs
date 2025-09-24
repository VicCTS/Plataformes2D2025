using UnityEngine;
using UnityEngine.Audio;

public class Star : MonoBehaviour
{
    //GameManager _gameManager;

    [SerializeField]private AudioClip _starSFX;

    void Awake()
    {
        //_gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void Interaction()
    {
        //_gameManager.AddStar();

        GameManager.instance.AddStar();
        AudioManager.instance.ReproduceSound(_starSFX);
        Destroy(gameObject);
    }
}
