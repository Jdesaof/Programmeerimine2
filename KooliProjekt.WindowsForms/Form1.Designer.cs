namespace KooliProjekt.WindowsForms;

partial class Form1
{
    private System.ComponentModel.IContainer components;
    private DataGridView dataGridView1;
    private Button reloadButton;
    private Label statusLabel;
    private TableLayoutPanel layout;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            lifetime.Cancel();
            client.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        layout = new TableLayoutPanel();
        reloadButton = new Button();
        statusLabel = new Label();
        dataGridView1 = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        layout.SuspendLayout();
        SuspendLayout();

        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(12);
        layout.ColumnCount = 2;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
        layout.RowCount = 2;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(statusLabel, 0, 0);
        layout.Controls.Add(reloadButton, 1, 0);
        layout.Controls.Add(dataGridView1, 0, 1);
        layout.SetColumnSpan(dataGridView1, 2);

        reloadButton.Text = "Värskenda";
        reloadButton.Dock = DockStyle.Fill;
        reloadButton.Click += ReloadButton_Click;
        statusLabel.Text = "Õllede nimekiri";
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        statusLabel.AutoEllipsis = true;

        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.ReadOnly = true;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdColumn", DataPropertyName = "Id", HeaderText = "ID", FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NimiColumn", DataPropertyName = "Nimi", HeaderText = "Nimi", FillWeight = 100 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TuupColumn", DataPropertyName = "Tuup", HeaderText = "Tüüp", FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "AlkoholColumn", DataPropertyName = "Alkoholiprotsent", HeaderText = "Alkohol (%)", FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "KirjeldusColumn", DataPropertyName = "Kirjeldus", HeaderText = "Kirjeldus", FillWeight = 170 });

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 540);
        MinimumSize = new Size(760, 380);
        Controls.Add(layout);
        Name = "Form1";
        Text = "Õlleprojekt — õllede nimekiri";
        StartPosition = FormStartPosition.CenterScreen;
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        layout.ResumeLayout(false);
        ResumeLayout(false);
    }
}

