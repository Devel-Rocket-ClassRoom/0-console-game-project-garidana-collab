using System;


public class Items
{
    public string Name;
    public string Description;
    public int Price;
    public Action <Player,BattleSystem> Effect;
}