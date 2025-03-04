using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    abstract public float getCooldown();

    abstract public Sprite getSprite();

    abstract public void cast(Transform playerTransform, Vector2 direction);

    abstract public void updateCooldown(float cdr);

    abstract public void endAbility();
}
