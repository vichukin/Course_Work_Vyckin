
namespace Course_Work_Vyckin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


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
        Dictionary<int,string> Letters = new Dictionary<int,string>();
        bool White = true;

        Bitmap black;
        Bitmap white;
        Dictionary<Panel, (int, int)> RowAndCow = new Dictionary<Panel, (int, int)>();
        List<Panel> playing_fields = new List<Panel>();
        List<Panel> Forward = new List<Panel>();
        private void Form1_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //tableLayoutPanel1.Paint += TableLayoutPanel1_Paint;

            GetLetters();
            int counter;
            if (White)
                counter = 0;
            else 
                counter = 1;
            //Label lb = new Label();
            //lb.Text = "HUJ";
            //tableLayoutPanel1.Controls.Add(lb);
            //tableLayoutPanel1.SetRow(lb, 0);
            //tableLayoutPanel1.SetColumn(lb, 3);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {


                    if ((i == 0 && j == 0) || (i == 9 && j == 9)||(i==0&&j==9)||(i==9&&j==0))
                    {
                        //Label l = new Label();
                        ////l.Text = Letters[j];
                        //l.Size = new Size(70, 70);
                        //tableLayoutPanel1.Controls.Add(l);
                        //tableLayoutPanel1.SetRow(l, i);
                        //tableLayoutPanel1.SetColumn(l, j - 1);
                    }
                    else if(i == 0||i==9)
                    {
                        Label l = new Label();
                        l.Text = Letters[j];
                        l.Size = new Size(70, 70);
                        //l.BackColor= Color.BlanchedAlmond;
                        l.Margin = new Padding(0,0,0,0);
                        
                        //Font f = new Font("Segoe", 10, FontStyle.Bold);
                        l.Font = new Font("Segoe", 15, FontStyle.Bold);
                        if (i == 0)
                            l.TextAlign = ContentAlignment.BottomCenter;
                        else
                            l.TextAlign = ContentAlignment.TopCenter;
                        tableLayoutPanel1.Controls.Add(l);
                        tableLayoutPanel1.SetRow(l, i);
                        tableLayoutPanel1.SetColumn(l, j );
                        //tableLayoutPanel1.SetCellPosition(l, new TableLayoutPanelCellPosition(j, i));
                        //this.Text = tableLayoutPanel1.GetCellPosition(l).ToString();
                        //this.Text= tableLayoutPanel1.ColumnCount.ToString();
                    }
                    else if(j==0 || j == 9)
                    {
                        Label l = new Label();
                        if (White)
                            l.Text = (9 - i).ToString();
                        else
                            l.Text = i.ToString();
                        l.Margin = new Padding(0);
                        l.Size = new Size(70,70);
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
                    p.BackgroundImage.Tag = "white";
                }

            }
            for (int i = playing_fields.Count - 1; i > playing_fields.Count - 1 - 12; i--)
            {
                Panel p = playing_fields[i];
                if (White)
                {
                    p.BackgroundImage = white;
                    p.BackgroundImage.Tag = "white";
                }
                else
                {
                    p.BackgroundImage = black;
                    p.BackgroundImage.Tag = "Black";
                }
                
                
                Forward.Add(p);
                //img.Dispose();

            }
            img.Dispose();
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
                            FightFigures(p, SelectedFigure);
                            Forward.Remove(SelectedFigure);
                            Forward.Add(p);

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

                    if (p.BackgroundImage == SelectedFigure.BackgroundImage)
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
                            if ((buf.Item1 == selected.Item1 - 1 && buf.Item2 == selected.Item2 - 1) || (buf.Item1 == selected.Item1 + 1 && buf.Item2 == selected.Item2 - 1))
                            {
                                SwapFigures(p, SelectedFigure);
                                Forward.Remove(SelectedFigure);
                                Forward.Add(p);
                                //FightSteps = MandatoryFight();
                            }
                            else
                            {
                                SelectedFigure = null;
                                IsSelectedFigure = false;
                            }
                        }
                        else
                        {
                            if ((buf.Item1 == selected.Item1 - 1 && buf.Item2 == selected.Item2 + 1) || (buf.Item1 == selected.Item1 + 1 && buf.Item2 == selected.Item2 + 1))
                            {
                                SwapFigures(p, SelectedFigure);

                            }
                            else
                            {
                                SelectedFigure = null;
                                IsSelectedFigure = false;
                            }
                        }

                    }
                }
                else
                {
                    if (p.BackgroundImage != null)
                    {
                        IsSelectedFigure = true;
                        SelectedFigure = p;
                        p.Invalidate();
                    }
                }
            }
            FightSteps = null;
            FightSteps = MandatoryFight();
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
        List<(int, int, Panel)> CheckThisFight(Panel P)
        {
            List<(int, int, Panel)> ls = new List<(int, int, Panel)>();
            (int, int) buf = RowAndCow[P];
            List<Panel> Canfight = new List<Panel>();
            List<Panel> Candef = new List<Panel>();
            List<(int, int)> Steps = new List<(int, int)>();
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
                            (int, int, Panel) b;
                            b.Item1 = Steps[i].Item1;
                            b.Item2 = Steps[i].Item2;
                            b.Item3 = P;
                            ls.Add(b);
                        }
                    }
                }
            }
            return ls;
        }
        List<(int, int, Panel, Panel)> MandatoryFight()
        {
            List<(int, int, Panel, Panel)> ls = new List<(int, int, Panel, Panel)>();

            foreach (var item in Forward)
            {
                if (item.BackgroundImage != null)
                {
                    (int, int) buf = RowAndCow[item];
                    List<Panel> Canfight = new List<Panel>();
                    List<Panel> Candef = new List<Panel>();
                    List<(int, int)> Steps = new List<(int, int)>();
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
                            if (Canfight[i].BackgroundImage != item.BackgroundImage && Canfight[i].BackgroundImage != null)
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
            return ls;
        }
    }
}