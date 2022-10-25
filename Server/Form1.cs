using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Server
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Task task;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (task == null)
            {
                IPAddress address = IPAddress.Parse("192.168.0.26");
                IPEndPoint end = new IPEndPoint(address, 1024);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                socket.Bind(end);
                task = Task.Run(() => ListenerFunc(socket));
            }
        }
        EndPoint White;
        EndPoint Black;
        int WhiteFigures = 16;
        int BlackFigures = 16;
        int StepsWithOutFight = 0;
        bool Game = false;
        async void ListenerFunc(Socket socket)
        {
            IPEndPoint end = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 1024);

            try
            {
                do
                {
                    byte[] buf = new byte[1024];
                    IPEndPoint clientend = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 1025);

                    SocketReceiveFromResult res = await socket.ReceiveFromAsync(new ArraySegment<byte>(buf), SocketFlags.None, clientend);
                    string text = Encoding.Default.GetString(buf, 0, res.ReceivedBytes);
                    if (text == "Ready")
                    {
                        if (!Game)
                        {
                            if (White != null)
                            {
                                Black = res.RemoteEndPoint;
                                buf = Encoding.Default.GetBytes("Black");
                                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                            }
                            else if (Black != null)
                            {
                                White = res.RemoteEndPoint;
                                buf = Encoding.Default.GetBytes("White");
                                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                            }
                            else
                            {
                                Random random = new Random();
                                int rnd = random.Next(0, 1);
                                if (rnd == 0)
                                {
                                    White = res.RemoteEndPoint;
                                    buf = Encoding.Default.GetBytes("White");
                                    await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                                }
                                else
                                {
                                    Black = res.RemoteEndPoint;
                                    buf = Encoding.Default.GetBytes("Black");
                                    await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                                }
                            }
                            if (White != null && Black != null)
                            {
                                Game = true;
                                buf = Encoding.Default.GetBytes("Start Game");
                                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                                buf = Encoding.Default.GetBytes("Start Game");
                                await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                            }
                        }
                        else
                        {
                            buf = Encoding.Default.GetBytes("Error");
                            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, res.RemoteEndPoint);
                        }
                    }
                    else
                    {
                        List<StepInfo> st = JsonSerializer.Deserialize<List<StepInfo>>(text);
                        int counter = 0;
                        foreach(var item in st)
                            if(item.Fight!=(50,50))
                                counter++;
                        if (counter == 0)
                            StepsWithOutFight++;
                        else
                            StepsWithOutFight = 0;
                        if(White == res.RemoteEndPoint)
                        {
                            BlackFigures -= counter;
                            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                        }
                        else if(Black ==res.RemoteEndPoint)
                        {
                            WhiteFigures -= counter;
                            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                        }

                    }
                    if (WhiteFigures == 0)
                    {
                        buf = Encoding.Default.GetBytes("Win");
                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                        buf = Encoding.Default.GetBytes("Lose");
                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                        Game = false;
                    }
                    else if (BlackFigures == 0)
                    {
                        buf = Encoding.Default.GetBytes("Lose");
                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                        buf = Encoding.Default.GetBytes("Win");
                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                        Game = false;
                    }
                    else if ((WhiteFigures == 1 && BlackFigures == 1) || StepsWithOutFight == 20)
                    {
                        buf = Encoding.Default.GetBytes("Draw");
                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, Black);
                        buf = Encoding.Default.GetBytes("Draw");
                        await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, White);
                        Game = false;

                    }

                } while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}