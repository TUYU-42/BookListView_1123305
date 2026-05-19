using System.Drawing;
using System.Drawing.Drawing2D;

namespace BookListView
{
    public class frmBooks : Form
    {
        // ══════════════════════════════════════════
        //  共用成員變數（對應投影片 p.13）
        // ══════════════════════════════════════════
        string[] b_name = { "三國演義", "西遊記", "唐詩三百首", "楚辭",
                            "西廂記", "水滸傳", "紅樓夢", "牡丹亭" };

        string[] author = { "羅貫中", "吳承恩", "孫洙", "劉向",
                            "王實甫", "施耐庵", "曹雪芹", "湯顯祖" };

        string[] kind = { "章回小說", "章回小說", "詩選", "詩歌",
                          "戲曲", "章回小說", "章回小說", "戲曲" };

        // ══════════════════════════════════════════
        //  控制項宣告（對應投影片 p.12 佈局圖）
        // ══════════════════════════════════════════
        private ListView lvwBooks = null!;
        private ComboBox cmbView = null!;
        private ListBox lstBorrow = null!;
        private GroupBox grpView = null!;
        private GroupBox grpBorrow = null!;
        private Panel pnlTools = null!;
        private ImageList imgL = null!;
        private ImageList imgS = null!;

        // ── 美化用色彩 ──
        private static readonly Color HeaderBg = Color.FromArgb(44, 62, 80);
        private static readonly Color AccentColor = Color.FromArgb(52, 152, 219);
        private static readonly Color WarmAccent = Color.FromArgb(230, 126, 34);
        private static readonly Color SurfaceBg = Color.FromArgb(236, 240, 241);
        private static readonly Color CardBg = Color.White;
        private static readonly Color TextPrimary = Color.FromArgb(44, 62, 80);
        private static readonly Color TextSecondary = Color.FromArgb(127, 140, 141);
        private static readonly Color BorderLight = Color.FromArgb(189, 195, 199);

        public frmBooks()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // ── 表單基本設定 ──
            this.Text = "📚 圖書管理";
            this.Size = new Size(820, 560);
            this.MinimumSize = new Size(780, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SurfaceBg;
            this.Font = new Font("Microsoft JhengHei UI", 9.5f);

            // ══════════════════════════════════════════
            //  ImageList 設定（對應投影片 p.12）
            // ══════════════════════════════════════════
            imgL = new ImageList();
            imgL.ImageSize = new Size(90, 120);
            imgL.ColorDepth = ColorDepth.Depth32Bit;

            imgS = new ImageList();
            imgS.ImageSize = new Size(15, 20);
            imgS.ColorDepth = ColorDepth.Depth32Bit;

            // 動態生成書籍封面圖片（無需外部圖檔）
            GenerateBookCovers();

            // ══════════════════════════════════════════
            //  右側工具面板 pnlTools（Dock: Right）
            // ══════════════════════════════════════════
            pnlTools = new Panel
            {
                Dock = DockStyle.Right,
                Width = 200,
                BackColor = CardBg,
                Padding = new Padding(8)
            };
            pnlTools.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderLight, 1);
                e.Graphics.DrawLine(pen, 0, 0, 0, pnlTools.Height);
            };
            this.Controls.Add(pnlTools);

            // ── grpView 檢視方式（Dock: Top）──
            grpView = new GroupBox
            {
                Text = "檢視方式：",
                Dock = DockStyle.Top,
                Height = 70,
                Font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = CardBg,
                Padding = new Padding(6, 14, 6, 4)
            };

