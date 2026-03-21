using System;
using static System.Console;
// 상점 판매 아이템 목록
public class TownShop
{
    private List<Items> items = new ();

    public TownShop()
    {
        // 체력 포션 20골
        items.Add(new Items 
        { 
            Name = "체력 포션", 
            Description = "체력을 20 회복합니다.", 
            Price = 20, 
            Effect = (player,bs) =>
            {
                player.Hp += 20;
            }
        });
        // 마나 포션 20골
        items.Add(new Items
        {
            Name = "마나 포션", 
            Description = "마나를 20 회복합니다.", 
            Price = 20,
            Effect = (player,bs) =>
            {
                player.Mp += 20;
            }
        });
        // 정화 포션
         items.Add(new Items
        {
            Name = "정화의 포션", 
            Description = "상태이상 1개를 정화 합니다.", 
            Price = 20,
            Effect = (player, bs) =>
            {
                if (player.statusEffects.Count > 0)
                {
                player.statusEffects.RemoveAt(player.statusEffects.Count - 1);
                    
                }
            }
        });
        
    }

    public void Enter (Player p)
    {
        while (true)
        {
            Console.Clear();
            WriteLine("== 임용수의 잡화상점 ==");
            WriteLine("[임용수]");
            WriteLine("안녕하세요. 혹시 비뇨기과 찾아오신건 아니시죠?\n요즘 들어 부쩍 비뇨기과로 착각하고 오시는 분들이 계시더라고요\n마음껏 골라보세요");
            WriteLine("상점에 입장 했습니다. 구매할 아이템을 선택하세요.");
            WriteLine();
            WriteLine($"보유 골드 : {p.gold}G");
            WriteLine();
            WriteLine("아이템 목록");
            WriteLine();
            //아이템 목록 출력
            for (int i = 0; i < items.Count; i++)
            {
                WriteLine($"[{i+1}] {items[i].Name} - {items[i].Price}\n- {items[i].Description}");
            }
            WriteLine("[0]. 상점 나가기");
            Write(">>");
            string input = ReadLine();
            if (input == "0") break;
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= items.Count)
            {
                Items selected = items[choice - 1];
                if (p.gold < 0)
                {
                    WriteLine("골드가 부족합니다. 살 수 있는 아이템이 없습니다.");
                    continue;
                }
                if (p.gold < selected.Price)
                {
                    WriteLine("골드가 부족합니다.");
                    continue;
                }
                p.gold -= selected.Price;
                if (p.Inv.ContainsKey(selected.Name))
                {
                    p.Inv[selected.Name]++;
                }
                else
                {
                    p.Inv[selected.Name] = 1;
                   
                }
                 WriteLine($"[{selected.Name}]을 구매했습니다. (현재 갯수:{p.Inv[selected.Name]})");
            }

        }
        
    }
}