using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class FlyControls : MonoBehaviour
{
    //PUBLIC
    public float maxthrust = 40.0F; //Максемальная тяга двигателя
    public float maxspeed = 200.0F; //Максемальная скорость
    public float maneuverability = 1.0F; //Манёвренность или упровляемость
    public float racing = 50.0F; //Скорость набора скорости
    public float maxangleX = 30.0F;// Нудевой угол атаки

    //PRIVATE
    private float lforce = 0.0F; //Подёмная сила
    private float clift = 0.0F; //Коэффициент подъемной силы 
    private float thrust = 0.0F; //Тяга двигателя

    private float ax = 0.0F;
    private float ay = 0.0F;
    private float az = 0.0F;
    private float anglesx = 0.0F;
    private float anglesz = 0.0F;


    void Start()
    {
    }

    void FixedUpdate()
    {
        az = 0.0F;

        if (Input.GetButton("Jump")) //Жмём пробел
        {
            if (thrust < maxthrust)
            {
                thrust += racing * Time.deltaTime;//Прибовляем тягу пока она меньше 25
            }
        }
        else
        {
            if (thrust > 0.0F)//Если клавиша не нажата и скорость больше нуля то
            {
                thrust = GetComponent<Rigidbody>().velocity.magnitude * 0.35F;
            }
        }

        if (GetComponent<Rigidbody>().rotation.eulerAngles.x < 180.0F)
        {
            anglesx = -GetComponent<Rigidbody>().rotation.eulerAngles.x;
        }
        else
        {
            anglesx = 360.0F - GetComponent<Rigidbody>().rotation.eulerAngles.x;
        }

        if (GetComponent<Rigidbody>().rotation.eulerAngles.z < 180.0F)
        {
            anglesz = -GetComponent<Rigidbody>().rotation.eulerAngles.z;
        }
        else
        {
            anglesz = 360.0F - GetComponent<Rigidbody>().rotation.eulerAngles.z;
        }

        anglesx = Mathf.Clamp(anglesx, -maxangleX, maxangleX + 5.0F);
        clift = 1.0F - (0.035F * (maxangleX - anglesx));

        if (anglesx > maxangleX - 5.0F && anglesz < 45.0F && anglesz > -45.0F)// а тут код для того, чтобы при сильном угле атаки падала тяга и подёмная сила
        {
            clift = -thrust * 0.15F;
        }
        ax = Input.GetAxisRaw("Vertical") * maneuverability;
        ay = Input.GetAxisRaw("Horizontal") * maneuverability;
        GetComponent<Rigidbody>().angularDrag = 6.0F - maneuverability;
        GetComponent<Rigidbody>().drag = 1.5F - maneuverability;
        if (Input.GetKey("q"))
        {
            az = 3.0F * maneuverability;
        }
        if (Input.GetKey("e"))
        {
            az = -3.0F * maneuverability;
        }

        lforce = (GetComponent<Rigidbody>().velocity.magnitude * 0.175F) + (GetComponent<Rigidbody>().velocity.magnitude * 0.04F * clift);//тут расчёт подъёмной силы
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(ax, ay, az) * Time.deltaTime, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(az * az, lforce, thrust) * Time.deltaTime, ForceMode.VelocityChange);
    }
    void OnGUI()
    {
        GUILayout.Label("    Height: " + transform.position.y.ToString("f2"));
        GUILayout.Label("    Speed: " + GetComponent<Rigidbody>().velocity.magnitude.ToString("f2"));
        GUILayout.Label("    Lifting force: " + lforce.ToString("f2"));
        GUILayout.Label("    Thrust: " + thrust.ToString("f2"));
        GUILayout.Label("    AnglesX: " + anglesx.ToString("f2"));
        GUILayout.Label("    Temp: " + anglesz.ToString("f2"));
        GUILayout.Label("    Clift: " + clift.ToString("f2"));
    }
}

