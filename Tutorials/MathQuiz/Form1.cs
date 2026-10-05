namespace MathQuiz;

public class Form1 : Form
{
    private readonly Random random = new();
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };
    private readonly Label time = new() { AutoSize = true, Text = "Aega: 30 sekundit" };
    private readonly Button start = new() { Text = "Alusta", AutoSize = true };
    private readonly Label[] questions = new Label[4];
    private readonly NumericUpDown[] answers = new NumericUpDown[4];
    private readonly int[] expected = new int[4];
    private int seconds;
    public Form1()
    {
        Text = "Matemaatikaviktoriin";
        ClientSize = new Size(540, 360);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), ColumnCount = 2, RowCount = 6 };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        grid.Controls.Add(time, 0, 0);
        grid.SetColumnSpan(time, 2);
        for (var i = 0; i < 4; i++)
        {
            questions[i] = new Label { Text = "? + ? =", AutoSize = true, Font = new Font(Font.FontFamily, 18) };
            answers[i] = new NumericUpDown { Minimum = 0, Maximum = 1000, Width = 120, Enabled = false, Font = new Font(Font.FontFamily, 16) };
            answers[i].Enter += (_, _) => { if (ActiveControl is NumericUpDown number) number.Select(0, number.Text.Length); };
            grid.Controls.Add(questions[i], 0, i + 1);
            grid.Controls.Add(answers[i], 1, i + 1);
        }
        grid.Controls.Add(start, 0, 5);
        start.Click += (_, _) => StartTheQuiz();
        timer.Tick += TimerTick;
        Controls.Add(grid);
    }
    private void StartTheQuiz()
    {
        int a = random.Next(51), b = random.Next(51);
        SetProblem(0, a, "+", b, a + b);
        a = random.Next(1, 101); b = random.Next(a + 1);
        SetProblem(1, a, "−", b, a - b);
        a = random.Next(2, 11); b = random.Next(2, 11);
        SetProblem(2, a, "×", b, a * b);
        b = random.Next(2, 11); var quotient = random.Next(2, 11);
        SetProblem(3, b * quotient, "÷", b, quotient);
        foreach (var answer in answers) { answer.Value = 0; answer.Enabled = true; }
        seconds = 30;
        time.Text = "Aega: 30 sekundit";
        time.ForeColor = SystemColors.ControlText;
        start.Enabled = false;
        timer.Start();
        answers[0].Focus();
    }
    private void SetProblem(int index, int a, string operation, int b, int value)
    {
        questions[index].Text = $"{a} {operation} {b} =";
        expected[index] = value;
    }
    private bool CheckTheAnswer() => answers.Select((answer, i) => answer.Value == expected[i]).All(correct => correct);
    private void TimerTick(object sender, EventArgs e)
    {
        if (CheckTheAnswer()) { Finish("Kõik vastused on õiged!"); return; }
        seconds--;
        time.Text = $"Aega: {seconds} sekundit";
        if (seconds <= 5) time.ForeColor = Color.Red;
        if (seconds == 0)
        {
            for (var i = 0; i < answers.Length; i++) answers[i].Value = expected[i];
            Finish("Aeg sai läbi. Õiged vastused on vormil.");
        }
    }
    private void Finish(string message)
    {
        timer.Stop();
        foreach (var answer in answers) answer.Enabled = false;
        start.Enabled = true;
        MessageBox.Show(this, message, "Tulemus");
    }
    protected override void Dispose(bool disposing) { if (disposing) timer.Dispose(); base.Dispose(disposing); }
}
