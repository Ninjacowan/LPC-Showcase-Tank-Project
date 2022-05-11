using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class shoot : NetworkBehaviour
{

    public GameObject bulletSpawner;
    public GameObject bullet_y;
    public GameObject Bullet;
    public AudioSource cannon;
    public float intialBulletSpeed = 5000f;
    public float fireRate = 1f;
    private float nextFire = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Cmdshoot();

        }
    }

    [Command]
    void Cmdshoot()
    {
        
        Vector3 spawn = new Vector3(bulletSpawner.transform.position.x, bullet_y.transform.position.y, bulletSpawner.transform.position.z);
        cannon.Play();
        GameObject cBullet = Instantiate(Bullet, spawn, Quaternion.identity);
        cBullet.transform.rotation = bulletSpawner.transform.rotation;
        Rigidbody rig = cBullet.GetComponent<Rigidbody>();
        rig.AddForce(cBullet.transform.forward * intialBulletSpeed);
        NetworkServer.Spawn(cBullet);

    }
}
