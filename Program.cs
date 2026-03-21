
using System;
using static System.Console;
using System.Threading;


//WriteLine("====== 김경일의 모험 ver 0.0.5 ======");
Town ChunHo = new();
BattleSystem BS = new();
Player p = ChunHo.Enter();
WriteLine("준비가 되었으니 던전에 입장합니다...");
Thread.Sleep(1000);
WriteLine(".");
Thread.Sleep(1000);
WriteLine(".");
Thread.Sleep(1000);
WriteLine(".");
Console.Clear();
WriteLine("<<화산 동굴>>에 입장하였습니다.");
WriteLine();
WriteLine("            ~이글~");
WriteLine("                              ~불~");
WriteLine(" ~용암~");
WriteLine("       ~불~");
WriteLine("                     ~이글~");
WriteLine();
WriteLine("동굴 내부의 온도가 꽤나 높습니다...");
WriteLine();
WriteLine("더 깊숙히 들어갑니다... (아무키나 입력)");
Console.ReadLine();

// Warrior w = new("김경일");
// Mage m = new Mage("김경일");
// Ranger r = new("김경일");
VolcanoMole vm = new();
SulfuricSlime ss = new();
FireSpirit fs = new();
MagmaTortoise mt = new();
//EarthWorm ew = new();
BS.RunBattle(p, vm);
BS.RunBattle(p, ss);
BS.RunBattle(p, fs);
BS.RunBattle(p, mt);
//BS.RunBattle(p.ew);


