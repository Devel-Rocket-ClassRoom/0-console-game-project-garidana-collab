using System;
using static System.Console;
// 상점 판매 아이템 목록
public class TownShop
{
    private List<Items> items = new ();

    public TownShop()
    {
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
            WriteLine("== 임용수의 잡화상점 ==");
            WriteLine("상점에 입장 했습니다. 구매할 아이템을 선택하세요.");
            WriteLine();
            WriteLine($"보유 골드 : {p.gold}G");
            WriteLine();
            WriteLine("아이템 목록");
            WriteLine();
            //아이템 목록 출력
            for (int i = 0; i < items.Count; i++)
            {
                WriteLine($"[{i}] {items[i].Name} - {items[i].Price}\n- {items[i].Description}");
            }
            WriteLine("[0]. 상점 나가기");
            Write(">>");
            string input = ReadLine();
            if (input == "0") break;
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= items.Count)
            {
                Items selected = items[choice];

                if (player)
            }

        }
        
    }
}