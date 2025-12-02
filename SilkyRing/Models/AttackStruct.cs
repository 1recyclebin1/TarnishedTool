// 

namespace SilkyRing.Models;

public class AttackStruct(
    long myId,
    PhysicalAttackType physicalAttackType,
    float poiseDamage,
    int totalDamage,
    int fireDamage,
    int magicDamage,
    int lightningDamage,
    int holyDamage,
    float rawFireDamage,
    float rawPhysicalDamage,
    float rawMagicDamage,
    float rawLightningDamage,
    float rawHolyDamage,
    int enemyId)
{
    public long MyId { get; set; } = myId;
    public float RawPhysicalDamage { get; set; } = rawPhysicalDamage;
    public float RawFireDamage { get; set; } = rawFireDamage;
    public float RawMagicDamage { get; set; } = rawMagicDamage;
    public float RawLightningDamage { get; set; } = rawLightningDamage;
    public float RawHolyDamage { get; set; } = rawHolyDamage;
    public float RawPoiseDamage { get; set; } = poiseDamage;
    public PhysicalAttackType PhysicalAttackType { get; set; } = physicalAttackType;
    public int TotalDamage { get; set; } = totalDamage;
    public int FireDamage { get; set; } = fireDamage;
    public int MagicDamage { get; set; } = magicDamage;
    public int LightningDamage { get; set; } = lightningDamage;
    public int HolyDamage { get; set; } = holyDamage;
    public float PoiseDamage { get; set; } = poiseDamage;
    public int EnemyId { get; set; } = enemyId;
}

public enum PhysicalAttackType
{
    Slash = 0,
    Strike = 1,
    Pierce = 2,
}