            cmbView = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft JhengHei UI", 10f),
                BackColor = Color.White
            };
            cmbView.SelectedIndexChanged += cmbView_SelectedIndexChanged;
            grpView.Controls.Add(cmbView);
            pnlTools.Controls.Add(grpView);

            // ── grpBorrow 借書清單（Dock: Fill）──
            grpBorrow = new GroupBox
            {
                Text = "借書清單：",
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = CardBg,
                Padding = new Padding(6, 14, 6, 4)
            };

            lstBorrow = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft JhengHei UI", 10f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(253, 254, 254)
            };
            grpBorrow.Controls.Add(lstBorrow);
            pnlTools.Controls.Add(grpBorrow);

            // ══════════════════════════════════════════
            //  左側 ListView（Dock: Fill）
            // ══════════════════════════════════════════
            lvwBooks = new ListView
            {
                Dock = DockStyle.Fill,
                LargeImageList = imgL,
                SmallImageList = imgS,
                Activation = ItemActivation.TwoClick,
                View = View.LargeIcon,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Microsoft JhengHei UI", 9.5f),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            lvwBooks.ItemActivate += lvwBooks_ItemActivate;
            this.Controls.Add(lvwBooks);

            // ── 載入資料 ──
            this.Load += frmBooks_Load;
        }

        // ══════════════════════════════════════════
        //  表單載入事件（對應投影片 p.14）
        // ══════════════════════════════════════════
        private void frmBooks_Load(object? sender, EventArgs e)
        {
            // 新增檢視方式選項
            cmbView.Items.Add("大圖示");
            cmbView.Items.Add("詳細資料");
            cmbView.Items.Add("小圖示");
            cmbView.Items.Add("清單");
            cmbView.Items.Add("大圖示加詳細資料");
            cmbView.SelectedIndex = 0; // 預設選取第一個項目

            // 新增欄位
            lvwBooks.Columns.Add("書名", 100);
            lvwBooks.Columns.Add("作者", 60);
            lvwBooks.Columns.Add("類別", 60);

            // 暫停重繪
            lvwBooks.BeginUpdate();
            for (int i = 0; i < b_name.Length; i++)
            {
                ListViewItem lvi = new ListViewItem(b_name[i]);
                lvi.SubItems.Add(author[i].ToString());
                lvi.SubItems.Add(kind[i]);
                lvwBooks.Items.Add(lvi);
                lvwBooks.Items[i].ImageIndex = i;
            }
            lvwBooks.EndUpdate(); // 重繪
        }

        // ══════════════════════════════════════════
        //  切換檢視方式（對應投影片 p.16）
        // ══════════════════════════════════════════
        private void cmbView_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (cmbView.SelectedIndex)
            {
                case 0: // 大圖示
                    lvwBooks.View = View.LargeIcon;
                    break;
                case 1: // 詳細資料
                    lvwBooks.View = View.Details;
                    break;
                case 2: // 小圖示
                    lvwBooks.View = View.SmallIcon;
                    break;
                case 3: // 清單
                    lvwBooks.View = View.List;
                    break;
                case 4: // 大圖示加詳細資料
                    lvwBooks.View = View.Tile;
                    break;
            }
        }

        // ══════════════════════════════════════════
        //  雙擊借書事件（對應投影片 p.18）
        // ══════════════════════════════════════════
        private void lvwBooks_ItemActivate(object? sender, EventArgs e)
        {
            // 取得書名
            string strBookname = b_name[lvwBooks.SelectedIndices[0]];
            bool exist = lstBorrow.Items.Contains(strBookname);

            if (exist != true) // 若選取的書名不存在借書清單中
            {
                DialogResult dr = MessageBox.Show("確定要借閱嗎?",
                    strBookname, MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes) // 若按 <是> 鈕
                {
                    lstBorrow.Items.Add(strBookname); // 新增項目到借書清單
                }
            }
        }

        // ══════════════════════════════════════════
        //  動態生成書籍封面圖片
        //  （因無法使用外部圖檔，程式自動繪製封面）
        // ══════════════════════════════════════════
        private void GenerateBookCovers()
        {
            // 每本書的封面底色
            Color[] coverColors =
            {
                Color.FromArgb(192, 57, 43),   // 三國演義 - 紅
                Color.FromArgb(41, 128, 185),  // 西遊記 - 藍
                Color.FromArgb(39, 174, 96),   // 唐詩三百首 - 綠
                Color.FromArgb(142, 68, 173),  // 楚辭 - 紫
                Color.FromArgb(243, 156, 18),  // 西廂記 - 橙
                Color.FromArgb(44, 62, 80),    // 水滸傳 - 深藍灰
                Color.FromArgb(211, 84, 0),    // 紅樓夢 - 深橘
                Color.FromArgb(22, 160, 133),  // 牡丹亭 - 青綠
            };

            for (int i = 0; i < b_name.Length; i++)
            {
                // ── 大圖 (90 × 120) ──
                var bmpL = new Bitmap(90, 120);
                using (var g = Graphics.FromImage(bmpL))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    // 封面底色
                    using var bgBrush = new SolidBrush(coverColors[i]);
                    g.FillRectangle(bgBrush, 0, 0, 90, 120);

                    // 裝飾線
                    using var linePen = new Pen(Color.FromArgb(80, 255, 255, 255), 1);
                    g.DrawRectangle(linePen, 6, 6, 77, 107);

                    // 書名文字（直書效果）
                    using var titleFont = new Font("Microsoft JhengHei UI", 14f, FontStyle.Bold);
                    using var titleBrush = new SolidBrush(Color.FromArgb(240, 255, 255, 255));

                    string title = b_name[i];
                    float charHeight = 22f;
                    float startY = (120 - title.Length * charHeight) / 2f;
                    float x = (90 - 22) / 2f;

                    for (int c = 0; c < title.Length; c++)
                    {
                        g.DrawString(title[c].ToString(), titleFont, titleBrush,
                            x, startY + c * charHeight);
                    }
                }
                imgL.Images.Add(bmpL);

                // ── 小圖 (15 × 20) ──
                var bmpS = new Bitmap(15, 20);
                using (var g = Graphics.FromImage(bmpS))
                {
                    using var bgBrush = new SolidBrush(coverColors[i]);
                    g.FillRectangle(bgBrush, 0, 0, 15, 20);
                    using var linePen = new Pen(Color.FromArgb(120, 255, 255, 255), 1);
                    g.DrawRectangle(linePen, 1, 1, 12, 17);
                }
                imgS.Images.Add(bmpS);
            }
        }
    }
}
