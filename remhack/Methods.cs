using Memory;
using AOBList;
using System.Diagnostics;

namespace HackMethods
{
    public class Hacks
    {

        private Mem mem;        
        private IDictionary<string, string> currentAddresses = new Dictionary<string, string>();        
        private long lbAddress, ubAddress;
        
        public Hacks(Mem mem)
        {
            this.mem = mem;
        }

        public bool GetMemoryRange()
        {            
            Console.WriteLine($"Identifying relevant memory range based on {AOB.MeleeSpd.name}...");

            Stopwatch w = new Stopwatch();
            w.Start();
            IEnumerable<long> results = mem.AoBScan(AOB.MeleeSpd.aob, true, false).Result; //.Result converts the await method to non-await            
            long resultCount = results.Count();            
            w.Stop();

            if (resultCount == 0)
            {                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opps: found no result! Possible reasons:");
                Console.WriteLine("- Relevant trait levels are not level 19.");
                Console.WriteLine("- Character has not yet spawned.");
                Console.WriteLine("- Old hacks values are still in effect.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (resultCount != 1)
            {
                foreach (long address in results)   //Check if results include also obsolete addresses found before
                {
                    if (currentAddresses.ContainsKey(address.ToString("X")))
                    {
                        resultCount -= 1;
                        results.ToList().Remove(address);
                    }
                }
                
                if (resultCount != 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Opps: Region scan returned {resultCount} result! Possible reasons:");
                    Console.WriteLine("- Character in the session have died many times before running remhack.");
                    Console.WriteLine("- Multiple equipments were changed during the hacks. Put back the equips then rerun hack.");
                    Console.WriteLine("- Wait 5 mins for the game to clean up memory or restart game session before running remhack again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (long item in results)
                        Console.WriteLine(item.ToString("X"));

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Try one of them? Type the index (1 = first) or 'n' to skip: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string answer = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    if (int.TryParse(answer, out int index))
                    {                        
                        if (index > resultCount)
                        {
                            Console.WriteLine("Chosen index is out of bound.");
                            return false;
                        }                        
                        Console.WriteLine($"User chose address {results.ElementAt(index - 1).ToString("X")}.");
                        lbAddress = results.ElementAt(index - 1) - 500;
                        ubAddress = results.ElementAt(index - 1) + 500;
                        Console.WriteLine($"Trying memory region {lbAddress.ToString("X")} - {ubAddress.ToString("X")}.");
                        return true;
                    }

                    return false;
                }
            }

            lbAddress = results.FirstOrDefault() - 500;
            ubAddress = results.FirstOrDefault() + 500;
            Console.WriteLine($"Found potential memory region {lbAddress.ToString("X")} - {ubAddress.ToString("X")} in {w.ElapsedMilliseconds}ms.");
            return true;
        }
        
        public void Apply(string HackName, string AOB, float ValueToApply)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Scanning {HackName}...");
            Console.ForegroundColor = ConsoleColor.White;

            //Perform Initial AOB Scan
            //.Result converts the await method to non-await
            IEnumerable<long> results = mem.AoBScan(lbAddress, ubAddress, AOB, true, false).Result;
            long resultCount = results.Count();
            string firstAddr = results.FirstOrDefault().ToString("X");

            if (resultCount != 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Opps: scan returned {resultCount} results!");
                Console.ForegroundColor = ConsoleColor.White;
                if (resultCount == 0)
                    return;
                foreach (long item in results)
                    Console.WriteLine(item.ToString("X"));                                
                Console.WriteLine($"Address taken: {firstAddr}");                
            }
            else
            {
                Console.WriteLine($"Found address: {firstAddr}");
            }

            string currentVal = mem.ReadFloat(firstAddr, round: false).ToString();            
            Console.WriteLine($"Current value: {currentVal}");

            //Apply Hack
            mem.FreezeValue(firstAddr, "float", ValueToApply.ToString());
            Console.Write($"Applied value: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(ValueToApply + "\n");
            Console.ForegroundColor = ConsoleColor.White;

            //Save Addresses to Current List
            currentAddresses.Add(firstAddr, currentVal);
        }

        public bool ToRescan()
        {
            while (true)
            {
                IEnumerable<long> results = mem.AoBScan(AOB.ReloadBuff.aob, true, false).Result;
                long resultCount = results.Count();
                if (resultCount == 1)
                {
                    Console.WriteLine("\nAddress Reset detected.");                    
                    lbAddress = results.FirstOrDefault() - 1000000000;
                    ubAddress = results.FirstOrDefault() + 1000000000;
                    Console.WriteLine($"Found new memory region {lbAddress.ToString("X")} - {ubAddress.ToString("X")}");
                    Console.WriteLine("Rescanning for new addresses...");
                    return true;
                }
                Thread.Sleep(1000);
            }
        }

        public void Release()
        {
            foreach (var address in currentAddresses)
            {
                mem.UnfreezeValue(address.Key);
                mem.WriteMemory(address.Key, "float", address.Value);   //Restore original values
            }
                            
            currentAddresses.Clear();
        }
    }
}
