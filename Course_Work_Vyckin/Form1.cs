using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;

namespace Course_Work_Vyckin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trydraw();
        }
        void GetLetters()
        {
            Letters.Add(1, "A");
            Letters.Add(2, "B");
            Letters.Add(3, "C");
            Letters.Add(4, "D");
            Letters.Add(5, "E");
            Letters.Add(6, "F");
            Letters.Add(7, "G");
            Letters.Add(8, "H");
        }
        Dictionary<int, string> Letters = new Dictionary<int, string>();
        bool White = true;

        Bitmap black;
        Bitmap white;
        Bitmap queenwhite;
        Bitmap queenblack;
        Dictionary<Panel, (int, int)> RowAndCow = new Dictionary<Panel, (int, int)>();
        List<Panel> playing_fields = new List<Panel>();
        List<Panel> Forward = new List<Panel>();
        List<Panel> Queens = new List<Panel>();
        string mytag;
        List<StepInfo> StepInfos = new List<StepInfo>();
        private void Form1_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //tableLayoutPanel1.Paint += TableLayoutPanel1_Paint;

            GetLetters();


        }
        void GetTable()
        {
            int counter;
            if (White)
            {
                mytag = "White";
                counter = 0;
            }
            else
            {
                counter = 1;
                mytag = "Black";
            }
            //Label lb = new Label();
            //lb.Text = "HUJ";
            //tableLayoutPanel1.Controls.Add(lb);
            //tableLayoutPanel1.SetRow(lb, 0);
            //tableLayoutPanel1.SetColumn(lb, 3);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {


                    if ((i == 0 && j == 0) || (i == 9 && j == 9) || (i == 0 && j == 9) || (i == 9 && j == 0))
                    {
                        //Label l = new Label();
                        ////l.Text = Letters[j];
                        //l.Size = new Size(70, 70);
                        //tableLayoutPanel1.Controls.Add(l);
                        //tableLayoutPanel1.SetRow(l, i);
                        //tableLayoutPanel1.SetColumn(l, j - 1);
                    }
                    else if (i == 0 || i == 9)
                    {
                        Label l = new Label();
                        l.Text = Letters[j];
                        l.Size = new Size(70, 70);
                        //l.BackColor= Color.BlanchedAlmond;
                        l.Margin = new Padding(0, 0, 0, 0);

                        //Font f = new Font("Segoe", 10, FontStyle.Bold);
                        l.Font = new Font("Segoe", 15, FontStyle.Bold);
                        if (i == 0)
                            l.TextAlign = ContentAlignment.BottomCenter;
                        else
                            l.TextAlign = ContentAlignment.TopCenter;
                        tableLayoutPanel1.Controls.Add(l);
                        tableLayoutPanel1.SetRow(l, i);
                        tableLayoutPanel1.SetColumn(l, j);
                        //tableLayoutPanel1.SetCellPosition(l, new TableLayoutPanelCellPosition(j, i));
                        //this.Text = tableLayoutPanel1.GetCellPosition(l).ToString();
                        //this.Text= tableLayoutPanel1.ColumnCount.ToString();
                    }
                    else if (j == 0 || j == 9)
                    {
                        Label l = new Label();
                        if (White)
                            l.Text = (9 - i).ToString();
                        else
                            l.Text = i.ToString();
                        l.Margin = new Padding(0);
                        l.Size = new Size(70, 70);
                        //l.BorderStyle = BorderStyle.FixedSingle;
                        //Font f = new Font("Segoe", 10, FontStyle.Bold);
                        l.Font = new Font("Segoe", 15, FontStyle.Bold);
                        if (j == 0)
                            l.TextAlign = ContentAlignment.MiddleRight;
                        else
                            l.TextAlign = ContentAlignment.MiddleLeft;
                        tableLayoutPanel1.Controls.Add(l);
                        tableLayoutPanel1.SetRow(l, i);
                        tableLayoutPanel1.SetColumn(l, j);
                    }
                    else if (counter % 2 == 0)
                    {
                        Panel P = new Panel();

                        P.Margin = new Padding(0);
                        P.BackgroundImageLayout = ImageLayout.None;
                        tableLayoutPanel1.Controls.Add(P, j, i);

                        P.BackColor = Color.BlanchedAlmond;
                    }
                    else if (counter % 2 == 1)
                    {

                        Panel P = new Panel();

                        P.Margin = new Padding(0);
                        P.BackgroundImageLayout = ImageLayout.None;
                        tableLayoutPanel1.Controls.Add(P, j, i);
                        P.BackColor = Color.SandyBrown;
                        P.Click += P_Click;
                        playing_fields.Add(P);
                        RowAndCow.Add(P, (j, i));

                    }
                    counter++;
                }
                counter -= 7;
            }
            GetDesk();
        }
        private void TableLayoutPanel1_Paint(object? sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            e.Graphics.FillEllipse(Brushes.Blue, p.Location.X, p.Location.Y, 20, 20);
        }

        void GetDesk()
        {

            Image img = Image.FromFile("Figures/chern.png");
            black = new Bitmap(img, playing_fields.First().Size);
            img.Dispose();
            img = Image.FromFile("Figures/bel.png");
            white = new Bitmap(img, playing_fields.First().Size);
            img.Dispose();
            img = Image.FromFile("Figures/queenbel.png");
            queenwhite = new Bitmap(img, playing_fields.First().Size);
            img.Dispose();
            img = Image.FromFile("Figures/queenchern.png");
            queenblack = new Bitmap(img, playing_fields.First().Size);
            img.Dispose();
            for (int i = 0; i < 12; i++)
            {
                Panel p = playing_fields[i];
                //PictureBox pictureBox = new PictureBox();
                //pictureBox.Size = p.Size;

                //pictureBox.Image = new Bitmap(img, p.Size);
                //img.Dispose();
                //p.Controls.Add(pictureBox);
                if (White)
                {
                    p.BackgroundImage = black;
                    p.BackgroundImage.Tag = "Black";
                }
                else
                {

                    p.BackgroundImage = white;
                    p.BackgroundImage.Tag = "White";
                }

            }
            for (int i = playing_fields.Count - 1; i > playing_fields.Count - 1 - 12; i--)
            {
                Panel p = playing_fields[i];
                if (White)
                {
                    p.BackgroundImage = white;
                    p.BackgroundImage.Tag = "White";
                }
                else
                {
                    p.BackgroundImage = black;
                    p.BackgroundImage.Tag = "Black";
                }


                Forward.Add(p);
                //img.Dispose();

            }
            if (White)
            {
                SwitchEnabledTableLayoutPanel(true);
            }
            else
                SwitchEnabledTableLayoutPanel(false);
        }
        List<(int, int, Panel, Panel)> FightSteps = new List<(int, int, Panel, Panel)>();
        bool IsSelectedFigure;
        Panel SelectedFigure;
        private void P_Click(object? sender, EventArgs e)
        {

            Panel p = sender as Panel;
            if (FightSteps.Count > 0)
            {
                if (IsSelectedFigure)
                {
                    FightSteps = MandatoryFight();
                    foreach (var item in FightSteps.Where(t => t.Item3 == SelectedFigure))
                    {

                        (int, int) b;
                        //(int, int) c;
                        b.Item1 = item.Item1;
                        b.Item2 = item.Item2;
                        if (b == RowAndCow[p])
                        {
                            StepInfo inf = new StepInfo() { From = RowAndCow[SelectedFigure], To = b, Fight = RowAndCow[item.Item4] };
                            StepInfos.Add(inf);
                            FightFigures(p, SelectedFigure);
                            Forward.Remove(SelectedFigure);
                            Forward.Add(p);
                            if (Queens.Contains(SelectedFigure))
                            {
                                Queens.Remove(SelectedFigure);
                                Queens.Add(p);
                            }
                            if (b.Item2 == 1)
                            {
                                Queens.Add(p);
                                if (White)
                                {
                                    p.BackgroundImage = queenwhite;
                                    p.BackgroundImage.Tag = "White";
                                }
                                else
                                {
                                    p.BackgroundImage = queenblack;
                                    p.BackgroundImage.Tag = "Black";
                                }
                            }
                            //c.Item1 = item.Item1 - b.Item1;
                            //c.Item2 = item.Item2 - b.Item2;
                            //Panel P = RowAndCow.Where(t => t.Value.Item1 == item.Item1 - c.Item1 + 1 && t.Value.Item2 == item.Item2 - c.Item2 + 1).First().Key;
                            //P.BackgroundImage = null;
                            item.Item4.BackgroundImage = null;


                            var buff = CheckThisFight(p);
                            if (buff.Count > 0)
                            {
                                buff = null;
                                SelectedFigure = p;
                                P_Click(p, e);

                            }
                            else
                            {
                                IsSelectedFigure = false;
                                SelectedFigure = null;
                                SwitchEnabledTableLayoutPanel(false);
                                SendToServ(StepInfos);
                                StepInfos.Clear();
                            }
                        }
                    }

                }
                else
                {
                    foreach (var item in FightSteps)
                    {
                        if (p == item.Item3)
                        {
                            SelectedFigure = p;
                            IsSelectedFigure = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                if (IsSelectedFigure)
                {

                    if (p.BackgroundImage != null && p.BackgroundImage.Tag == SelectedFigure.BackgroundImage.Tag)
                    {
                        SelectedFigure = p;
                    }
                    else if (p.BackgroundImage != SelectedFigure.BackgroundImage && p.BackgroundImage != null)
                    {
                        //MessageBox.Show("sdfs");
                    }
                    else
                    {
                        (int, int) buf = RowAndCow[p];
                        (int, int) selected = RowAndCow[SelectedFigure];
                        if (Forward.Contains(SelectedFigure))
                        {
                            if (Queens.Contains(SelectedFigure))
                            {
                                List<(int, int)> Steps = new List<(int, int)>();
                                Steps = CheckForQueen(SelectedFigure);

                                if (Steps.Contains(buf))
                                {
                                    StepInfo info = new StepInfo() { From = RowAndCow[SelectedFigure], To = RowAndCow[p] };
                                    StepInfos.Add(info);
                                    SwapFigures(p, SelectedFigure);
                                    Forward.Remove(SelectedFigure);
                                    Forward.Add(p);
                                    Queens.Remove(SelectedFigure);
                                    Queens.Add(p);
                                }
                                else
                                {
                                    SelectedFigure = null;
                                    IsSelectedFigure = false;
                                }
                                SwitchEnabledTableLayoutPanel(false);
                                SendToServ(StepInfos);
                                StepInfos.Clear();
                            }
                            else
                            {
                                if ((buf.Item1 == selected.Item1 - 1 && buf.Item2 == selected.Item2 - 1) || (buf.Item1 == selected.Item1 + 1 && buf.Item2 == selected.Item2 - 1))
                                {
                                    StepInfo info = new StepInfo() { From = RowAndCow[SelectedFigure], To = RowAndCow[p] };
                                    StepInfos.Add(info);
                                    SwapFigures(p, SelectedFigure);
                                    Forward.Remove(SelectedFigure);
                                    Forward.Add(p);
                                    this.Text = $"{buf.Item1} - {buf.Item2}";
                                    if (buf.Item2 == 1)
                                    {
                                        Queens.Add(p);
                                        if (White)
                                        {
                                            p.BackgroundImage = queenwhite;
                                            p.BackgroundImage.Tag = "White";
                                        }
                                        else
                                        {
                                            p.BackgroundImage = queenblack;
                                            p.BackgroundImage.Tag = "Black";
                                        }
                                    }
                                    //FightSteps = MandatoryFight();
                                    SwitchEnabledTableLayoutPanel(false);
                                    SendToServ(StepInfos);
                                    StepInfos.Clear();
                                }
                                else
                                {
                                    SelectedFigure = null;
                                    IsSelectedFigure = false;
                                }
                            }
                        }
                        //else
                        //{
                        //    if ((buf.Item1 == selected.Item1 - 1 && buf.Item2 == selected.Item2 + 1) || (buf.Item1 == selected.Item1 + 1 && buf.Item2 == selected.Item2 + 1))
                        //    {
                        //        SwapFigures(p, SelectedFigure);

                        //    }
                        //    else
                        //    {
                        //        SelectedFigure = null;
                        //        IsSelectedFigure = false;
                        //    }
                        //}

                    }
                }
                else
                {
                    if (p.BackgroundImage != null && p.BackgroundImage.Tag == mytag)
                    {
                        IsSelectedFigure = true;
                        SelectedFigure = p;
                        //p.Invalidate();
                        //if (Queens.Contains(SelectedFigure))
                        //{
                        //    List<(int, int)> Steps = CheckForQueen(SelectedFigure);
                        //    DrawElipses(Steps);
                        //}
                    }
                }
            }
            FightSteps = null;
            FightSteps = MandatoryFight();

        }
        void trydraw()
        {
            Graphics g = this.CreateGraphics();
            Rectangle r = new Rectangle(750, 200, 50, 50);
            g.FillEllipse(Brushes.AntiqueWhite, r);
            g.Dispose();
        }
        void DrawElipses(List<(int, int)> steps)
        {
            foreach (var step in steps)
            {
                Panel thispanel = RowAndCow.Where(t => t.Value == step).FirstOrDefault().Key;
                Graphics g = tableLayoutPanel1.CreateGraphics();
                Rectangle r = new Rectangle(thispanel.Location.X + thispanel.Width / 2 - 1, thispanel.Location.Y - thispanel.Height / 2 - 1, 50, 50);
                g.DrawEllipse(Pens.Aquamarine, r);
            }
        }
        void FightFigures(Panel p, Panel sel)
        {
            p.BackgroundImage = SelectedFigure.BackgroundImage;
            SelectedFigure.BackgroundImage = null;

        }

        void SwapFigures(Panel p, Panel sel)
        {
            p.BackgroundImage = SelectedFigure.BackgroundImage;
            SelectedFigure.BackgroundImage = null;
            IsSelectedFigure = false;
            SelectedFigure = null;
        }
        bool CheckForDoubleFight(Panel queen, Panel killed)
        {
            (int, int) buf = RowAndCow[queen];
            List<(int, int)> Steps = new List<(int, int)>();
            (int, int) sel = buf;
            List<Panel> Canfight = new List<Panel>();
            while ((sel.Item1 >= 0 && sel.Item2 >= 0))
            {
                sel.Item1--;
                sel.Item2--;
                Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                if (mbfight != null)
                {
                    if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != queen.BackgroundImage.Tag && mbfight != killed)
                    {
                        Canfight.Add(mbfight);

                    }
                    else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                    {
                        return true;
                    }
                }
            }
            Canfight.Clear();
            sel = buf;
            while ((sel.Item1 <= 8 && sel.Item2 <= 8))
            {
                sel.Item1++;
                sel.Item2++;
                Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                if (mbfight != null)
                {
                    if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != queen.BackgroundImage.Tag && mbfight != killed)
                    {
                        Canfight.Add(mbfight);

                    }
                    else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                    {
                        return true;
                    }
                }
            }
            Canfight.Clear();
            sel = buf;
            while ((sel.Item1 >= 0 && sel.Item2 <= 8))
            {
                sel.Item1--;
                sel.Item2++;
                Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                if (mbfight != null)
                {
                    if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != queen.BackgroundImage.Tag && mbfight != killed)
                    {
                        Canfight.Add(mbfight);

                    }
                    else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                    {
                        return true;
                    }
                }
            }
            Canfight.Clear();
            sel = buf;
            while ((sel.Item1 <= 8 && sel.Item2 >= 0))
            {
                sel.Item1++;
                sel.Item2--;
                Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                if (mbfight != null)
                {
                    if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != queen.BackgroundImage.Tag && mbfight != killed)
                    {
                        Canfight.Add(mbfight);

                    }
                    else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        List<(int, int)> CheckForQueen(Panel P)
        {
            List<(int, int)> result = new List<(int, int)>();
            (int, int) Sel = RowAndCow[P];
            (int, int) buf = Sel;
            while (buf.Item1 >= 0 && buf.Item2 >= 0)
            {
                buf.Item1--;
                buf.Item2--;
                Panel step = RowAndCow.Where(t => t.Value == (buf.Item1, buf.Item2)).FirstOrDefault().Key;
                if (step != null)
                {
                    if (step.BackgroundImage == null)
                    {
                        result.Add((buf.Item1, buf.Item2));
                    }
                    else if (step.BackgroundImage.Tag == P.BackgroundImage.Tag)
                    {
                        break;
                    }
                    else
                    {
                        //MessageBox.Show("Need Fight");
                        break;
                    }
                }
            }
            buf = Sel;
            while (buf.Item1 <= 8 && buf.Item2 <= 8)
            {
                buf.Item1++;
                buf.Item2++;
                Panel step = RowAndCow.Where(t => t.Value == (buf.Item1, buf.Item2)).FirstOrDefault().Key;
                if (step != null)
                {
                    if (step.BackgroundImage == null)
                    {
                        result.Add((buf.Item1, buf.Item2));
                    }
                    else if (step.BackgroundImage.Tag == P.BackgroundImage.Tag)
                    {
                        break;
                    }
                    else
                    {
                        //MessageBox.Show("Need Fight");
                        break;
                    }
                }
            }
            buf = Sel;
            while (buf.Item1 >= 0 && buf.Item2 <= 8)
            {
                buf.Item1--;
                buf.Item2++;
                Panel step = RowAndCow.Where(t => t.Value == (buf.Item1, buf.Item2)).FirstOrDefault().Key;
                if (step != null)
                {
                    if (step.BackgroundImage == null)
                    {
                        result.Add((buf.Item1, buf.Item2));
                    }
                    else if (step.BackgroundImage.Tag == P.BackgroundImage.Tag)
                    {
                        break;
                    }
                    else
                    {
                        //MessageBox.Show("Need Fight");
                        break;
                    }
                }
            }
            buf = Sel;
            while (buf.Item1 <= 8 && buf.Item2 >= 0)
            {
                buf.Item1++;
                buf.Item2--;
                Panel step = RowAndCow.Where(t => t.Value == (buf.Item1, buf.Item2)).FirstOrDefault().Key;
                if (step != null)
                {
                    if (step.BackgroundImage == null)
                    {
                        result.Add((buf.Item1, buf.Item2));
                    }
                    else if (step.BackgroundImage.Tag == P.BackgroundImage.Tag)
                    {
                        break;
                    }
                    else
                    {
                        //MessageBox.Show("Need Fight");
                        break;
                    }
                }
            }
            return result;
        }
        List<(int, int, Panel)> CheckThisFight(Panel P)
        {
            List<(int, int, Panel)> ls = new List<(int, int, Panel)>();
            (int, int) buf = RowAndCow[P];
            List<Panel> Canfight = new List<Panel>();
            List<Panel> Candef = new List<Panel>();
            List<(int, int)> Steps = new List<(int, int)>();
            if (Queens.Contains(P))
            {
                Panel item = P;
                (int, int) sel = buf;
                int lscount = ls.Count;
                while ((sel.Item1 >= 0 && sel.Item2 >= 0))
                {
                    sel.Item1--;
                    sel.Item2--;
                    Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                    if (mbfight != null)
                    {
                        if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count == 0)
                        {
                            Canfight.Add(mbfight);
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                        {
                            Canfight.Clear();
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                        {
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                        {
                            (int, int, Panel) b;
                            b.Item1 = sel.Item1;
                            b.Item2 = sel.Item2;
                            b.Item3 = item;
                            //b.Item4 = Canfight[Canfight.Count - 1];
                            ls.Add(b);
                        }
                    }
                }
                Canfight.Clear();
                sel = buf;
                lscount = ls.Count;
                while ((sel.Item1 <= 8 && sel.Item2 <= 8))
                {
                    sel.Item1++;
                    sel.Item2++;
                    Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                    if (mbfight != null)
                    {
                        if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count == 0)
                        {
                            Canfight.Add(mbfight);
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                        {
                            Canfight.Clear();
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                        {
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                        {
                            (int, int, Panel) b;
                            b.Item1 = sel.Item1;
                            b.Item2 = sel.Item2;
                            b.Item3 = item;
                            //b.Item4 = Canfight[Canfight.Count - 1];
                            ls.Add(b);
                        }
                    }
                }
                Canfight.Clear();
                sel = buf;
                lscount = ls.Count;
                while ((sel.Item1 >= 0 && sel.Item2 <= 8))
                {
                    sel.Item1--;
                    sel.Item2++;
                    Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                    if (mbfight != null)
                    {
                        if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count() == 0)
                        {
                            Canfight.Add(mbfight);
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                        {
                            Canfight.Clear();
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                        {
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                        {
                            (int, int, Panel) b;
                            b.Item1 = sel.Item1;
                            b.Item2 = sel.Item2;
                            b.Item3 = item;
                            //b.Item4 = Canfight[Canfight.Count - 1];
                            ls.Add(b);
                        }
                    }
                }
                Canfight.Clear();
                sel = buf;
                lscount = ls.Count;
                while ((sel.Item1 <= 8 && sel.Item2 >= 0))
                {
                    sel.Item1++;
                    sel.Item2--;
                    Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                    if (mbfight != null)
                    {
                        if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count == 0)
                        {
                            Canfight.Add(mbfight);
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                        {
                            Canfight.Clear();
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                        {
                            break;
                        }
                        else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                        {
                            (int, int, Panel) b;
                            b.Item1 = sel.Item1;
                            b.Item2 = sel.Item2;
                            b.Item3 = item;
                            //b.Item4 = Canfight[Canfight.Count - 1];
                            ls.Add(b);
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 2; i++)
                {
                    if (i == 1)
                    {
                        Canfight.Add(RowAndCow.Where(t => t.Value == ((buf.Item1 - i), (buf.Item2 - i))).FirstOrDefault().Key);
                        Canfight.Add(RowAndCow.Where(t => t.Value == (buf.Item1 - i, buf.Item2 + i)).FirstOrDefault().Key);
                        Canfight.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 - i)).FirstOrDefault().Key);
                        Canfight.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 + i)).FirstOrDefault().Key);
                    }
                    else
                    {

                        Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 - i, buf.Item2 - i)).FirstOrDefault().Key);
                        Steps.Add((buf.Item1 - i, buf.Item2 - i));
                        Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 - i, buf.Item2 + i)).FirstOrDefault().Key);
                        Steps.Add((buf.Item1 - i, buf.Item2 + i));
                        Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 - i)).FirstOrDefault().Key);
                        Steps.Add((buf.Item1 + i, buf.Item2 - i));
                        Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 + i)).FirstOrDefault().Key);
                        Steps.Add((buf.Item1 + i, buf.Item2 + i));
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (Canfight[i] != null)
                    {
                        if (Canfight[i].BackgroundImage != P.BackgroundImage && Canfight[i].BackgroundImage != null)
                        {
                            if (Candef[i] == null || Canfight[i].BackgroundImage == Candef[i].BackgroundImage || P.BackgroundImage == Candef[i].BackgroundImage)
                            {
                                //MessageBox.Show(Candef[i].BackgroundImage.Tag.ToString());
                                //MessageBox.Show(Canfight[i].BackgroundImage.Tag.ToString());
                                //MessageBox.Show(item.BackgroundImage.Tag.ToString());
                                //MessageBox.Show("candef");


                            }
                            else
                            {
                                //MessageBox.Show("can fight");
                                if (Candef[i].BackgroundImage != null)
                                {
                                    if (Candef[i].BackgroundImage.Tag != P.BackgroundImage.Tag || Canfight[i].BackgroundImage.Tag != Candef[i].BackgroundImage.Tag)
                                    {
                                        (int, int, Panel) b;
                                        b.Item1 = Steps[i].Item1;
                                        b.Item2 = Steps[i].Item2;
                                        b.Item3 = P;
                                        ls.Add(b);
                                    }
                                }
                                else
                                {
                                    (int, int, Panel) b;
                                    b.Item1 = Steps[i].Item1;
                                    b.Item2 = Steps[i].Item2;
                                    b.Item3 = P;
                                    ls.Add(b);
                                }

                            }
                        }
                    }
                }
            }
            return ls;
        }
        List<(int, int, Panel, Panel)> MandatoryFight()
        {
            List<(int, int, Panel, Panel)> ls = new List<(int, int, Panel, Panel)>();
            List<(int, int, Panel, Panel)> doublefight = new List<(int, int, Panel, Panel)>();
            foreach (var item in Forward)
            {
                if (item.BackgroundImage != null)
                {
                    (int, int) buf = RowAndCow[item];

                    List<Panel> Canfight = new List<Panel>();
                    List<Panel> Candef = new List<Panel>();
                    List<(int, int)> Steps = new List<(int, int)>();
                    if (Queens.Contains(item))
                    {
                        (int, int) sel = buf;
                        int lscount = ls.Count;
                        while ((sel.Item1 >= 0 && sel.Item2 >= 0))
                        {
                            sel.Item1--;
                            sel.Item2--;
                            Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                            if (mbfight != null)
                            {
                                if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count == 0)
                                {
                                    Canfight.Add(mbfight);
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                                {
                                    Canfight.Clear();
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                                {
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                                {
                                    (int, int, Panel, Panel) b;
                                    var bufimg = mbfight.BackgroundImage;
                                    mbfight.BackgroundImage = item.BackgroundImage;
                                    if (CheckForDoubleFight(mbfight, Canfight[Canfight.Count - 1]))
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        doublefight.Add(b);
                                    }
                                    else
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        ls.Add(b);
                                    }
                                    mbfight.BackgroundImage = bufimg;

                                }
                            }
                        }
                        sel = buf;
                        Canfight.Clear();
                        lscount = ls.Count;
                        while ((sel.Item1 <= 8 && sel.Item2 <= 8))
                        {
                            sel.Item1++;
                            sel.Item2++;
                            Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                            if (mbfight != null)
                            {
                                if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count == 0)
                                {
                                    Canfight.Add(mbfight);
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                                {
                                    Canfight.Clear();
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                                {
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                                {
                                    (int, int, Panel, Panel) b;
                                    var bufimg = mbfight.BackgroundImage;
                                    mbfight.BackgroundImage = item.BackgroundImage;
                                    if (CheckForDoubleFight(mbfight, Canfight[Canfight.Count - 1]))
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        doublefight.Add(b);
                                    }
                                    else
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        ls.Add(b);
                                    }
                                    mbfight.BackgroundImage = bufimg;
                                }
                            }
                        }
                        Canfight.Clear();
                        lscount = ls.Count;
                        sel = buf;
                        while ((sel.Item1 >= 0 && sel.Item2 <= 8))
                        {
                            sel.Item1--;
                            sel.Item2++;
                            Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                            if (mbfight != null)
                            {
                                if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count() == 0)
                                {
                                    Canfight.Add(mbfight);
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                                {
                                    Canfight.Clear();
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                                {
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                                {
                                    (int, int, Panel, Panel) b;
                                    var bufimg = mbfight.BackgroundImage;
                                    mbfight.BackgroundImage = item.BackgroundImage;
                                    if (CheckForDoubleFight(mbfight, Canfight[Canfight.Count - 1]))
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        doublefight.Add(b);
                                    }
                                    else
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        ls.Add(b);
                                    }
                                    mbfight.BackgroundImage = bufimg;
                                }
                            }
                        }
                        Canfight.Clear();
                        lscount = ls.Count;
                        sel = buf;
                        while ((sel.Item1 <= 8 && sel.Item2 >= 0))
                        {
                            sel.Item1++;
                            sel.Item2--;
                            Panel mbfight = RowAndCow.Where(t => t.Value == (sel.Item1, sel.Item2)).FirstOrDefault().Key;
                            if (mbfight != null)
                            {
                                if (mbfight.BackgroundImage != null && mbfight.BackgroundImage.Tag != item.BackgroundImage.Tag && Canfight.Count == 0)
                                {
                                    Canfight.Add(mbfight);
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count == lscount)
                                {
                                    Canfight.Clear();
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage != null && ls.Count > lscount)
                                {
                                    break;
                                }
                                else if (Canfight.Count > 0 && mbfight.BackgroundImage == null)
                                {
                                    (int, int, Panel, Panel) b;
                                    var bufimg = mbfight.BackgroundImage;
                                    mbfight.BackgroundImage = item.BackgroundImage;
                                    if (CheckForDoubleFight(mbfight, Canfight[Canfight.Count - 1]))
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        doublefight.Add(b);
                                    }
                                    else
                                    {
                                        b.Item1 = sel.Item1;
                                        b.Item2 = sel.Item2;
                                        b.Item3 = item;
                                        b.Item4 = Canfight[Canfight.Count - 1];
                                        ls.Add(b);
                                    }
                                    mbfight.BackgroundImage = bufimg;
                                }
                            }
                        }
                    }
                    else
                    {

                        for (int i = 1; i <= 2; i++)
                        {
                            if (i == 1)
                            {
                                Canfight.Add(RowAndCow.Where(t => t.Value == ((buf.Item1 - i), (buf.Item2 - i))).FirstOrDefault().Key);
                                Canfight.Add(RowAndCow.Where(t => t.Value == (buf.Item1 - i, buf.Item2 + i)).FirstOrDefault().Key);
                                Canfight.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 - i)).FirstOrDefault().Key);
                                Canfight.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 + i)).FirstOrDefault().Key);
                            }
                            else
                            {

                                Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 - i, buf.Item2 - i)).FirstOrDefault().Key);
                                Steps.Add((buf.Item1 - i, buf.Item2 - i));
                                Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 - i, buf.Item2 + i)).FirstOrDefault().Key);
                                Steps.Add((buf.Item1 - i, buf.Item2 + i));
                                Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 - i)).FirstOrDefault().Key);
                                Steps.Add((buf.Item1 + i, buf.Item2 - i));
                                Candef.Add(RowAndCow.Where(t => t.Value == (buf.Item1 + i, buf.Item2 + i)).FirstOrDefault().Key);
                                Steps.Add((buf.Item1 + i, buf.Item2 + i));
                            }
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            if (Canfight[i] != null)
                            {
                                if (Canfight[i].BackgroundImage != item.BackgroundImage && Canfight[i].BackgroundImage != null && Canfight[i].BackgroundImage.Tag != item.BackgroundImage.Tag)
                                {
                                    if (Candef[i] == null || Canfight[i].BackgroundImage == Candef[i].BackgroundImage || item.BackgroundImage == Candef[i].BackgroundImage)
                                    {
                                        //MessageBox.Show(Candef[i].BackgroundImage.Tag.ToString());
                                        //MessageBox.Show(Canfight[i].BackgroundImage.Tag.ToString());
                                        //MessageBox.Show(item.BackgroundImage.Tag.ToString());
                                        //MessageBox.Show("candef");


                                    }
                                    else
                                    {
                                        //MessageBox.Show("can fight");
                                        if (Candef[i].BackgroundImage != null)
                                        {
                                            if (Candef[i].BackgroundImage.Tag != item.BackgroundImage.Tag || Canfight[i].BackgroundImage.Tag != Candef[i].BackgroundImage.Tag)
                                            {
                                                (int, int, Panel, Panel) b;
                                                b.Item1 = Steps[i].Item1;
                                                b.Item2 = Steps[i].Item2;
                                                b.Item3 = item;
                                                b.Item4 = Canfight[i];
                                                ls.Add(b);
                                            }
                                        }
                                        else
                                        {
                                            (int, int, Panel, Panel) b;
                                            b.Item1 = Steps[i].Item1;
                                            b.Item2 = Steps[i].Item2;
                                            b.Item3 = item;
                                            b.Item4 = Canfight[i];
                                            ls.Add(b);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (doublefight.Count > 0)
                return doublefight;
            else
                return ls;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Rectangle r = new Rectangle(750, 200, 50, 50);
            e.Graphics.FillEllipse(Brushes.AntiqueWhite, r);
        }

        IPEndPoint ServerIP = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 1024);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
        IPAddress MyIp = IPAddress.Any;
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
            Random rnd = new Random();
            int port = rnd.Next(10000, 64000);
            //IPEndPoint end = new IPEndPoint(IPAddress.Parse(GetLocalIP()), port);
            IPEndPoint end = new IPEndPoint(MyIp, port);
            socket.Bind(end);
            Task task = Task.Run(() => ListenerFunc(socket));
            Task task1 = Task.Run(() => SendToServ("Ready"));
            button1.Enabled = false;

        }

        async void SendToServ(string text)
        {
            byte[] buf = new byte[1024];
            buf = Encoding.Default.GetBytes(text);
            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, ServerIP);

        }
        async void SendToServ(List<StepInfo> ls)
        {
            byte[] buf = new byte[1024];
            //string text = JsonSerializer.Serialize(ls);
            string text = JsonConvert.SerializeObject(ls,Formatting.Indented);
            //MessageBox.Show(text);
            buf = Encoding.Default.GetBytes(text);
            await socket.SendToAsync(new ArraySegment<byte>(buf), SocketFlags.None, ServerIP);
        }
        async void ListenerFunc(Socket socket)
        {

            try
            {
                do
                {
                    byte[] buf = new byte[1024];

                    SocketReceiveFromResult res = await socket.ReceiveFromAsync(new ArraySegment<byte>(buf), SocketFlags.None, ServerIP);
                    string text = Encoding.Default.GetString(buf, 0, res.ReceivedBytes);
                    if (text == "Black")
                        White = false;
                    else if (text == "White")
                        White = true;
                    else if (text == "Start Game")
                    {
                        MessageBox.Show($"{text}");
                        if (this.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)(() =>
                            {
                                GetTable();

                            }
                            ));
                        }
                        else
                        {
                            GetTable();
                        }

                    }
                    else if (text == "Win")
                        MessageBox.Show("You are win!!!");
                    else if (text == "Lose")
                        MessageBox.Show("You are lose!!!");
                    else if (text == "Draw")
                        MessageBox.Show("Draw!!!");
                    else if (text == "Error")
                        MessageBox.Show("Error, try next time");
                    else
                    {
                        List<StepInfo> info = JsonConvert.DeserializeObject<List<StepInfo>>(text);
                        TranslateStep(info);
                        FightSteps= MandatoryFight();
                        Thread.Sleep(1500);
                        if (this.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)(() =>
                            {
                                SwitchEnabledTableLayoutPanel(true);

                            }
                            ));
                        }
                        else
                        {
                            SwitchEnabledTableLayoutPanel(true);
                        }
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
        void ServerStep(Panel from, Panel to)
        {
            to.BackgroundImage = from.BackgroundImage;
            from.BackgroundImage = null;
        }
        void TranslateStep(List<StepInfo> ls)
        {
            foreach (var item in ls)
            {
                StringBuilder sb = new StringBuilder();
                //sb.AppendLine(item.ToString());
                //sb.AppendLine(item.From.ToString());
                //sb.AppendLine(item.To.ToString());
                //sb.AppendLine(item.Fight.ToString());
                //MessageBox.Show(sb.ToString());
                Panel from = RowAndCow.Where(t => t.Value.Item1 == item.From.Item1&&t.Value.Item2==(8-item.From.Item2+1)).FirstOrDefault().Key;
                Panel to = RowAndCow.Where(t => t.Value.Item1 == item.To.Item1 && t.Value.Item2 == (8 - item.To.Item2 + 1)).FirstOrDefault().Key;
                if (item.Fight != (50, 50))
                {
                    Panel fight = RowAndCow.Where(t => t.Value.Item1 == item.Fight.Item1 && t.Value.Item2 == (8 - item.Fight.Item2 + 1)).FirstOrDefault().Key;
                    if (fight != null)
                    {
                        fight.BackgroundImage = null;
                    }
                }
                if (from != null && to != null)
                {
                    ServerStep(from, to);

                }
                Thread.Sleep(300);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyIp = IPAddress.Parse(GetLocalIP());
            button2.Enabled = false;
        }
        void SwitchEnabledTableLayoutPanel(bool b)
        {
            tableLayoutPanel1.Enabled = b;
        }
    }

}