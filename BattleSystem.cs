using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Linq;
using static System.Console;

// BattleSystem.cs


public class BattleSystem
{
    private Random _random = new Random();
    public int Round = 0;
    public bool IsCrit;
    public bool IsEvade;
    // 전투 실행
    public bool RunBattle(Player player, Monster monster)
    {
        if (player.Hp <= player.MaxHp * 0.6 && player.Hp > 0)
        {
            player.Hp += 30;
        }
        Clear();
        WriteLine("저 멀리 무언가 보입니다.");
        WriteLine("전투를 준비합니다... 아무키나 누르세요");
        WriteLine($"체력: {player.Hp} 마나: {player.Mp}");
        Write(">> ");
        ReadLine();
        Console.Clear();
        Console.WriteLine($"\n>>>>> {monster.Name} 출현! <<<<<\n");
        // 불의 저령이후 몬스터들의 전용 출현 메시지
        if (monster.HeatDamage > 0)
        {
            WriteLine("-------------------------------------");
            WriteLine("엄청난 열기가 느껴집니다. 중심부에 가까워지는 느낌입니다.");
            WriteLine($"라운드 마다 {monster.HeatDamage}의 열기 피해를 입습니다.");
        }

        while (player.IsAlive && monster.IsAlive)
        {
           

            WriteLine("-------------------------------------");
            if (Round == 0)
            {
                WriteLine("<전투 시작>\n");
            }
            else
            {
                WriteLine($"<{Round} 라운드>\n");
            }
            // 플레이어 턴

            Console.WriteLine($"[{player.Name}] HP: {player.Hp}/{player.MaxHp} MP: {player.Mp}/{player.MaxMp}");

            Console.WriteLine($"[{monster.Name}] HP: {monster.Hp}/{monster.MaxHp}");
            WriteLine();

            // 플레이어 지속효과 지속시간 감소 
            foreach (var se in player.statusEffects)
            {
                if (se.Duration > 0)
                {
                    se.Duration--;
                }

            }
            var pExpired = player.statusEffects.Where(se => se.Duration <= 0).ToList();
            // 플레이어 만료된 지속효과 (효과를 받은 스택의 원상복귀) OnExpire 델리게이트 호출
            foreach (var se in pExpired)
            {
                if (se.Duration <= 0 && se.OnExpire != null)
                {
                    se.OnExpire(player, this);
                }
            }
            // 플레이어 지속시간이 다 되면 지속효과 삭제
            player.statusEffects.RemoveAll(se => se.Duration <= 0);
            // 플레이어 현재 적용 되는 지속효과 출력
            player.PrintStatusEffects();
            player.IsIncap = false;
            // 플레이어 잔여 지속효과 체크 후 적용
            foreach (StatusEffect effect in player.statusEffects)
            {
                if (effect.OnTurnStart != null)
                {
                    effect.OnTurnStart(player, this);
                }
            }
            // 플레이어 전투불능 상태 일시 메세지 출력 후 턴 스킵
            if (player.IsIncap)
            {
                Console.WriteLine($"{player.Name}은 현재 전투 불능 상태이다.. 아무것도 하지 못한다..");
                Thread.Sleep(200);
            }
            else
            {
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
                Skill selectedSkill = player.skills[idx];
                // 사용할 스킬 정보 출력
                WriteLine($"[{selectedSkill.Name}] 사용 ({selectedSkill.Description})");
                WriteLine();
                selectedSkill.Effect(player, monster, this);

                // 마나 차감과 쿨타임 갱신
                player.Mp -= selectedSkill.ManaCost;
                selectedSkill.CurrentCD = selectedSkill.CoolDown;
            }

            if (!monster.IsAlive) break;

            // 몬스터 턴
            // 몬스터 지속효과 지속시간 감소 및 효과 삭제
            foreach (var se in monster.statusEffects)
            {
                if (se.Duration > 0)
                {
                    se.Duration--;
                }

            }
            // 몬스터 만료된 지속효과 OnExpire 호출
            var mExpired = monster.statusEffects.Where(se => se.Duration <= 0).ToList();
            foreach (var se in mExpired)
            {
                if (se.Duration <= 0 && se.OnExpire != null)
                {
                    se.OnExpire(monster, this);
                }
            }
            monster.statusEffects.RemoveAll(se => se.Duration <= 0);

            if (monster.IsAlive)
            {
                monster.IsIncap = false;
                // 턴 시작시 적용할 지속효과가 있으면 적용
                foreach (StatusEffect effect in monster.statusEffects)
                {
                    if (effect.OnTurnStart != null)
                    {
                        effect.OnTurnStart(monster, this);
                    }
                }
                // 전투 불능 검사
                if (monster.IsIncap)
                {
                    WriteLine($"{monster.Name}은(는) 전투불능 상태이다. 아무것도 하지 못했다..");
                    
                }
                // 공격 게시
                else
                {
                    monster.ExecuteSkill(player, this);
                }
            }

            // 맵의 열기 데미지
            if (monster.HeatDamage > 0)
            {
                int IncreasedHeat = monster.HeatDamage * 2;
                if (player.statusEffects.Any(se => se.Name == "화상"))
                {
                    player.Hp -= IncreasedHeat;
                }
                else
                {
                    player.Hp -= monster.HeatDamage;
                }
            }
            // 한 라운드 끝
            Round++; // 라운드 증가

            // 한 라운드가 끝나고 처리해야할 사항
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
            Console.WriteLine($"{monster.Name}을(를) 성공적으로 처치했다....");
            Thread.Sleep(300);
            WriteLine(".");
            Thread.Sleep(300);
            WriteLine(".");
            Thread.Sleep(300);
            WriteLine(".");
            WriteLine("전리품을 얻었습니다. 획득하려면 아무키나 누르세요");
            ReadLine();
            WriteLine("전리품을 챙기고 더 나아갑니다.");
            Thread.Sleep(500);
            WriteLine("터벅");
            Thread.Sleep(500);
            WriteLine("터벅");
            Thread.Sleep(500);
            WriteLine("터벅 X 100");
            Thread.Sleep(500);
            Round = 0;
            monster.HeatDamage = 0;
            return true;
        }
        else
        {
            Console.WriteLine($"{monster.Name} 에게 플레이어가 살해당했다...");
            Console.WriteLine($"GAME OVER...");
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
        // 전투불능 상태에서 회피 무효
        if (defender.IsIncap)
        {
            defender.IsEvade = false;
        }
        // 피격자 회피
        else if (_random.NextDouble() <= defender.EvadeChance)
        {
            defender.IsEvade = true;
            damage = 0;
        }
        // defender가 플레이어일 때 OnTakeDamage StatusEffect를 보유하고 있는지
        if (defender is Player p)
        {
            foreach (var effect in p.statusEffects)
            {
                if (effect.OnTakeDamage != null)
                {
                    damage = effect.OnTakeDamage(damage);
                }
            }
            
        }
        // defender가 몬스터일 때 OnTakeDamage 있는지
        if (defender is Monster m)
        {
            foreach (var effect in m.statusEffects)
            {
                // 몬스터도 데미지 감소 효과 적용 
                if (effect.OnTakeDamage != null)
                {
                    damage = effect.OnTakeDamage(damage);
                }
            }
        }

        defender.Hp -= damage;
        WriteLine(defender.IsEvade ? $"[{defender.Name}]이(가) 공격을 회피했다! 아무런 피해가 없었다.." : $"[{defender.Name}]이(가) {damage} 피해를 입었다.");
        defender.IsEvade = false;
        WriteLine();



    }
}