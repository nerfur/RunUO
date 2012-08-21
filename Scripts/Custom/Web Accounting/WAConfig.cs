using System;
using Server;

//Fill in your MySQL database details here...

namespace Server.Accounting
{
	public class WAConfig
	{
		public const bool Enabled = false;

		public const bool UpdateOnWorldSave = true;
		public const bool UpdateOnWorldLoad = true;
	
		public const string
			DatabaseDriver		= "{MySQL ODBC 3.51 Driver}", //Shouldn't need changing
            DatabaseServer      = "74.200.236.250", //Server IP of the database
            DatabaseName        = "uoorigin_my", //Name of the database
			DatabaseTable		= "Accounts", //Name of the table storing accounts
			DatabaseUserID		= "uoorigin_admin", //Username for the database
			DatabasePassword	= "zxvf8c3u"; //Username password
	}
}