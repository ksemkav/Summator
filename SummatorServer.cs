// Server
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
	class Program
	{

		static void Main(string[] args)
		{
			// Устанавливаем для сокета локальную конечную точку
			IPHostEntry ipHost = Dns.GetHostEntry("localhost");
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

			// Создаем сокет Tcp/Ip
			Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			// Назначаем сокет локальной конечной точке и слушаем входящие сокеты
			try
			{
				sListener.Bind(ipEndPoint);
				sListener.Listen(0);

				// Начинаем слушать соединения
				while (true)
				{
					Console.WriteLine("Ожидаем соединения с клиентом...\n");

					// Программа приостанавливается, ожидая входящее соединение
					Socket client = sListener.Accept();
					string inputMessage = null;

					// Мы дождались клиента, пытающегося с нами соединиться, и получаем от него сообщение с тремя числами
					byte[] bytes = new byte[1024];
					int bytesRec = client.Receive(bytes);
					
					inputMessage += Encoding.UTF8.GetString(bytes, 0, bytesRec);
					//Разбиваем строку с тремя числаи на отдельные числа. Затем суммируем их.
					string[] numbers = inputMessage.Split(' ');
					int summ = Convert.ToInt32(numbers[0]) + Convert.ToInt32(numbers[1]) + Convert.ToInt32(numbers[2]);

					Console.Write(String.Format("Пришли данные от клиента. Cумма: {0} + {1} + {2} = {3}\n\n",numbers[0], numbers[1], numbers[2], summ));

					// Отправляем полученный результат клиенту
					string reply = String.Format("Сумма отправленных Вами чисел равна {0}", summ); 
					byte[] msg = Encoding.UTF8.GetBytes(reply);
					client.Send(msg);
					
					//Разрываем соединение с текущим клиентом
					client.Shutdown(SocketShutdown.Both);
					client.Close();
				}
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
	}
}