using System;
using UnityEngine;
using UnityEngine.U2D;

public class IcebergLogic : MonoBehaviour
{
    [SerializeField] IcebergDataSO data;
    [SerializeField] ListSmallBergsSO listSmallIcebergs;

    ParticleSystem myParticles;
    SpriteShapeRenderer mySprite;
    ushort HP;

    void Start()
    {
        myParticles = GetComponent<ParticleSystem>();
        mySprite = GetComponent<SpriteShapeRenderer>();

        try
        {
            HP = data.GetHitsCountBeforeDestruction();
        }
        catch (NullReferenceException)
        {
            // FOR TESTING
            HP = 1000;
        }
    }

    void FixedUpdate()
    {
        if (!CanBeDamaged())
        {
            float alphaChannel;
            if (data.GetDamageAmount() > .3)
            {
                alphaChannel = Mathf.Lerp(mySprite.color.a, 0f, 0.05f);
            }
            else
            {
                alphaChannel = Mathf.Lerp(mySprite.color.a, 0f, 0.1f);
            }

            var color = mySprite.color;
            mySprite.color = new Color(color.r, color.g, color.b, alphaChannel);

            // destroy if no particle playing anymore
            if (!myParticles.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if it is with the boat, call the boat "GetDamage" method
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boat"))
        {
            collision.gameObject.GetComponent<BoatLogic>().GetDamage(data.GetDamageAmount(), collision.GetContact(0).point);
        }
    }

    public void GetDamage(ushort damageDegree)
    {
        if (CanBeDamaged())
        {
            HP -= damageDegree;

            if (HP == 0)
            {
                LaunchDestructionProcess();
            }
        }
    }

    void LaunchDestructionProcess()
    {
        // play particles
        myParticles.Play();

        // deactivate collider
        GetComponent<PolygonCollider2D>().isTrigger = true;
        GetComponent<PolygonCollider2D>().excludeLayers = LayerMask.GetMask("Missiles");

        // generate small bergs if we have to
        if (data.GetIcebergsCreationCount() > 0)
        {
            GenerateRandomIcebergs();
        }
    }

    void GenerateRandomIcebergs()
    {
        for (int i = 0; i < 1; i++)
        {
            // create an iceberg
            var iceberg = Instantiate(listSmallIcebergs.GetRandomSmallIceberg());
            iceberg.transform.position = transform.position;
        }
    }

    bool CanBeDamaged()
    {
        return HP > 0;
    }
}
