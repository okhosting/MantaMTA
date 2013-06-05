﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Transactions;
using Colony101.MTA.Library;
using Colony101.MTA.Library.Server;
using NUnit.Framework;
	

namespace MTALibraryTests
{
	[TestFixture]
	public class ServerTests : TestFixtureBase
    {
		/// <summary>
		/// Test that we can create a server and connect to it.
		/// </summary>
		[Test]
		public void CreateServer()
		{
			using (SmtpServer s = new SmtpServer(8000))
			{
				TcpClient client = new TcpClient();
				client.Connect(IPAddress.Parse("127.0.0.1"), 8000);
				client.Close();
			}
		}

		/// <summary>
		/// Test that a message is received.
		/// </summary>
		[Test]
		public void SendMessage()
		{
			using (TransactionScope ts = CreateTransactionScopeObject())
			{
				using (SmtpServer s = new SmtpServer(8000))
				{
					TcpClient client = new TcpClient();
					client.Connect(IPAddress.Parse("127.0.0.1"), 8000);
					SmtpStreamHandler smtp = new SmtpStreamHandler(client);

					Action<string, string> sendLine = new Action<string,string>(delegate(string cmd, string expectedResponse)
					{
						smtp.WriteLine(cmd, false);
						string response = smtp.ReadLine(false);
						Console.WriteLine(cmd + " " + expectedResponse + " " + response);
						Assert.AreEqual(expectedResponse, response.Substring(0, 3));
					});

					smtp.ReadLine();
					sendLine("HELO localhost", "220");
					sendLine("MAIL FROM: <local@localhost>", "250");
					sendLine("RCPT TO: <local@localhost>", "250");
					sendLine("DATA", "354");
					smtp.WriteLine("Hello", false);
					sendLine(".", "250");
					smtp.WriteLine("QUIT");
				}
			}
		}

		/// <summary>
		/// Tests that a message is queued for relaying to somewhere else
		/// </summary>
		[Test]
		public void QueueMessage()
		{
			using (TransactionScope ts = CreateTransactionScopeObject())
			{
				using (SmtpServer s = new SmtpServer(8000))
				{
					TcpClient client = new TcpClient();
					client.Connect(IPAddress.Parse("127.0.0.1"), 8000);
					SmtpStreamHandler smtp = new SmtpStreamHandler(client);

					Action<string, string> sendLine = new Action<string, string>(delegate(string cmd, string expectedResponse)
					{
						smtp.WriteLine(cmd, false);
						string response = smtp.ReadLine(false);
						Console.WriteLine(cmd + " " + expectedResponse + " " + response);
						Assert.AreEqual(expectedResponse, response.Substring(0, 3));
					});

					smtp.ReadLine();
					sendLine("HELO localhost", "220");
					sendLine("MAIL FROM: <local@localhost>", "250");
					sendLine("RCPT TO: <daniel.longworth@colony101.co.uk>", "250");
					sendLine("DATA", "354");
					smtp.WriteLine("Hello", false);
					sendLine(".", "250");
					smtp.WriteLine("QUIT");
				}
			}
		}
    }
}