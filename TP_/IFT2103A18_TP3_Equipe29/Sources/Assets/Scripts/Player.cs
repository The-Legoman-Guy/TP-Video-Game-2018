using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Controls controls;
    public float posX;
    public float posY;
    public float speed = 0.2f;
    public string playerName;
    public GameObject[] iaBonusList;
    public GameObject iaBestTarget;
    public float iaSpeed;
    public float iaClosestDistance;
    public float iaTimeAction;
    public float iaTimeActionMax = 4.5f;
    public float animationPoint = 0;
    public float animationPointMax = 1.3f;
    public GameObject particleSystemExplosion;
    public AudioSource eatSound;
    public AudioSource berkSound;

    // Use this for initialization
    void Start()
    {
        eatSound = GameObject.Find("Eat").GetComponent<AudioSource>();
        berkSound = GameObject.Find("Berk").GetComponent<AudioSource>();
        // DEBUG, A ENLEVER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /*Environnement.P1 = new Controls();
        Environnement.P1.Up = KeyCode.W;
        Environnement.P1.Down = KeyCode.S;
        Environnement.P1.Left = KeyCode.A;
        Environnement.P1.Right = KeyCode.D;
        Environnement.P1.Shoot = KeyCode.Space;
        Environnement.P2 = new Controls();
        Environnement.P2.Up = KeyCode.UpArrow;
        Environnement.P2.Down = KeyCode.DownArrow;
        Environnement.P2.Left = KeyCode.LeftArrow;
        Environnement.P2.Right = KeyCode.RightArrow;
        Environnement.P2.Shoot = KeyCode.KeypadEnter;*/
        ////////////////////////////////////////////////

        controls = new Controls();
        if (playerName == "one")
            controls = Environnement.P1;
        else
            controls = Environnement.P2;
        posX = this.transform.localPosition.x;
        posY = this.transform.localPosition.y;
        iaTimeAction = iaTimeActionMax;
    }

    // Update is called once per frame
    void Update()
    {
        movements();
        if (Environnement.pointsPlayerOne < 0)
            Environnement.pointsPlayerOne = 0;
        if (Environnement.pointsPlayerTwo < 0)
            Environnement.pointsPlayerTwo = 0;
        iaTimeAction += Time.deltaTime;
        animationPoint -= Time.deltaTime;
        if (this.playerName == "one" && animationPoint > 0)
        {
            GameObject.Find("Player1Renderer").GetComponent<Animator>().enabled = true;
            GameObject.Find("Player1Renderer").GetComponent<Animator>().Play("Point");
        }
        if (this.playerName == "two" && animationPoint > 0)
        {
            GameObject.Find("Player2Renderer").GetComponent<Animator>().enabled = true;
            GameObject.Find("Player2Renderer").GetComponent<Animator>().Play("Point2");
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains("Bonus"))
        {
            Environnement.bonus--;
            if (playerName == "one")
            {
                eatSound.panStereo = -1;
                eatSound.Play();
                Environnement.playerOneGainedPoints = true;
                Environnement.pointsPlayerOne += 100;
            }
            else
            {
                eatSound.panStereo = 1;
                eatSound.Play();
                Environnement.playerTwoGainedPoints = true;
                Environnement.pointsPlayerTwo += 100;
            }
            GameObject tmpParticle = Instantiate(particleSystemExplosion, 
                col.transform.position, 
                Quaternion.identity);
            tmpParticle.GetComponent<ParticleSystem>().Play();
            Destroy(col.gameObject);
            animationPoint = animationPointMax;

        }
        else if (col.gameObject.name.Contains("Malus"))
        {
            berkSound.Play();
            Environnement.malus--;
            if (playerName == "one")
            {
                berkSound.panStereo = -1;
                berkSound.Play();
                Environnement.pointsPlayerOne -= 75;
            }
            else
            {
                berkSound.panStereo = 1;
                berkSound.Play();
                Environnement.pointsPlayerTwo -= 75;
            }
            Destroy(col.gameObject);
        }
    }

    public void movements()
    {
        float oldY = posY;
        float oldX = posX;

        if (Environnement.pause == false)
        {
            if (playerName == "two" && Environnement.ia == true) // IA STUFF
            {
                if (Environnement.difficulty == "Easy")
                    iaSpeed = 0.05f;
                else if (Environnement.difficulty == "Medium")
                    iaSpeed = 0.08f;
                else if (Environnement.difficulty == "Hard")
                    iaSpeed = 0.12f;

                iaBonusList = GameObject.FindGameObjectsWithTag("Bonus");
                /////// inspiré de la réponse de l'utilisateur edwarddrowe sur ce lien https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
                if (iaBonusList != null)
                {
                    iaBestTarget = null;
                    iaClosestDistance = Mathf.Infinity;
                    foreach (GameObject bonus in iaBonusList)
                    {
                        Vector3 directionToTarget = bonus.transform.localPosition - this.transform.localPosition;
                        float distanceToTarget = directionToTarget.sqrMagnitude;
                        if (distanceToTarget < iaClosestDistance)
                        {
                            iaClosestDistance = distanceToTarget;
                            iaBestTarget = bonus;
                        }
                    }
                }
                /////////

                // Check if IA must active power or not

                string ennemy = "";
                if (playerName == "one")
                    ennemy = "Player2";
                else
                    ennemy = "Player1";
                GameObject ennemyObject = GameObject.FindGameObjectWithTag(ennemy);
                if (ennemyObject != null && iaBestTarget != null)
                {
                    //Debug.Log((this.transform.localPosition - ennemyObject.transform.localPosition).sqrMagnitude);
                    if ((iaBestTarget.transform.localPosition - ennemyObject.transform.localPosition).sqrMagnitude > iaClosestDistance
                        && ((playerName == "one") ? (Environnement.powerPlayerOne) : (Environnement.powerPlayerTwo)) > 0
                        && (this.transform.localPosition - ennemyObject.transform.localPosition).sqrMagnitude > 30
                        && iaTimeAction > iaTimeActionMax)
                    {
                        // Ennemy is less away than player, so we are going to remap
                        iaTimeAction = 0;
                        usePowerPlayer();
                    }
                    else if (iaBestTarget != null)
                        transform.position = Vector3.MoveTowards(transform.localPosition, iaBestTarget.transform.localPosition, iaSpeed);
                }
            }
            else // Real player
            {
                // Move
                // Debug

                // Controller
                if (Environnement.hasTheController == playerName)
                {
                    //Debug.Log("V = " + Input.GetAxis("Vertical") + " H = " + Input.GetAxis("Horizontal"));
                    if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                    {
                        if (Environnement.controllerInverted == false)
                        {
                            posX += Input.GetAxis("Horizontal") / 10;
                            posY += Input.GetAxis("Vertical") / 10;
                        }
                        else
                        {
                            posX += (Input.GetAxis("Horizontal") * -1) / 10;
                            posY += (Input.GetAxis("Vertical") * -1) / 10;
                        }
                        if (animationPoint <= 0 && playerName == "one")
                            GameObject.Find("Player1Renderer").GetComponent<Animator>().Play("Walk");
                        if (animationPoint <= 0 && playerName == "two")
                            GameObject.Find("Player2Renderer").GetComponent<Animator>().Play("Walk2");
                    }
                }
                else // Claver
                {

                    if (Input.GetKey(controls.Up))
                        posY += speed;
                    if (Input.GetKey(controls.Down))
                        posY -= speed;
                    if (Input.GetKey(controls.Left))
                        posX -= speed;
                    if (Input.GetKey(controls.Right))
                        posX += speed;
                    if (Input.GetKeyDown(controls.Shoot))
                        usePowerPlayer();
                }
                //LevelLimits
                if (posX < -9.5f)
                    posX = -9.5f;
                if (posX > 9.5f)
                    posX = 9.5f;
                if (posY < -4)
                    posY = -4;
                if (posY > 6)
                    posY = 6;
                this.transform.position = new Vector3(posX, posY, 0);
            }
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if ((Mathf.Round(oldX * 100f) / 100f != (Mathf.Round(posX * 100f) / 100f) ||
                (Mathf.Round(oldY * 100f) / 100f != (Mathf.Round(posY * 100f) / 100f))) &&
                animationPoint <= 0)
            {
                if (playerName == "one")
                {
                    GameObject.Find("Player1Renderer").GetComponent<Animator>().enabled = true;
                    GameObject.Find("Player1Renderer").GetComponent<Animator>().Play("Walk");
                }
                else if (playerName == "two")
                {
                    GameObject.Find("Player2Renderer").GetComponent<Animator>().enabled = true;
                    GameObject.Find("Player2Renderer").GetComponent<Animator>().Play("Walk2");
                }
            }
            else
            {
                if (playerName == "one")
                    GameObject.Find("Player1Renderer").GetComponent<Animator>().enabled = false;
                if (playerName == "two")
                    GameObject.Find("Player2Renderer").GetComponent<Animator>().enabled = false;
            }
        }
        else
        {
            this.transform.position = new Vector3(posX, posY, 0);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (animationPoint <= 0 && playerName == "one")
                GameObject.Find("Player1Renderer").GetComponent<Animator>().enabled = false;
            if (animationPoint <= 0 && playerName == "two")
                GameObject.Find("Player2Renderer").GetComponent<Animator>().enabled = false;
        }
    }

    public void usePowerPlayer()
    {
        bool allow = false;
        if (playerName == "one" && Environnement.powerPlayerOne > 0)
        {
            allow = true;
            Environnement.powerPlayerOne--;
        }
        if (playerName == "two" && Environnement.powerPlayerTwo > 0)
        {
            allow = true;
            Environnement.powerPlayerTwo--;
        }
        if (allow)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Bonus");
            foreach (GameObject obj in objects)
                Destroy(obj);
            objects = GameObject.FindGameObjectsWithTag("Malus");
            foreach (GameObject obj in objects)
                Destroy(obj);
            Environnement.bonus = 0;
            Environnement.malus = 0;
        }
    }
}
