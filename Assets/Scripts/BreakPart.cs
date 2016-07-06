using UnityEngine;
using System.Collections;

public class BreakPart : MonoBehaviour {

    void OnTriggerEnter(Collider other) // если тригер пересечёт какойнибудь коллайдер
    {
        if (other.gameObject.tag == "Tube") // если у этого коллайдера будет тэг Tube
        {
            if (!GetComponent<Rigidbody>()) // если у обьекта скрипта нет ригибоди (чтобы запустить лишь один раз)
            {
                StartCoroutine(destroy(5f)); // запускаем корутин с параметром 5
            }
        }
    }

    IEnumerator destroy(float sec)
    {
        transform.SetParent(null); // обнуляем параметр родителя
        gameObject.AddComponent<Rigidbody>(); // добавляем ригибоди
        GetComponent<Rigidbody>().useGravity = false; // убираем влияние гравитации
        yield return new WaitForSeconds(sec); // ждем 5 секунд
        Destroy(gameObject); // уничтожаем обьект
    }
}

