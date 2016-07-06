using UnityEngine;
using System.Collections;

public class WingsTailAnim : MonoBehaviour
{
    GameObject go_wr1; // 1 правое крыло
    GameObject go_wr2; // 2 правое крыло
    GameObject go_wl1; // 1 левое крыло
    GameObject go_wl2; // 2 левое крыло
    GameObject go_tu; // Верхнее хвостовое оперение
    GameObject go_td; // Нижнее хвостовое оперение

    float am1w1; // Угол 1 крыльев для перехода в 1 режим
    float am1w2; // Угол 2 крыльев для перехода в 1 режим
    float am2w1; // Угол 1 крыльев для перехода во 2 режим
    float am2w2; // Угол 2 крыльев для перехода во 2 режим
    float am1t; // Угол х.о. для перехода в 1 режим

    float tm0r1; // Режим 0. Положение 1 правого
    float tm0l1; // Режим 0. Положение 1 левого
    float tm0r2; // Режим 0. Положение 2 правого
    float tm0l2; // Режим 0. Положение 2 левого
    float tm1r1; // Режим 1. Положение 1 правого
    float tm1l1; // Режим 1. Положение 1 левого
    float tm1r2; // Режим 1. Положение 2 правого
    float tm1l2; // Режим 1. Положение 2 левого
    float tm2r1; // Режим 2. Положение 1 правого
    float tm2l1; // Режим 2. Положение 1 левого
    float tm2r2; // Режим 2. Положение 2 правого
    float tm2l2; // Режим 2. Положение 2 левого
    float tm1tu; // Режим 0. Положение верхнего х.о.
    float tm1td; // Режим 0. Положение нижнего х.о.
    float tm2tu; // Режим 1. Положение верхнего х.о.
    float tm2td; // Режим 1. Положение нижнего х.о.

    int WingsMode = 1; // Режим крыльев: 1-сложенные, 2-полусложенные, 3-раскрытые
    int oWingsMode; // Предыдущий режим крыльев
    int TailMode = 1; // Режим х.о.: 1-сложенны, 2-разложенны (тормоз)

    bool rotateWings_rw1 = false; // Вкл/выкл поворота 1 крыльев
    bool rotateWings_rw2 = false; // Вкл/выкл поворота 2 крыльев
    bool rotateTailUp = false; // Вкл/выкл поворота верхнего х.о.
    bool rotateTailDown = false; // Вкл/выкл поворота нижнего х.о.

    float s_rw1; // скорость поворота 1 крыльев
    float s_rw2; // скорость поворота 2 крыльев
    float s_rt; // скорость поворота х.о.

    float v_rw1; // В какую сторону будем вращать крылья
    float v_rw2; // В какую сторону будем вращать крылья
    float vr_tu; // Вектор вращения вхо
    float vr_td; // Вектор вращения нхо

    public Vector3 ac_tu; // модификатор ускорения х.о.
    public Vector3 ac_td; // модификатор ускорения х.о.
    public Vector3 w_thrust; // модификатор ускорения положения крыльев
    public float wm; // параметр отражающий ушудшение управляемости в зависимости от режима крыльев

    
    public float t_tu; // сила модификатора ac_tu влияемая вхо
    public float t_td; // сила модификатора ac_td влияемая нхо
    float w_t;

    Flyer fi = new Flyer();


