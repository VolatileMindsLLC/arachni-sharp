using System;
using arachnisharp;
using MsgPack;
using System.Threading;
using System.Collections.Generic;

namespace example
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (ArachniSession session = new ArachniSession ("192.168.2.207", 4567, true)) {
				using (ArachniManager manager = new ArachniManager (session)) {
					var blah = manager.IsBusy ();

					var resp = manager.StartScan ("http://192.168.2.87/cgi-bin/badstore.cgi?searchquery=fdsa&action=search&x=20&y=12");

					Console.WriteLine ("Running");

					bool isRunning = manager.IsBusy ().AsBoolean ();
					List<uint> issues = new List<uint> ();
					DateTime start = DateTime.Now;
					Console.WriteLine ("Starting scan at " + start.ToLongTimeString ());
					while (isRunning) {
						var progress = manager.GetProgress (issues);
						foreach (MessagePackObject p in progress.AsDictionary()["issues"].AsEnumerable()) {
							MessagePackObjectDictionary dict = p.AsDictionary ();
							Console.WriteLine ("Issue found: " + dict ["name"].AsString ());
							issues.Add (dict ["digest"].AsUInt32());
						}
						Thread.Sleep (10000);
						isRunning = manager.IsBusy ().AsBoolean ();
					}
					DateTime end = DateTime.Now;
					Console.WriteLine ("Finishing scan at " + end.ToLongTimeString () + ". Scan took " + ((end - start).ToString ()) + ".");

					Console.WriteLine ("done");
				}
			}
		}
	}
}
