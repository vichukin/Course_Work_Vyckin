using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Server
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Task task;
        string GetLocalIP()
        {
            using (Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket1.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket1.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            //if (task == null)
            //{
            //    IPAddress address = IPAddress.Parse("192.168.0.1");
            //    IPEndPoint end = new IPEndPoint(address, 1024);

            //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            //    socket.Bind(end);
            //    task = Task.Run(() => ListenerFunc(socket));
            //}
            StartServ();
        }
        //EndPoint White;
        //EndPoint Black;

        //async void ListenerFunc(Socket socket)
        //{
        //    IPEndPoint end = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 1024);

        //    try
        //    {
        //        do
        //        {
        //            byte[] buf = new byte[1024];
        //            IPEndPoint clientend = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 1025);

        //            SocketReceiveFromResult res = await socket.ReceiveFromAsync(new ArraySegment<byte>(buf), SocketFlags.None, clientend);
        //            string text = Encoding.Default.GetString(buf, 0, res.ReceivedBytes);
        //            if (text == "Ready")
        //            {
        //                if (!Game)
        //                {
        //                    if (White != null)
        //                    {
        //                        Black = res.RemoteEndPoint;
        //                        buf = Encoding.Default.GetBytes("Black");
        //                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                    }
        //                    else if (Black != null)
        //                    {
        //                        White = res.RemoteEndPoint;
        //                        buf = Encoding.Default.GetBytes("White");
        //                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                    }
        //                    else
        //                    {
        //                        Random random = new Random();
        //                        int rnd = random.Next(0, 1);
        //                        if (rnd == 0)
        //                        {
        //                            White = res.RemoteEndPoint;
        //                            buf = Encoding.Default.GetBytes("White");
        //                            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                        }
        //                        else
        //                        {
        //                            Black = res.RemoteEndPoint;
        //                            buf = Encoding.Default.GetBytes("Black");
        //                            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                        }
        //                    }
        //                    if (White != null && Black != null)
        //                    {
        //                        Game = true;
        //                        buf = Encoding.Default.GetBytes("Start Game");
        //                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                        buf = Encoding.Default.GetBytes("Start Game");
        //                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                    }
        //                }
        //                else
        //                {
        //                    buf = Encoding.Default.GetBytes("Error");
        //                    await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, res.RemoteEndPoint);
        //                }
        //            }
        //            else
        //            {
        //                List<StepInfo> st = JsonConvert.DeserializeObject<List<StepInfo>>(text);
        //                //MessageBox.Show(text);
        //                int counter = 0;
        //                foreach (var item in st)
        //                {
        //                    if (item.Fight != (50, 50))
        //                        counter++;
        //                }
        //                if (counter == 0)
        //                    StepsWithOutFight++;
        //                else
        //                    StepsWithOutFight = 0;
        //                if(White.ToString() == res.RemoteEndPoint.ToString())
        //                {
        //                    BlackFigures -= counter;
        //                    await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                }
        //                else if(Black.ToString() == res.RemoteEndPoint.ToString())
        //                {
        //                    WhiteFigures -= counter;
        //                    await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                }

        //            }
        //            if (WhiteFigures == 0)
        //            {
        //                buf = Encoding.Default.GetBytes("Win");
        //                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                buf = Encoding.Default.GetBytes("Lose");
        //                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                Game = false;
        //            }
        //            else if (BlackFigures == 0)
        //            {
        //                buf = Encoding.Default.GetBytes("Lose");
        //                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                buf = Encoding.Default.GetBytes("Win");
        //                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                Game = false;
        //            }
        //            else if ((WhiteFigures == 1 && BlackFigures == 1) || StepsWithOutFight == 20)
        //            {
        //                buf = Encoding.Default.GetBytes("Draw");
        //                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
        //                buf = Encoding.Default.GetBytes("Draw");
        //                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
        //                Game = false;

        //            }

        //        } while (true);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        throw;
        //    }
        //    finally
        //    {
        //        socket.Shutdown(SocketShutdown.Both);
        //        socket.Close();
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {


        }
        TcpClient clientOne;
        TcpClient clientTwo;
        TcpClient White;
        TcpClient Black;
        TcpListener tcpServer;
        int WhiteFigures = 12;
        int BlackFigures = 12;
        int StepsWithOutFight = 0;
        bool Game = false;
        public async void StartServ()
        {
            var ipAddress = IPAddress.Parse("192.168.0.26");
            var ipEndpoint = new IPEndPoint(ipAddress, 1024);
            tcpServer = new TcpListener(ipEndpoint);
            tcpServer.Start();

            clientOne = await tcpServer.AcceptTcpClientAsync();
            //MessageBox.Show("Client one connected!\nWaiting for client two...");


            clientTwo = await tcpServer.AcceptTcpClientAsync();
            //MessageBox.Show("Client two connected!");


            var taskFactory = new TaskFactory();
            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            var taskArray = new Task[2];
            taskArray[0] = taskFactory.StartNew(() => MessagingTask(clientOne, clientTwo, tokenSource, cancellationToken), cancellationToken);
            taskArray[1] = taskFactory.StartNew(() => MessagingTask(clientTwo, clientOne, tokenSource, cancellationToken), cancellationToken);
            //stream.Dispose();
            //Task.WaitAny(taskArray);
        }
        void SendToClient(TcpClient client, string txt)
        {

        }
        private static void ServerConsole(CancellationTokenSource tokenSource, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (Console.ReadLine() != "q")
                {

                }

                Console.WriteLine("Closing application...");
                tokenSource.Cancel();
            }
        }

        private void MessagingTask(TcpClient clientOne, TcpClient clientTwo, CancellationTokenSource tokenSource, CancellationToken cancellationToken)
        {

            var buf1 = new byte[1024];

            var clientOneStream = clientOne.GetStream();
            var clientTwoStream = clientTwo.GetStream();
            buf1 = Encoding.Default.GetBytes("Start");
            clientOneStream.Write(buf1, 0, buf1.Length);
            var buf = new byte[1024];
            while (!cancellationToken.IsCancellationRequested)
            {

                int i;
                while ((i = clientOneStream.Read(buf)) != 0)
                {

                    if (buf == null)
                        continue;
                    var data = Encoding.Default.GetString(buf, 0, i);
                    //AddTextToTxt($"{DateTime.Now}: {data}");
                    //MessageBox.Show(data);
                    if (data == "Ready")
                    {
                        if (!Game)
                        {
                            if (White != null)
                            {
                                Black = clientOne;
                                buf = Encoding.Default.GetBytes("Black");
                                clientOneStream.Write(buf);
                            }
                            else if (Black != null)
                            {
                                White = clientOne;
                                buf = Encoding.Default.GetBytes("White");
                                clientOneStream.Write(buf);
                            }
                            else
                            {
                                Random random = new Random();
                                int rnd = random.Next(0, 1);
                                if (rnd == 0)
                                {
                                    White = clientOne;
                                    buf = Encoding.Default.GetBytes("White");
                                    clientOneStream.Write(buf);
                                }
                                else
                                {
                                    Black = clientOne;
                                    buf = Encoding.Default.GetBytes("Black");
                                    clientOneStream.Write(buf);
                                }
                            }
                            if (White != null && Black != null)
                            {
                                Game = true;
                                buf = Encoding.Default.GetBytes("Start Game");
                                clientOneStream.Write(buf);
                                clientTwoStream.Write(buf);
                            }
                        }
                        else
                        {
                            //buf = Encoding.Default.GetBytes("Error");
                            //await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, res.RemoteEndPoint);
                        }
                    }
                    else
                    {
                        List<StepInfo> st = JsonConvert.DeserializeObject<List<StepInfo>>(data);
                        //MessageBox.Show(data);
                        int counter = 0;
                        foreach (var item in st)
                        {
                            if (item.Fight.Item1 != 50 && item.Fight.Item2 != 50)
                                counter++;
                        }
                        if (counter >= 3)
                        {
                            MessageBox.Show($"{data}");
                        }
                        if (counter == 0)
                            StepsWithOutFight++;
                        else
                            StepsWithOutFight = 0;
                        if (White == clientOne)
                        {
                            BlackFigures -= counter;
                            //await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                            clientTwoStream.Write(buf, 0, buf.Length);
                        }
                        else if (Black == clientOne)
                        {
                            WhiteFigures -= counter;
                            clientTwoStream.Write(buf, 0, buf.Length);
                        }
                        if (WhiteFigures == 0)
                        {

                            if (White == clientTwo)
                            {
                                buf = Encoding.Default.GetBytes("Lose");
                                clientTwoStream.Write(buf);
                                buf = Encoding.Default.GetBytes("Win");
                                clientOneStream.Write(buf);
                            }
                            else if (Black == clientTwo)
                            {
                                buf = Encoding.Default.GetBytes("Win");
                                clientTwoStream.Write(buf);
                                buf = Encoding.Default.GetBytes("Lose");
                                clientOneStream.Write(buf);
                            }

                            //await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                            Game = false;
                        }
                        else if (BlackFigures == 0)
                        {
                            if (White == clientTwo)
                            {
                                buf = Encoding.Default.GetBytes("Win");
                                clientTwoStream.Write(buf);
                                buf = Encoding.Default.GetBytes("Lose");
                                clientOneStream.Write(buf);
                            }
                            else if (Black == clientTwo)
                            {
                                buf = Encoding.Default.GetBytes("Lose");
                                clientTwoStream.Write(buf);
                                buf = Encoding.Default.GetBytes("Win");
                                clientOneStream.Write(buf);
                            }
                            Game = false;
                        }
                        else if ((WhiteFigures == 1 && BlackFigures == 1) || StepsWithOutFight == 20)
                        {
                            if (White == clientTwo)
                            {
                                buf = Encoding.Default.GetBytes("Draw");
                                clientTwoStream.Write(buf);
                            }
                            else if (Black == clientTwo)
                            {
                                buf = Encoding.Default.GetBytes("Draw");
                                clientTwoStream.Write(buf);
                            }
                            Game = false;
                        }



                    }
                    buf = new byte[1024];
                }
                if (i == 0)
                {
                    //AddTextToTxt($"Client {clientOne.Client.LocalEndPoint} disconnected");
                    buf = Encoding.Default.GetBytes("disc");
                    clientTwoStream.Write(buf);
                    tokenSource.Cancel();
                    //CloseAllConn();
                    this.Close();
                    break;
                }
            }
        }

        void CloseAllConn()
        {
            clientOne = null;
            clientTwo = null;
            tcpServer.Stop();
            tcpServer = null;
            White = null;
            Black = null;
            BlackFigures = 12;
            WhiteFigures = 12;
            StartServ();
        }
    }
}