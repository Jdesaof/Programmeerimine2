namespace PictureViewer;

public class Form1 : Form
{
    private readonly PictureBox picture = new() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.Fixed3D };
    private readonly CheckBox stretch = new() { Text = "Venita", AutoSize = true };
    public Form1()
    {
        Text = "Piltide vaatamine";
        ClientSize = new Size(800, 550);
        MinimumSize = new Size(600, 400);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.Controls.Add(picture, 0, 0);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(5) };
        buttons.Controls.Add(stretch);
        AddButton(buttons, "Ava pilt", OpenPicture);
        AddButton(buttons, "Tühjenda", (_, _) => ReplaceImage(null));
        AddButton(buttons, "Taustavärv", (_, _) =>
        {
            using var dialog = new ColorDialog { Color = picture.BackColor };
            if (dialog.ShowDialog(this) == DialogResult.OK) picture.BackColor = dialog.Color;
        });
        AddButton(buttons, "Sulge", (_, _) => Close());
        stretch.CheckedChanged += (_, _) => picture.SizeMode = stretch.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;
        layout.Controls.Add(buttons, 0, 1);
        Controls.Add(layout);
    }
    private static void AddButton(Control parent, string text, EventHandler handler)
    {
        var button = new Button { Text = text, AutoSize = true };
        button.Click += handler;
        parent.Controls.Add(button);
    }
    private void OpenPicture(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog { Filter = "Pildid|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Kõik failid|*.*" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var source = Image.FromFile(dialog.FileName);
            ReplaceImage(new Bitmap(source));
        }
        catch (Exception ex) when (ex is ArgumentException or IOException or OutOfMemoryException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, "Pilti ei saanud avada. Vali sobiv pildifail.", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void ReplaceImage(Image image)
    {
        var previous = picture.Image;
        picture.Image = image;
        previous?.Dispose();
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing) ReplaceImage(null);
        base.Dispose(disposing);
    }
}
