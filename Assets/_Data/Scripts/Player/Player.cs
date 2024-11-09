using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerRenderer;

    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;
    private Collider2D col2D;

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
        col2D = GetComponent<Collider2D>();

        CharacterSelectionManager.onCharacterSelected += CharacterSelectedCallback;
    }

    private void OnDestroy()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallback;
    }

    public void TakeDamage(int damage) => playerHealth.TakeDamage(damage);

    public Vector3 GetCenter()
    {
        return (Vector2)transform.position + col2D.offset;
    }

    public bool HasLevelUp() => playerLevel.HasLevelUp();

    private void CharacterSelectedCallback(CharacterDataSO data)
    {
        playerRenderer.sprite = data.Sprite;
    }

}
