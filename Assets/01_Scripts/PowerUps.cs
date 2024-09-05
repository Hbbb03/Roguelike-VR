using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum PowerUpType
    {
        Health,
        Damage,
        Speed
    }

    public PowerUpType powerUpType;
    public float value = 1f; // Valor del power-up (por ejemplo, +10 de vida, +10% de velocidad, etc.)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                ApplyPowerUp(player);
                Destroy(gameObject); // Destruir el power-up después de ser recogido
            }
        }
    }

    void ApplyPowerUp(Player player)
    {
        switch (powerUpType)
        {
            case PowerUpType.Health:
                player.life += value;
                if (player.life > player.maxLife)
                {
                    player.life = player.maxLife; // No exceder la vida máxima
                }
                break;

            case PowerUpType.Damage:
                player.bulletSpeed += value;
                break;

            case PowerUpType.Speed:
                player.moveSpeed += value;
                break;
        }
    }
}
