// Client
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				//Создаем правильную строку с тремя разными числами ("правильная строка" означает, что в ней встреяаются только пробелы и цифры) 
				bool ifMessageIncorrect = true;
				string message = "";
				while (ifMessageIncorrect)
				{
					Console.Clear();
					Console.WriteLine("Введите три разных числа, через пробел. Пример:\"1 2 3\": \n");
					message = Console.ReadLine();

					bool ifLettersWereInMessage = false;
					for (int i = 0; i < message.Length; i++)
						if ((message[i] < '0' || message[i] > '9') && message[i] != ' ')
						{
							Console.WriteLine("\nОбнаружены буквы! Пожалуйста, вводите только цифры и пробелы между ними.\nНажмите Enter для повторного ввода...");
							ifLettersWereInMessage = true;
							break;
						}
					if (ifLettersWereInMessage)
					{
						Console.ReadLine();
						continue;
					}
					string[] test = message.Split(' ');
					if (test.Length != 3)
					{
						Console.WriteLine("\nПожалуйста, введите только три числа!\nНажмите Enter для повторного ввода...");
						Console.ReadLine();
						continue;
					}
					else
						if (test[0].ToString() == test[1].ToString() || test[0].ToString() == test[2].ToString() || test[1].ToString() == test[2].ToString())
						{
							Console.WriteLine("\nПожалуйста, вводите разные числа!\nНажмите Enter для повторного ввода...");
							Console.ReadLine();
						}
						else ifMessageIncorrect = false;
				}
 
				// Буфер для входящих данных
				byte[] buffer = new byte[1024];

				// Соединяемся с сервером

				// Устанавливаем удаленную точку для сокета
				IPHostEntry ipHost = Dns.GetHostEntry("localhost");
				IPAddress ipAddr = ipHost.AddressList[0];
				IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

				Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

				// Соединяем сокет с удаленной точкой
				sender.Connect(ipEndPoint);
				Console.WriteLine("\nСоединение с сервером...");
				byte[] msg = Encoding.UTF8.GetBytes(message);

				// Отправляем данные через сокет
				int bytesSent = sender.Send(msg);

				// Получаем ответ от сервера
				int bytesRec = sender.Receive(buffer);

				Console.WriteLine("Ответ от сервера: {0}\n\n", Encoding.UTF8.GetString(buffer, 0, bytesRec));

				// Разрываем соединение с сервером
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Console.ReadLine();
			}
		}

		static void SendMessageFromSocket(int port)
		{
			
		}
	}
}