using System;
using arachnisharp;
using MsgPack;
using System.Threading;

namespace example
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (ArachniSession session = new ArachniSession ("192.168.2.207", 4567, true)) {
				using (ArachniManager manager = new ArachniManager (session)) {
					var blah = manager.IsBusy ();

					var resp = manager.StartScan ("http://192.168.2.87/?searchquery=fdsa&action=search&x=11&y=15");

					Console.WriteLine ("Running");

					while (!manager.IsBusy ().IsNil ) {
						Thread.Sleep (10000);
						Console.Write (".");
					}

					var trewq = manager.GetResults ();
					foreach (var pair in trewq.AsDictionary())
						Console.WriteLine (pair.Key + ": " + pair.Value);

					Console.WriteLine ("done");
				}
			}
		}
	}
}
