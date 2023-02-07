using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;
public class shoot : NetworkBehaviour
{
    [ReadOnly]public string tankColor;
    public tankControlOverhaul tankControlOverhaul;
    public float intialBulletSpeed = 5000f;
    [Range(0,100)]public int secretChancePercentage = 1;
    public GameObject bulletPrefab;
    [Range(1,2),LabelText("Number of Cannons")]public int numOfCannons = 1;
    
    [Title("Main Cannon")]
    public int trigger;
    public GameObject turretBulletSpawner;
    public ParticleSystem flame;
    public ParticleSystem flash;
    public AudioSource turretReloadAudio;
    public AudioSource secretSound1;
    [Range(0, 5)] public float fireRate = 1f;
    private AudioSource sound1;
    public List<GameObject> dashIndicators;

    [Title("Secondary Cannon")]
    [LabelText("Secondary Trigger")]public int trigger2;
    [LabelText("Secondary Bullet Spawner")]public GameObject hullBulletSpawner;
    [LabelText("Secondary Flame")]public ParticleSystem flame2;
    [LabelText("Secondary Flash")]public ParticleSystem flash2;
    public AudioSource hullReloadAudio;
    public AudioSource secretSound2;
    [LabelText("Secondary Fire Rate"),Range(0, 5)] public float fireRate2 = 1f;
    private AudioSource sound2;
    
    
    private float nextFire = 0;
    private float nextFire2 = 0;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        tankColor = tankControlOverhaul.tankColor.name;
        sound1 = turretBulletSpawner.GetComponent<AudioSource>();
        sound2 = hullBulletSpawner.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(distance > 0)
        {
            distance = nextFire - Time.time;
        }
        if (Input.GetMouseButtonDown(trigger) && Time.time > nextFire)
        {
            
            for(int i = 0; i < dashIndicators.Count; i++)
            {
                dashIndicators[i].SetActive(false);
            }
            nextFire = Time.time + fireRate;
            distance = nextFire - Time.time;
            CmdshootMain();
            if (isServer) 
            {
                RpcMainMuzzelFlash();
            }
            else
            {
                CmdMainMuzzelFlash();
            }
        }
        if(Input.GetMouseButtonDown(trigger2)&&Time.time > nextFire2)
        {
            nextFire2 = Time.time + fireRate2;
            
            CmdshootSecondary();
            if (isServer)
            {
                RpcSecMuzzelFlash();
            }
            else
            {
                CmdSecMuzzelFlash();
            }
        }
        
        if (distance <= fireRate * .75) 
        { 
            dashIndicators[0].SetActive(true);
        }
        if (distance <= fireRate * .50)
        {
            dashIndicators[1].SetActive(true);
        }
        if (distance <= fireRate * .25)
        {
            dashIndicators[2].SetActive(true);
        }
        if (distance <= 0)
        {
            dashIndicators[3].SetActive(true);
        }
        
    }
    [Command]
    void CmdMainMuzzelFlash()
    {
        RpcMainMuzzelFlash();
    }
    [ClientRpc]
    void RpcMainMuzzelFlash()
    {
        flash.Play();
        flame.Play();
    }
    [Command]
    void CmdSecMuzzelFlash()
    {
        RpcSecMuzzelFlash();
    }
    [ClientRpc]
    void RpcSecMuzzelFlash()
    {
        flash2.Play();
        flame2.Play();
    }

    [Command]
    void CmdshootMain()
    { 
        Vector3 spawn = new Vector3(turretBulletSpawner.transform.position.x, turretBulletSpawner.transform.position.y, turretBulletSpawner.transform.position.z);
        int i = Random.Range(0, secretChancePercentage);
        if (i == 1 && i != 0)
        {
            secretSound1.Play();
        }
        else
        {
            sound1.Play();
        }
        turretReloadAudio.Play();
        GameObject cBullet = Instantiate(bulletPrefab, spawn, Quaternion.identity);
        cBullet.tag = tankColor;
        bulletPrefab.tag = tankColor;
        cBullet.transform.rotation = turretBulletSpawner.transform.rotation;
        Rigidbody rig = cBullet.GetComponent<Rigidbody>();
        rig.AddForce(cBullet.transform.forward * intialBulletSpeed);
        NetworkServer.Spawn(cBullet);

    }
    [Command]
    void CmdshootSecondary()
    {
        Vector3 spawn = hullBulletSpawner.transform.position;
        int i = Random.Range(0, secretChancePercentage);
        if (i == 1 && i != 0)
        {
            secretSound2.Play();
        }
        else
        {
            sound2.Play();
        }
        hullReloadAudio.Play();
        GameObject cBullet = Instantiate(bulletPrefab, spawn, Quaternion.identity);
        cBullet.tag = tankColor;
        bulletPrefab.tag = tankColor;
        cBullet.transform.rotation = hullBulletSpawner.transform.rotation;
        Rigidbody rig = cBullet.GetComponent<Rigidbody>();
        rig.AddForce(cBullet.transform.forward * intialBulletSpeed);
        NetworkServer.Spawn(cBullet);
    }
}
