using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion_Tostatronic.Services.Ticket
{
    public static class PrintLanPrinter
    {
        public static void Print(string ipP, string fn)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.NoDelay = true;

            IPAddress ip = IPAddress.Parse(ipP);
            IPEndPoint ipep = new IPEndPoint(ip, 9100);
            clientSocket.Connect(ipep);

            byte[] fileBytes = File.ReadAllBytes(@"C:\Users\Tostatronic\Desktop\example.oxps");

            clientSocket.Send(fileBytes);
            clientSocket.Close();
        }

        public static void Print2(string fn)
        {
            PrintDialog pd = new PrintDialog();

            pd.PrinterSettings.PrinterName = @"\\192.168.3.25\";
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "printto";
            info.FileName = fn;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo.Arguments = pd.PrinterSettings.PrinterName.ToString();
            pd.PrinterSettings.MaximumPage = 1;
            p.StartInfo = info;
            p.Start();
            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
        }
    }
}
