﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public float velMax = 5;
    public float forcaPulo = 150;
    public float raioDosPes = 0.5f;
    public Transform posicaoDosPes;
    public float intervaloDisparo = 0.25f;

    public GameObject tiro;
    public Transform posicaoArma;

    private bool estaPulando = false;
    private bool estaAndando = false;
    private bool estaNoChao = false;

    private float movHorizontal;
    private float timerDisparo = 0;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animacao;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animacao = GetComponent<Animator>();
    }

    private void Update()
    {
        movHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && estaNoChao == true)
        {
            estaPulando = true;
        }        

        animacao.SetBool("Andando", estaAndando);
        animacao.SetBool("Pulando", !estaNoChao);

        timerDisparo -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && timerDisparo <= 0)
        {
            timerDisparo = intervaloDisparo;
            GameObject cloneTiro = Instantiate(tiro, posicaoArma.position, posicaoArma.rotation);

            if (sprite.flipX)
            {
                cloneTiro.transform.eulerAngles = new Vector3(0, 0, 180);
            }

            animacao.SetTrigger("Atirando");
        }
    }

    private void FixedUpdate()
    {
        estaNoChao = Physics2D.OverlapCircle(posicaoDosPes.position, raioDosPes, LayerMask.GetMask("Chao"));        
        Mover();
        estaAndando = rb.velocity.x != Vector2.zero.x;
        Pular();        
    }
      
    private void Mover()
    {
        rb.velocity = new Vector2(movHorizontal * velMax, rb.velocity.y);
        InverteSprite();
    }

    private void InverteSprite()
    {
        if ((movHorizontal > 0 && sprite.flipX == true) || movHorizontal < 0 && sprite.flipX == false)
        {
            sprite.flipX = !sprite.flipX;

            posicaoArma.localPosition = new Vector2(posicaoArma.localPosition.x *-1,
                                                    posicaoArma.localPosition.y);
        }
    }

    private void Pular()
    {
        if (estaPulando == true)
        {
            rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
            estaPulando = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoDosPes.position, raioDosPes);
    }
}