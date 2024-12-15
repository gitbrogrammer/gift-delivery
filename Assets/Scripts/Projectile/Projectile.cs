//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
//#endregion


public class Projectile : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector2 initialVelocity;
    [SerializeField] private bool isStartPhysics = false;
    [SerializeField] private Vector3 offsetForward;
    //#endregion

    //#region public fields and properties
    public Vector2 InitialVelocity { get { return initialVelocity; } set { initialVelocity = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public bool IsStartPhysics { get { return isStartPhysics; } set { isStartPhysics = value; } }
    //#endregion

    //#region private fields and properties
    private Bubble bubble;
    private new Rigidbody2D rigidbody2D;
    private new Collider2D collider2D;

    private Vector2 lastVelocity;

    private bool isStop = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        bubble = GetComponent<Bubble>();
    }

    void Start()
    {
        TogglePhysics(isStartPhysics);
    }

    void FixedUpdate()
    {
        Move();
    }

    //#endregion

    //#region public methods

    public void StartMove()
    {
        Vector3 direction = initialVelocity.normalized;


        rigidbody2D.velocity = direction * speed;

        bubble.RotateArrow(direction * speed);
    }

    public void TogglePhysics(bool isOn)
    {
        rigidbody2D.simulated = isOn;
        collider2D.enabled = isOn;
    }

    public void Stop()
    {
        isStop = true;
        rigidbody2D.simulated = false;
    }

    //#endregion

    //#region private methods

    private void Move()
    {
        lastVelocity = rigidbody2D.velocity;
    }

    //#endregion

    //#region event handlers

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float speed = lastVelocity.magnitude;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision2D.contacts[0].normal);

        rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);


        if (isStop) return;
        bubble.RotateArrow(direction * Mathf.Max(speed, 0f));
    }

    //#endregion
}
