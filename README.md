# SmartCard

This components is able to read and write numeric and alphanumeric data from GemClubMemo and other type's Smartcard. The GemClubMemo produced by www.gemplus.com, this kind of smartcard have a very tiny memory but as well these have a very cheap price! Although everything, this kind of card is the best solution for a solid "protection" circuit, even because each card has onboard three different passwords (commonly named "Card Secret Code) and a protection that will put off-line the card after three unsuccesul authentication attempts for each password.

<br>

Properties | Description |
------------- | -------------
ATR|Return a string to describe the Answer to Reset
Enabled|Active events of insertion/removal card
ErrorDescription|Return description of errors
ErrorNumber|Return code number of error
Interval|Set the interval time ot check state
ReaderName|Return or set the reader name of to use
State|Return the state of the reader or the card
StatusWord|Return a code to describe the state of last APDU command

<br>

Methods | Description |
------------- | -------------
APDUCommand|Send generic APDU commands
Connect|Connect to smartcard for operations
Disconnect|Disconnect from scmartcard
ReadCard|Read data from smartcard
SendCSC|Send the CSC to able I/O operations to smartcard
WriteCard|Write data to smartcard

<br>

```csharp

	using Priore.Controls;	using System.Timers;

	SmartCard.StateConstants iState = SmartCard.StateConstants.STATE_ABSENT;	SmartCard SCard = new SmartCard();
	SCard.Errors +=new Priore.Controls.SmartCard.ErrorsEventHandler(SCard_Errors);	Timer tmr = new Timer();
	tmr.Elapsed +=new ElapsedEventHandler(tmr_Elapsed);	tmr.Interval = 1000;	tmr.Enabled = true;	Console.WriteLine("READER: " + SCard.ReaderName);	private void SCard_Errors(object Sender, int Number, string Description)	{		Console.WriteLine("ERROR: " + Number + " " + Description);	}	private void tmr_Elapsed(object sender, ElapsedEventArgs e)	{		SmartCard.StateConstants s = SCard.State;		if (s != iState)		{			iState = s;			switch (s)			{				case SmartCard.StateConstants.STATE_ABSENT:					Console.WriteLine("EVENT: CardOut");					break;				case SmartCard.StateConstants.STATE_SPECIFIC:					Console.WriteLine("EVENT: CardIn");					break;			}		}	}
```		
_Attention! you can download and use this code, but is a discontinued code and the supports not are longer available._
