using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class characterSwitch : MonoBehaviour
{
    public GameObject activeCharacter;
    public int activeCharacterIndex;
    private Vector3 currentPosition;
    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> abilities = new List<GameObject>();
    public List<GameObject> avatars = new List<GameObject>();
    public ParticleSystem effect;
    public List<GameObject> avatarsSmall = new List<GameObject>();
    public float timeToSwitch;
    [SerializeField]
    private float switchDelay;
    public bool dead;
    public GameObject fuelBar;


    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerRespawn>().currentPlayer = gameObject;
        activeCharacterIndex = 0;
        activeCharacter = characters[activeCharacterIndex];
        activeCharacter.transform.position = FindObjectOfType<PlayerRespawn>().transform.position;
        currentPosition = activeCharacter.transform.position;

        for (int i = 0; i < avatars.Count; i++)
        {
            Image image = avatarsSmall[i].GetComponent<Image>();
            
            if (i == activeCharacterIndex)
            {
                var color = image.color;
                color.a = 1f;
                image.color = color;
                avatars[i].SetActive(true);
            } else
            {
                avatars[i].SetActive(false);
                var color = image.color;
                color.a = 0.3f;
                image.color = color;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        effect.transform.position = currentPosition;
        currentPosition = activeCharacter.transform.position;
        if (Time.time >= timeToSwitch && !dead)
        {
            if (Input.GetKeyDown("1"))
            {
                if (activeCharacterIndex != 0)
                {
                    fuelBar.SetActive(true);
                    SwapCharacter(0);
                    ActivateCharacter();
                    Shooting ability = FindObjectOfType<Shooting>();
                    if (ability.sonic)
                    {
                        ability.ResumeSonic();
                    }
                    else
                    {
                        ability.ResumeCooldown();
                    }
                }
                //FindObjectOfType<Fuel>().gameObject.SetActive(true);
                timeToSwitch = Time.time + switchDelay;
            }

            if (Input.GetKeyDown("2"))
            {
                if (activeCharacterIndex != 1)
                {
                    fuelBar.SetActive(false);
                    SwapCharacter(1);
                    ActivateCharacter();
                    Genji ability = FindObjectOfType<Genji>();
                    ability.ResumeCooldown();
                }
               // FindObjectOfType<Fuel>().gameObject.SetActive(false);
                timeToSwitch = Time.time + switchDelay;
            }

            if (Input.GetKeyDown("3"))
            {
                if (activeCharacterIndex != 2)
                {
                    fuelBar.SetActive(false);
                    SwapCharacter(2);
                    ActivateCharacter();
                    Tanya tanya = FindObjectOfType<Tanya>();
                    tanya.chargingJump = false;
                    tanya.jumpCharge = 0;
                    FindObjectOfType<JumpCharge>().SetHeight(0);
                    FindObjectOfType<PlayerMovementController>().jumpMultiplier = 0;
                    LaunchHook ability1 = FindObjectOfType<LaunchHook>();
                    ability1.ResumeCooldown();
                    ActivateShield ability2 = FindObjectOfType<ActivateShield>();
                    ability2.ResumeCooldown();
                    //FindObjectOfType<Fuel>().gameObject.SetActive(false);
                    timeToSwitch = Time.time + switchDelay;
                }
            }
        }
        

        activeCharacter.transform.position = currentPosition;


        void ActivateCharacter()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].name != activeCharacter.name)
                {
                    characters[i].GetComponent<Player>().parent.SetActive(false);
                    characters[i].SetActive(false);
                }
                else
                {
                    characters[i].GetComponent<Player>().parent.SetActive(true);
                    characters[i].SetActive(true);
                }
            }

            for (int i = 0; i < avatars.Count; i++)
            {
                if (i == activeCharacterIndex)
                {
                    avatars[i].SetActive(true);
                    Image image = avatarsSmall[i].GetComponent<Image>();
                    var color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
                else
                {
                    avatars[i].SetActive(false);
                    Image image = avatarsSmall[i].GetComponent<Image>();
                    var color = image.color;
                    color.a = 0.3f;
                    image.color = color;
                }
            }
        }
        
    }

    void SwapCharacter(int index)
    {
        if (activeCharacterIndex != index)
        {
            FindObjectOfType<AudioManager>().Play("Swap");
            FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[13].enabled = false;
            activeCharacterIndex = index;
            activeCharacter = characters[activeCharacterIndex];
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].GetComponent<SpriteRenderer>().color = Color.white;
                characters[i].transform.parent.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                characters[i].GetComponent<PlayerMovementController>().jumpMultiplier = 0;
            }
            characters[2].GetComponent<Tanya>().landed = true;
            effect.Play();
        }
    }
}