    void Start ()
    {

        fi.flyer = transform.Find("inFly").gameObject;

        // Прикрепляем крылья и х.о. к переменным
        go_wr1 = transform.FindChild("WingR1").gameObject;
        go_wr2 = go_wr1.transform.FindChild("WingR2").gameObject;
        go_wl1 = transform.FindChild("WingL1").gameObject;
        go_wl2 = go_wl1.transform.FindChild("WingL2").gameObject;
        go_tu = transform.FindChild("TailUp").gameObject;
        go_td = transform.FindChild("TailDown").gameObject;

        // Присваиваем изначальное положение крыльев и х.о.
        tm0r1 = go_wr1.transform.localEulerAngles.y;
        tm0l1 = go_wl1.transform.localEulerAngles.y;
        tm0r2 = go_wr2.transform.localEulerAngles.z;
        tm0l2 = go_wl2.transform.localEulerAngles.z;
        tm1tu = go_tu.transform.localEulerAngles.z;
        tm1td = go_td.transform.localEulerAngles.z;

        // Назначаем углы поворотов для всех режимов
        am1w1 = 0f;
        am1w2 = 40f;
        am2w1 = 65f;
        am2w2 = 60f;
        am1t = 70;

        // Назначаем скорость движения
        s_rw1 = 80f;
        s_rw2 = 80f;
        s_rt = 40f;

        // Расчёт положения х.о. в раскрытом состоянии
        tm2tu = tm1tu - am1t; 
        tm2td = tm1td + am1t;

        // Расчёт положения крыльев в полураскрытом состоянии
        tm1r1 = tm0r1 + am1w1;
        tm1l1 = tm0l1 + am1w1;
        tm1r2 = tm0r2 - am1w2;
        tm1l2 = tm0l2 + am1w2;

        // Расчёт положения крыльев в раскрытом состоянии
        tm2r1 = tm1r1 - am2w1;
        tm2l1 = tm1l1 + am2w1;
        tm2r2 = tm1r2 + am2w2;
        tm2l2 = tm1l2 - am2w2;

        // Исправляем значения переменных, вышедшие из стандартных значений координат (0-360)
        if (tm1r1 > 360) tm1r1 = tm1r1 - 360;
        if (tm1r2 > 360) tm1r2 = tm1r2 - 360;
        if (tm1l1 > 360) tm1l1 = tm1l1 - 360;
        if (tm1l2 > 360) tm1l2 = tm1l2 - 360;
        if (tm1r1 < 0) tm1r1 = 360 - (-tm1r1);
        if (tm1r2 < 0) tm1r2 = 360 - (-tm1r2);
        if (tm1l1 < 0) tm1l1 = 360 - (-tm1l1);
        if (tm1l2 < 0) tm1l2 = 360 - (-tm1l2);
        if (tm2r1 > 360) tm2r1 = tm2r1 - 360;
        if (tm2r2 > 360) tm2r2 = tm2r2 - 360;
        if (tm2l1 > 360) tm2l1 = tm2l1 - 360;
        if (tm2l2 > 360) tm2l2 = tm2l2 - 360;
        if (tm2r1 < 0) tm2r1 = 360 - (-tm2r1);
        if (tm2r2 < 0) tm2r2 = 360 - (-tm2r2);
        if (tm2l1 < 0) tm2l1 = 360 - (-tm2l1);
        if (tm2l2 < 0) tm2l2 = 360 - (-tm2l2);
        if (tm2tu > 360) tm2tu = tm2tu - 360;
        if (tm2td > 360) tm2td = tm2td - 360;
        if (tm2tu < 0) tm2tu = 360 - (-tm2tu);
        if (tm2td < 0) tm2td = 360 - (-tm2td);

        wm = 1;
    }

