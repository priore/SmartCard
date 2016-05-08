using System;
using Priore.Controls;
using System.Timers;

namespace CSharp_SCard_Test
{

	class MyClass
	{
		SmartCard.StateConstants iState = SmartCard.StateConstants.STATE_ABSENT;
		SmartCard SCard = new SmartCard();
		Timer tmr = new Timer();

		[STAThread]
		static void Main(string[] args)
		{
			MyClass myclass = new MyClass();
			myclass.SCardInit();

			Console.WriteLine("Press \'q\' to exit.");
			while(Console.Read()!='q');
		}

		void SCardInit()
		{
			SCard.Errors +=new Priore.Controls.SmartCard.ErrorsEventHandler(SCard_Errors);

			tmr.Elapsed +=new ElapsedEventHandler(tmr_Elapsed);
			tmr.Interval = 1000;
			tmr.Enabled = true;

			Console.WriteLine("READER: " + SCard.ReaderName);

		}

		private void SCard_Errors(object Sender, int Number, string Description)
		{
			Console.WriteLine("ERROR: " + Number + " " + Description);
		}

		private void tmr_Elapsed(object sender, ElapsedEventArgs e)
		{
			SmartCard.StateConstants s = SCard.State;
			if (s != iState)
			{
				iState = s;
				switch (s)
				{
					case SmartCard.StateConstants.STATE_ABSENT:
						Console.WriteLine("EVENT: CardOut");
						break;
					case SmartCard.StateConstants.STATE_SPECIFIC:
						Console.WriteLine("EVENT: CardIn");
						break;
				}
			}
		}
	}
}
