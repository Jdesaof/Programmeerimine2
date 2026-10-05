namespace MatchingGame;

public class Form1 : Form
{
    private readonly Random random = new();
    private readonly TableLayoutPanel board = new() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, BackColor = Color.CornflowerBlue, CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset };
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 750 };
    private Label firstClicked, secondClicked;
    public Form1()
    {
        Text = "Leia paarid";
        ClientSize = new Size(550, 590);
        MinimumSize = new Size(400, 440);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        for (var i = 0; i < 4; i++)
        {
            board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            board.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        }
        for (var i = 0; i < 16; i++)
        {
            var label = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Webdings", 48), UseCompatibleTextRendering = true, Margin = new Padding(0), Cursor = Cursors.Hand };
            label.Click += LabelClick;
            board.Controls.Add(label, i % 4, i / 4);
        }
        var restart = new Button { Text = "Uus mäng", AutoSize = true, Anchor = AnchorStyles.None };
        restart.Click += (_, _) => AssignIconsToSquares();
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            firstClicked.ForeColor = board.BackColor;
            secondClicked.ForeColor = board.BackColor;
            firstClicked = secondClicked = null;
        };
        layout.Controls.Add(board, 0, 0);
        layout.Controls.Add(restart, 0, 1);
        Controls.Add(layout);
        AssignIconsToSquares();
    }
    private void AssignIconsToSquares()
    {
        timer.Stop();
        firstClicked = secondClicked = null;
        var icons = new List<string> { "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z" };
        foreach (Label label in board.Controls)
        {
            var index = random.Next(icons.Count);
            label.Text = icons[index];
            icons.RemoveAt(index);
            label.ForeColor = board.BackColor;
        }
    }
    private void LabelClick(object sender, EventArgs e)
    {
        if (timer.Enabled || sender is not Label clicked || clicked.ForeColor == Color.Black) return;
        clicked.ForeColor = Color.Black;
        if (firstClicked == null) { firstClicked = clicked; return; }
        secondClicked = clicked;
        if (firstClicked.Text == secondClicked.Text)
        {
            firstClicked = secondClicked = null;
            if (board.Controls.Cast<Label>().All(label => label.ForeColor == Color.Black))
                MessageBox.Show(this, "Leidsid kõik paarid!", "Võit");
        }
        else timer.Start();
    }
    protected override void Dispose(bool disposing) { if (disposing) timer.Dispose(); base.Dispose(disposing); }
}
