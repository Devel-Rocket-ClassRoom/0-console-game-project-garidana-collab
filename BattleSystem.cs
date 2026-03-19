using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using static System.Console;
using static System.Net.Mime.MediaTypeNames;

// BattleSystem.cs


public class BattleSystem
{
    private Random _random = new Random();
    public int Round = 1;
    public bool IsCrit;
    public bool IsEvade;
    // 전투 실행
    public bool RunBattle(Player player, Monster monster)
    {
        Console.WriteLine($"\n===== {monster.Name} 출현! =====\n");

        while (player.IsAlive && monster.IsAlive)
        {
            WriteLine("------------------");
            WriteLine($"<{Round}번째 라운드>");
            WriteLine();
            // 플레이어 턴

            Console.WriteLine($"[{player.Name}] HP: {player.Hp}/{player.MaxHp} MP: {player.Mp}/{player.MaxMp}");

            Console.WriteLine($"[{monster.Name}] HP: {monster.Hp}/{monster.MaxHp}");
            WriteLine();
            // 스킬 선택
            player.PrintSkills(); // 플레이어 스킬 목록 출력
            WriteLine();
            Console.Write("선택 >> ");
            string input = Console.ReadLine(); // 스킬 선택 입력 받음
            Console.WriteLine();
            // 스킬 선택 입력 tryparse
            if (!int.TryParse(input, out int idx) || idx < 0 || idx >= player.skills.Count)
            {
                WriteLine($"올바른 숫자를 입력하세요");
                continue;
            }
            else if (player.skills[idx].CurrentCD > 0)
            {
                WriteLine("스킬이 쿨타임 중입니다.");
                continue;
            }
            else if (player.Mp < player.skills[idx].ManaCost)
            {
                WriteLine("마나가 부족합니다.");
                continue;
            }
            // 입력받은 스킬 사용
            Skill selected = player.skills[idx];
            // 사용할 스킬 정보 출력
            WriteLine($"[{selected.Name}] 사용 ({selected.Description})");
            WriteLine();
            selected.Effect(player, monster, this);
            // 마나 차감과 쿨타임 갱신
            player.Mp -= selected.ManaCost;
            selected.CurrentCD = selected.CoolDown;
            //Thread.Sleep(2000);

            if (!monster.IsAlive) break;

            // 몬스터 턴
            if (monster.IsAlive)
            {
                monster.ExecuteSkill(player, this);
            }

            // 한 라운드 끝
            Round++; // 라운드 증가
            // 스킬 남은 쿨다운 감소
            foreach (var skill in player.skills)
            {
                if (skill.CurrentCD > 0)
                {
                    skill.CurrentCD--;
                }
            }
            // 마나 회복
            // Math.Min(int a, int b) 는 둘중 더 작은 값을 반환
            player.Mp = Math.Min(player.Mp + 5, player.MaxMp); // 현재 마나는 최대마나를 넘길 수 없음
            if (!player.IsAlive) break;

            //Console.Clear();
        }

        if (player.IsAlive)
        {
            Write(">> ");

            Console.WriteLine($"{monster.Name}을(를) 성공적으로 처치했다....");
            Round = 0;
            return true;
        }
        else
        {
            Console.WriteLine($"{monster.Name} 에게 플레이어가 살해당했다...");
            Console.WriteLine($"{player.Name} 사망...");
            return false;
        }

    }








    // 플레이어가 데미지 주는 메서드
    public void PlayerDealDamage(Character attacker, Character defender, double multiplier, double bonusDmg = 0)  // 스킬내부에서 DealDamage계산
    {
        double damage = attacker.Attack * multiplier + bonusDmg;
        // 치명타
        if (_random.NextDouble() <= attacker.CritChance)
        {
            attacker.IsCrit = true;
            damage *= 2;
        }

        WriteLine(attacker.IsCrit ? $"[{attacker.Name}]의 치명타 공격!" : $"[{attacker.Name}]의 공격!");

        // 위의 내용으로 TakeDamage 호출
        TakeDamage(defender, damage);
        attacker.IsCrit = false;
    }
    // 몬스터가 데미지 주는 메서드 (치명타 데미지 배율이 다름
    public void MonsterDealDamage(Character attacker, Character defender, double multiplier)  // 스킬내부에서 DealDamage계산
    {
        double damage = attacker.Attack * multiplier;
        // 치명타
        if (_random.NextDouble() <= attacker.CritChance)
        {
            attacker.IsCrit = true;
            damage *= 1.5;
        }
        WriteLine(attacker.IsCrit ? $"[{attacker.Name}]의 치명타 공격!" : $"[{attacker.Name}]의 공격!");

        // 위의 내용으로 TakeDamage 호출
        TakeDamage(defender, damage);
        attacker.IsCrit = false;
    }
    // 피격자 데미지 받는 메서드 // 후에 스킬 내부에서 데미지 처리
    public void TakeDamage(Character defender, double damage)
    {
        // 피격자 회피
        if (_random.NextDouble() <= defender.EvadeChance)
        {
            defender.IsEvade = true;
            damage = 0;
        }
        // defender가 플레이어일 때 StatusEffect를 보유하고 있는지
        if (defender is Player p)
        {
            foreach (var effect in p.statusEffects)
            {
                if (effect.OnTakeDamage != null)
                {
                    damage = effect.OnTakeDamage(damage);
                    // 이펙트 지속시간 -1
                    effect.Duration--;

                }
            }
            // 지속효과의 지속시간이 다되면 지속효과 제거
            p.statusEffects.RemoveAll(e => e.Duration <= 0);
        }
        defender.Hp -= damage;
        WriteLine(defender.IsEvade ? $"[{defender.Name}]이(가) 공격을 회피했다! 아무런 피해가 없었다.." : $"[{defender.Name}]이(가) {damage} 피해를 입었다.");
        defender.IsEvade = false;
        WriteLine();



    }
}