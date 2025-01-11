using HeroCheats.Constants;
using UnityEngine;

namespace HeroCheats.Plugin {
    public static class BonusManager {
        // Note: redundant at the moment, could just be one function with a bool parameter
        public static void ApplyBonuses(Players_FindNearestAndAttack instance) {
            instance.bonus_attackPower += BonusConstants.BONUS_ATTACK_POWER;
            instance.bonus_specialPower += BonusConstants.BONUS_SPECIAL_POWER;
            instance.bonus_critChance += BonusConstants.BONUS_CRIT_CHANCE;
            instance.bonus_critDamage += BonusConstants.BONUS_CRIT_DAMAGE;
            instance.bonus_maxHp += BonusConstants.BONUS_MAX_HP;
            instance.changeInMaxMana += BonusConstants.BONUS_CHANGE_IN_MAX_MANA;
            instance.bonus_AttackSpeed += BonusConstants.BONUS_ATTACK_SPEED;
            instance.bonus_dmgReduction += BonusConstants.BONUS_DMG_REDUCTION;
            instance.bonus_lifeSteal += BonusConstants.BONUS_LIFE_STEAL;
            instance.bonus_spellSteal += BonusConstants.BONUS_SPELL_STEAL;
            instance.bonus_manaRegen += BonusConstants.BONUS_MANA_REGEN;
        }

        public static void RevertBonuses(Players_FindNearestAndAttack instance) {
            instance.bonus_attackPower -= BonusConstants.BONUS_ATTACK_POWER;
            instance.bonus_specialPower -= BonusConstants.BONUS_SPECIAL_POWER;
            instance.bonus_critChance -= BonusConstants.BONUS_CRIT_CHANCE;
            instance.bonus_critDamage -= BonusConstants.BONUS_CRIT_DAMAGE;
            instance.bonus_maxHp -= BonusConstants.BONUS_MAX_HP;
            instance.changeInMaxMana -= BonusConstants.BONUS_CHANGE_IN_MAX_MANA;
            instance.bonus_AttackSpeed -= BonusConstants.BONUS_ATTACK_SPEED;
            instance.bonus_dmgReduction -= BonusConstants.BONUS_DMG_REDUCTION;
            instance.bonus_lifeSteal -= BonusConstants.BONUS_LIFE_STEAL;
            instance.bonus_spellSteal -= BonusConstants.BONUS_SPELL_STEAL;
            instance.bonus_manaRegen -= BonusConstants.BONUS_MANA_REGEN;


            instance.Hp = Mathf.Min(instance.Hp, instance.bonus_maxHp);
        }
    }
}
