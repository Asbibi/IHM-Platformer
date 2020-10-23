using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    SpriteRenderer sprite = null;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(DieAfterDelay());
    }

    IEnumerator DieAfterDelay(){
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = sprite.color.a - (Time.deltaTime / duration);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
    }
}
