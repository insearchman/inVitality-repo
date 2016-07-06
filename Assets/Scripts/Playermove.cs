using UnityEngine;
using System.Collections;

public class Playermove : MonoBehaviour
{
    public Rigidbody rbp; // Переменная ФизТела
    public Vector3 oPos; // Позиция планера кадр назад
    public Vector3 sac_tu; // Стартовое ускорение
    public Vector3 thrust; // Стандартное ускорение
    public Vector3 speed; // Скорость планера
    public float tiltspeed; // Скорость крена
    public float cosmodromposition; // Крайняя позиция космодрома
    float wm; // параметр отражающий ушудшение управляемости в зависимости от режима крыльев

    Vector3 tt; // параметр отражающий ушудшение разгона в зависимости от режима хо
    Vector3 wt; // параметр отражающий ушудшение разгона в зависимости от режима крыльев

    void Start ()
    {
        rbp = GetComponent<Rigidbody>(); // присваиваем переменной ФизТело обьекта на котором находится скрипт
        oPos = transform.position; // изначальная позиция планера
        sac_tu = new Vector3(300f, 0f, 0f); // сила стартового ускорения
        thrust = new Vector3(20f, 0f, 0f); // обычное ускорение
        tiltspeed = 10f;
        cosmodromposition = 500;
    }
	
	void Update ()
    {
        tt = GetComponentInChildren<WingsTailAnim>().ac_tu; // берем вектор замедления из режима хо
        wt = GetComponentInChildren<WingsTailAnim>().w_thrust; // берем вектор замедления из режима крыльев
        wm = GetComponentInChildren<WingsTailAnim>().wm; // берем параметр ухудшения управляемости из режима крыльев

        // уменьшение замедления при торможении
        if (speed.x < 400)
        {
            tt.x = tt.x / 2.6f;
            if (speed.x < 350)
            {
                tt.x = tt.x / 3.2f;
                if (speed.x < 300)
                {
                    tt.x = tt.x / 3.8f;
                    if (speed.x < 250)
                    {
                        tt.x = tt.x / 4.4f;
                        if (speed.x < 240)
                        {
                            tt.x = tt.x / 5f;
                            if (speed.x < 230)
                            {
                                tt.x = tt.x / 5.6f;
                                if (speed.x < 220)
                                {
                                    tt.x = tt.x / 6.2f;
                                    if (speed.x < 210)
                                    {
                                        tt.x = tt.x / 6.6f;
                                        if (speed.x < 200) speed.x = 200;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        // Разгон
        if (transform.position.x < cosmodromposition) rbp.AddForce(sac_tu, ForceMode.Acceleration); // Стартовый разгон планера
        else rbp.AddForce(thrust-tt-wt, ForceMode.Acceleration); // Обычный разгон

        // Управление кренами планера
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal!=0 || vertical!=0)
        {
            rbp.position += new Vector3(0f, vertical * tiltspeed * wm * Time.deltaTime, horizontal * tiltspeed * wm * Time.deltaTime);
        }

        // Тестовая функция для отображения наклона планера в зависимости от WASD
        if (Input.GetKey(KeyCode.H))
        {
            rbp.transform.Rotate(new Vector3(0f,0f,1f)*Time.deltaTime*10,Space.Self);
        }
    }

    void FixedUpdate()
    {
        // спидометр
        speed = -(oPos - transform.position) / Time.deltaTime;
        oPos = transform.position;
    }

    void OnGUI()
    {
        // вывод спидометра на экран
        GUI.Box(new Rect(10, 10, 100, 25), "Speed = " + Mathf.RoundToInt(speed.x));
        // Mathf.RoundToInt(GetComponent<Rigidbody>().velocity.magnitude) // аналогичный вариант
    }
}