    void Update()
    {
        // Переключение режимов хо:
        if (rotateTailUp && rotateTailDown)
        {
            // Режим хо 1:
            if(TailMode == 1)
            {
                ac_tu = new Vector3(t_tu, 0f, 0f); // вектор влияния режима вхо на ускорение планера
                // Если вхо есть и (если положение вхо по z больше чем начальное +1 или если положение вхо по z меньше чем начальное -1)
                if (go_tu && (go_tu.transform.localEulerAngles.z > tm1tu + 1 || go_tu.transform.localEulerAngles.z < tm1tu - 1))
                {
                    t_tu -= 20f * Time.deltaTime; // влияние вхо на ускорение планера
                    vr_tu = tm1tu - go_tu.transform.localEulerAngles.z; // вычисляем в какую сторону вращать вхо
                    WingRotate(go_tu, vr_tu, s_rt); // вращаем вхо
                }
// Другими словами: если "положение вхо" не соответсвует "положению вхо в режиме 1", то тогда вращаем его в нужную нам сторону
                else rotateTailUp = false; // выключаем вращение вхо                                      

                // Тоже самое для нхо
                ac_td = new Vector3(t_td, 0f, 0f); // вектор влияния режима хо на ускорение планера
                if (go_td && (go_td.transform.localEulerAngles.z > tm1td + 1 || go_td.transform.localEulerAngles.z < tm1td - 1))
                {
                    t_td -= 20f * Time.deltaTime;
                    vr_td = tm1td - go_td.transform.localEulerAngles.z;
                    WingRotate(go_td, vr_td, s_rt);
                }
                else rotateTailDown = false;
            }

            // Режим хо 2:
            if(TailMode == 2)
            {
                ac_tu = new Vector3(t_tu, 0f, 0f);
                if (go_tu && (go_tu.transform.localEulerAngles.z > tm2tu + 1 || go_tu.transform.localEulerAngles.z < tm2tu - 1))
                {
                    t_tu += 20f * Time.deltaTime;
                    vr_tu = tm2tu - go_tu.transform.localEulerAngles.z;
                    WingRotate(go_tu, vr_tu, s_rt);
                }
                else rotateTailUp = false;
                ac_td = new Vector3(t_td, 0f, 0f); // вектор влияния режима хо на ускорение планера
                if (go_td && (go_td.transform.localEulerAngles.z > tm2td + 1 || go_td.transform.localEulerAngles.z < tm2td - 1))
                {
                    t_td += 20f * Time.deltaTime;
                    vr_td = tm2td - go_td.transform.localEulerAngles.z;
                    WingRotate(go_td, vr_td, s_rt);
                }
                else rotateTailDown = false;
            }
        }

        // Переключение режимов крыльев:
        if (rotateWings_rw2 || rotateWings_rw1)
        {
            // Режим крыльев 1:
            if (WingsMode == 1)
            {
                wm = 1;
                w_thrust = new Vector3(0f, 0f, 0f);
                if (rotateWings_rw2) // Вращаем 2 крылья
                {
                    if (go_wr2.transform.localEulerAngles.z > tm0r2 + 1 || go_wr2.transform.localEulerAngles.z < tm0r2 - 1)
                    {
                        v_rw2 = tm0r2 - go_wr2.transform.localEulerAngles.z;
                        WingRotate(go_wr2, v_rw2, s_rw2);
                        WingRotate(go_wl2, -v_rw2, s_rw2);
                    }
                    else rotateWings_rw2 = false;
                }
                if (rotateWings_rw1) // Вращаем 1 крылья
                {
                    if (go_wr1.transform.localEulerAngles.y > tm1r1 + 1 || go_wr1.transform.localEulerAngles.y < tm1r1 - 1)
                    {
                        v_rw1 = tm1r1 - go_wr1.transform.localEulerAngles.y;
                        WingRotate(go_wr1, v_rw1, s_rw1 / 2);
                        WingRotate(go_wl1, -v_rw1, s_rw1 / 2);
                    }
                    else rotateWings_rw1 = false;
                }
            }

            // Режим крыльев 2:
            if (WingsMode == 2)
            {
                wm = 2;
                w_thrust = new Vector3(6f, 0f, 0f);
                if (oWingsMode == 1)
                {
                    if (rotateWings_rw1) // Вращаем 1 крылья
                    {
                        if (go_wr1.transform.localEulerAngles.y > tm0r1 + 1 || go_wr1.transform.localEulerAngles.y < tm0r1 - 1)
                        {                        
                            v_rw1 = tm0r1 - go_wr1.transform.localEulerAngles.y;
                            WingRotate(go_wr1, v_rw1, s_rw1 / 2);
                            WingRotate(go_wl1, -v_rw1, s_rw1 / 2);
                        }
                        else rotateWings_rw1 = false;
                    }
                    if (rotateWings_rw2)
                    {
                        if (go_wr2.transform.localEulerAngles.z > tm1r2 + 1 || go_wr2.transform.localEulerAngles.z < tm1r2 - 1)
                        {
                            v_rw2 = tm1r2 - go_wr2.transform.localEulerAngles.z;
                            WingRotate(go_wr2, -1, s_rw2);
                            WingRotate(go_wl2, 1, s_rw2);
                        }
                        else rotateWings_rw2 = false;
                    }
               
                }
                if (oWingsMode == 3)
                {
                    if (rotateWings_rw1)
                    {
                        if (go_wr1.transform.localEulerAngles.y > tm1r1 + 1 || go_wr1.transform.localEulerAngles.y < tm1r1 - 1)
                        {
                            v_rw1 = tm1r1 - go_wr1.transform.localEulerAngles.y;
                            WingRotate(go_wr1, 1, s_rw1);
                            WingRotate(go_wl1, -1, s_rw1);
                        }
                        else rotateWings_rw1 = false;
                    }
                    if (rotateWings_rw2)
                    {
                        if (go_wr2.transform.localEulerAngles.z > tm1r2 + 1 || go_wr2.transform.localEulerAngles.z < tm1r2 - 1)
                        {
                            v_rw2 = tm1r2 - go_wr2.transform.localEulerAngles.z;
                            WingRotate(go_wr2, -1, s_rw2);
                            WingRotate(go_wl2, 1, s_rw2);
                        }
                        else rotateWings_rw2 = false;
                    }
                }
            }

            // Режим крыльев 3:
            if (WingsMode == 3)
            {
                wm = 4;
                w_thrust = new Vector3(10f, 0f, 0f);
                if (rotateWings_rw1)
                {
                    if (go_wr1.transform.localEulerAngles.y > tm2r1 + 1 || go_wr1.transform.localEulerAngles.y < tm2r1 - 1)
                    {
                        v_rw1 = tm2r1 - go_wr1.transform.localEulerAngles.y;
                        WingRotate(go_wr1, -1, s_rw1);
                        WingRotate(go_wl1, 1, s_rw1);
                    }
                    else rotateWings_rw1 = false;
                }
                if (rotateWings_rw2)
                {
                    if (go_wr2.transform.localEulerAngles.z > tm2r2 + 1 || go_wr2.transform.localEulerAngles.z < tm2r2 - 1)
                    {
                        v_rw2 = tm2r2 - go_wr2.transform.localEulerAngles.z;
                        WingRotate(go_wr2, 1, s_rw2);
                        WingRotate(go_wl2, -1, s_rw2);
                    }
                    else rotateWings_rw2 = false;
                }
            }
            
        }

        // Переключение режимов хо вверх
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (TailMode != 2)
            {
                TailMode++;
                rotateTailUp = true;
                rotateTailDown = true;
            }
        }
        // Переключение режимов хо вниз
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (TailMode != 1)
            {
                TailMode--;
                rotateTailUp = true;
                rotateTailDown = true;
            }
        }

        // Переключение режимов крыльев вверх
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (WingsMode != 3)
            {
                oWingsMode = WingsMode;
                WingsMode++;
                rotateWings_rw1 = true;
                rotateWings_rw2 = true;
            }
        }
        // Переключение режимов крыльев вниз
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (WingsMode != 1)
            {
                oWingsMode = WingsMode;
                WingsMode--;
                rotateWings_rw1 = true;
                rotateWings_rw2 = true;
            }
        }


        //Debug.Log("ac_tu = " + ac_tu);
        //Debug.Log("w_thrust = " + w_thrust); 
    }

    // Функция вращения крыльев и хо
    void WingRotate(GameObject wing, float anglez, float speed)
    {
        Quaternion target = Quaternion.Euler(wing.transform.rotation.eulerAngles.x, wing.transform.rotation.eulerAngles.y, wing.transform.rotation.eulerAngles.z + anglez);
        wing.transform.rotation = Quaternion.RotateTowards(wing.transform.rotation , target, Time.deltaTime * speed);
    }

    // Выводим на экран режим крыльев
    void OnGUI()
    {
        GUI.Box(new Rect(10, 40, 100, 25), "Wing Mode = " + (WingsMode));
    }
}