/*
var main_Rotor_GameObject     : GameObject;   // gameObject to be animated 
 var tail_Rotor_GameObject     : GameObject;   // gameObject to be animated 

 var max_Rotor_Force      : float = 22241.1081;    // newtons 
 static var max_Rotor_Velocity      : float = 7200;   // degrees per second 
 private var rotor_Velocity       : float = 0.0;   // value between 0 and 1 
 private var rotor_Rotation       : float = 0.0;    // degrees... used for animating rotors 

 var max_tail_Rotor_Force     : float = 15000.0;   // newtons 
 var max_Tail_Rotor_Velocity    : float = 2200.0;   // degrees per second 
 private var tail_Rotor_Velocity     : float = 0.0;    // value between 0 and 1 
 private var tail_Rotor_Rotation     : float = 0.0;    // degrees... used for animating rotors 
   
 var forward_Rotor_Torque_Multiplier  : float = 0.5;   // multiplier for control input 
 var sideways_Rotor_Torque_Multiplier    : float = 0.5;   // multiplier for control input 

 static var main_Rotor_Active     : boolean = true;  // boolean for determining if a prop is active 
 static var tail_Rotor_Active     : boolean = true;  // boolean for determining if a prop is active 

 // Forces are applied in a fixed update function so that they are consistent no matter what the frame rate of the game is. This is  
 // important to keeping the helicopter stable in the air. If the forces were applied at an inconsistent rate, the helicopter would behave  
 // irregularly. 
 function FixedUpdate()
{

    // First we must compute the torque values that are applied to the helicopter by the propellers. The "Control Torque" is used to simulate 
    // the variable angle of the blades on a helicopter and the "Torque Value" is the final sum of the torque from the engine attached to the  
    // main rotor, and the torque applied by the tail rotor. 
    var torqueValue : Vector3;
    var controlTorque : Vector3 = Vector3(Input.GetAxis("Vertical") * forward_Rotor_Torque_Multiplier, 1.0, -Input.GetAxis("Horizontal2") * sideways_Rotor_Torque_Multiplier);

    // Now check if the main rotor is active, if it is, then add it's torque to the "Torque Value", and apply the forces to the body of the  
    // helicopter. 
    if (main_Rotor_Active == true)
    {
        torqueValue += (controlTorque * max_Rotor_Force * rotor_Velocity);

        // Now the force of the prop is applied. The main rotor applies a force direclty related to the maximum force of the prop and the  
        // prop velocity (a value from 0 to 1) 
        rigidbody.AddRelativeForce(Vector3.up * max_Rotor_Force * rotor_Velocity);

        // This is simple code to help stabilize the helicopter. It essentially pulls the body back towards neutral when it is at an angle to 
        // prevent it from tumbling in the air. 
        if (Vector3.Angle(Vector3.up, transform.up) < 80)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotor_Velocity * 3);
        }
    }

    // Now we check to make sure the tail rotor is active, if it is, we add it's force to the "Torque Value" 
    if (tail_Rotor_Active == true)
    {
        torqueValue -= (Vector3.up * max_tail_Rotor_Force * tail_Rotor_Velocity);
    }

    // And finally, apply the torques to the body of the helicopter. 
    rigidbody.AddRelativeTorque(torqueValue);
}

function Update()
{
    // This line simply changes the pitch of the attached audio emitter to match the speed of the main rotor. 
    audio.pitch = rotor_Velocity;

    // Now we animate the rotors, simply by setting their rotation to an increasing value multiplied by the helicopter body's rotation. 
    if (main_Rotor_Active == true)
    {
        main_Rotor_GameObject.transform.rotation = transform.rotation * Quaternion.Euler(0, rotor_Rotation, 0);
    }
    if (tail_Rotor_Active == true)
    {
        tail_Rotor_GameObject.transform.rotation = transform.rotation * Quaternion.Euler(tail_Rotor_Rotation, 0, 0);
    }

    // this just increases the rotation value for the animation of the rotors. 
    rotor_Rotation += max_Rotor_Velocity * rotor_Velocity * Time.deltaTime;
    tail_Rotor_Rotation += max_Tail_Rotor_Velocity * rotor_Velocity * Time.deltaTime;

    // here we find the velocity required to keep the helicopter level. With the rotors at this speed, all forces on the helicopter cancel  
    // each other out and it should hover as-is. 
    var hover_Rotor_Velocity = (rigidbody.mass * Mathf.Abs(Physics.gravity.y) / max_Rotor_Force);
    var hover_Tail_Rotor_Velocity = (max_Rotor_Force * rotor_Velocity) / max_tail_Rotor_Force;

    // Now check if the player is applying any throttle control input, if they are, then increase or decrease the prop velocity, otherwise,  
    // slowly LERP the rotor speed to the neutral speed. The tail rotor velocity is set to the neutral speed plus the player horizontal input.  
    // Because the torque applied by the main rotor is directly proportional to the velocity of the main rotor and the velocity of the tail rotor, 
    // so when the tail rotor velocity decreases, the body of the helicopter rotates. 
    if (Input.GetAxis("Vertical2") != 0.0)
    {
        rotor_Velocity += Input.GetAxis("Vertical2") * 0.008;
    }
    else
    {
        rotor_Velocity = Mathf.Lerp(rotor_Velocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime * 30);
    }
    tail_Rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Horizontal");

    // now we set velocity limits. The multiplier for rotor velocity is fixed to a range between 0 and 1. You can limit the tail rotor velocity  
    // too, but this makes it more difficult to balance the helicopter variables so that the helicopter will fly well. 
    if (rotor_Velocity > 1.0)
    {
        rotor_Velocity = 1.0;
    }
    else if (rotor_Velocity < 0.0)
    {
        rotor_Velocity = 0.0;
    }
}
*/


