using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public JengaPlayer player;
    public Gun gun;
    public Rigidbody rb;
    public float speed = 5;
    public bool GettingInput = true;
    public void Move(Vector3 moveDirection)
    {
        if (moveDirection.magnitude > 0.1f)
        {
            if (rb.velocity.magnitude < speed)
            {
                moveDirection = moveDirection * speed;
                rb.AddForce(moveDirection, ForceMode.Force);
            }
        }
    }
    public void Look(Vector2 LookInput)
    {
        rb.angularVelocity = Vector3.zero;
        if (LookInput.magnitude > 0.1f)
        {
            float heading = Mathf.Atan2(LookInput.y, -LookInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, heading - 90, 0f);
        }
    }
    private void FixedUpdate()
    {
        if (GettingInput)
        {
            Vector3 moveInput = Vector3.zero;
            Vector2 lookInput = Vector3.zero;
            bool triggerDown = false;
            switch (player.input)
            {
                case JengaPlayer.InputType.keyboard:
                    moveInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                    lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    if (Input.GetButton("Fire1")) triggerDown = true;
                    break;
                case JengaPlayer.InputType.controller1:
                    break;
                case JengaPlayer.InputType.controller2:
                    break;
                case JengaPlayer.InputType.controller3:
                    break;
                case JengaPlayer.InputType.controller4:
                    break;
            }
            Move(moveInput);
            Look(lookInput);
            if (triggerDown && gun != null) gun.Fire(player);
        }
    }
    private void Awake()
    {
        player.OnDeath += DisableInput;
        player.OnSpawn += EnableInput;
    }
    public void EnableInput()
    {
        GettingInput = true;
    }
    public void DisableInput(ExampleGameTypeIntegration.DeathInfo deathInfo)
    {
        GettingInput = false;
    }
}
