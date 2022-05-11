//Remnant From The Ashes
//Requires Admin Elevation
//Remember to Unfreeze hack between each session

using AOBList;
using HackMethods;
using Memory;
using System.Media;

Mem m = new Mem();
Hacks h = new Hacks(m);

SoundPlayer success = new SoundPlayer(remhack.Properties.Resources.success);
SoundPlayer fail = new SoundPlayer(remhack.Properties.Resources.fail);
SoundPlayer cleared = new SoundPlayer(remhack.Properties.Resources.cleared);

Console.WindowHeight = 30;
Console.WindowWidth = 120;

Start:

//Get open Remnant process
if (m.OpenProcess("Remnant-Win64-Shipping.exe"))
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("Target process found.");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Remember to defrost mem before changing gears else game will crash.");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Process not found!");
    Console.ForegroundColor = ConsoleColor.White;
    goto AskUser;
}

//Get memory range
if (!h.GetMemoryRange())
{
    fail.Play();
    goto AskUser;
}
    

DoHacks:

//Release prev hacks
h.Release();

//Apply hacks
h.Apply(AOB.MeleeSpd.name, AOB.MeleeSpd.aob, AOB.MeleeSpd.val);
h.Apply(AOB.MvmtSpdBuff.name, AOB.MvmtSpdBuff.aob, AOB.MvmtSpdBuff.val);
h.Apply(AOB.FireRate.name, AOB.FireRate.aob, AOB.FireRate.val);
h.Apply(AOB.ReloadBuff.name, AOB.ReloadBuff.aob, AOB.ReloadBuff.val);
h.Apply(AOB.ConsumeSpd.name, AOB.ConsumeSpd.aob, AOB.ConsumeSpd.val);

success.Play();

////Maintain hacks
////Disabled due to performance - taxing.
//if (h.ToRescan())
//    goto DoHacks;

AskUser:

Console.WriteLine("\nAwaiting user's input...");
Console.WriteLine("  r : Rescan the memory to apply hacks");
Console.WriteLine("  s : Defrost memory and release all current hacks");

Console.ForegroundColor = ConsoleColor.Red;
Console.Write("Your choice: ");
Console.ForegroundColor = ConsoleColor.Green;
string answer = Console.ReadLine();
Console.ForegroundColor = ConsoleColor.White;

if (answer == "r")
{
    Console.WriteLine();
    goto Start;
}    
else if (answer == "s")
{
    h.Release();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("Memory defrosted! Original values restored.");
    Console.ForegroundColor = ConsoleColor.White;
    cleared.Play();
    goto AskUser;
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Unknown command.");
    Console.ForegroundColor = ConsoleColor.White;
    goto AskUser;
}
