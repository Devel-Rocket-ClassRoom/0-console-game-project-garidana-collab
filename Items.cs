using System;


public class Items
{
    public string Name;
    public string Description;
    public int Price;
    public Action <Player,BattleSystem> Effect;

    public static Dictionary<string, Items> All = new()
    {
        {"체력 포션", new Items
            {
                Name = "체력 포션",
                Description = "체력을 20 회복합니다.",
                Price = 20,
                Effect = (player,bs) =>
                {
                    player.Hp += 20;
                }
            }
        },
        {"마나 포션", new Items
            {
                Name = "마나 포션",
                Description = "마나를 20 회복합니다.",
                Price = 20,
                Effect = (player,bs) =>
                {
                    player.Mp += 20;
                }
        }
        },
        {"정화 포션", new Items
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
            }
        }
    };
}