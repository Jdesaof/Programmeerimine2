namespace KooliProjekt.WindowsForms;
partial class Form1
{
    private System.ComponentModel.IContainer components;
    private DataGridView dataGridView1;
    private Button reloadButton, addButton, saveButton, deleteButton;
    private Label statusLabel;
    private TableLayoutPanel layout, editor;
    private TextBox idBox, nameBox, typeBox, descriptionBox;
    private NumericUpDown alcoholBox;

    protected override void Dispose(bool disposing)
    {
        if (disposing) { lifetime.Cancel(); components?.Dispose(); }
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        layout = new TableLayoutPanel();
        editor = new TableLayoutPanel();
        dataGridView1 = new DataGridView();
        reloadButton = new Button();
        addButton = new Button();
        saveButton = new Button();
        deleteButton = new Button();
        statusLabel = new Label();
        idBox = new TextBox();
        nameBox = new TextBox();
        typeBox = new TextBox();
        descriptionBox = new TextBox();
        alcoholBox = new NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)alcoholBox).BeginInit();
        SuspendLayout();
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(12);
        layout.ColumnCount = 2;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
        layout.RowCount = 2;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(statusLabel, 0, 0);
        layout.Controls.Add(reloadButton, 1, 0);
        layout.Controls.Add(dataGridView1, 0, 1);
        layout.Controls.Add(editor, 1, 1);
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        statusLabel.Text = "Õllede nimekiri";
        reloadButton.Text = "Värskenda";
        reloadButton.Dock = DockStyle.Fill;
        reloadButton.Click += ReloadButton_Click;

        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.ReadOnly = true;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.MultiSelect = false;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nimi", HeaderText = "Nimi", FillWeight = 100 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tuup", HeaderText = "Tüüp", FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Alkoholiprotsent", HeaderText = "Alkohol (%)", FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Kirjeldus", HeaderText = "Kirjeldus", FillWeight = 140 });
        dataGridView1.SelectionChanged += SelectionChanged;

        editor.Dock = DockStyle.Fill;
        editor.Padding = new Padding(12, 0, 0, 0);
        editor.ColumnCount = 1;
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        editor.RowCount = 12;
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        editor.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        editor.Controls.Add(new Label { Text = "ID", Dock = DockStyle.Fill }, 0, 0);
        editor.Controls.Add(idBox, 0, 1);
        editor.Controls.Add(new Label { Text = "Nimi", Dock = DockStyle.Fill }, 0, 2);
        editor.Controls.Add(nameBox, 0, 3);
        editor.Controls.Add(new Label { Text = "Tüüp", Dock = DockStyle.Fill }, 0, 4);
        editor.Controls.Add(typeBox, 0, 5);
        editor.Controls.Add(new Label { Text = "Alkohol (%)", Dock = DockStyle.Fill }, 0, 6);
        editor.Controls.Add(alcoholBox, 0, 7);
        editor.Controls.Add(new Label { Text = "Kirjeldus", Dock = DockStyle.Fill }, 0, 8);
        editor.Controls.Add(descriptionBox, 0, 9);
        var buttons = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        buttons.Controls.Add(addButton, 0, 0);
        buttons.Controls.Add(saveButton, 1, 0);
        editor.Controls.Add(buttons, 0, 10);
        editor.Controls.Add(deleteButton, 0, 11);
        idBox.ReadOnly = true;
        idBox.Text = "Uus";
        idBox.Dock = DockStyle.Fill;
        nameBox.Dock = DockStyle.Fill;
        typeBox.Dock = DockStyle.Fill;
        descriptionBox.Dock = DockStyle.Fill;
        descriptionBox.Multiline = true;
        descriptionBox.ScrollBars = ScrollBars.Vertical;
        alcoholBox.Dock = DockStyle.Fill;
        alcoholBox.DecimalPlaces = 2;
        alcoholBox.Maximum = 100;
        addButton.Text = "Lisa uus";
        saveButton.Text = "Salvesta";
        deleteButton.Text = "Kustuta";
        addButton.Dock = DockStyle.Fill;
        saveButton.Dock = DockStyle.Fill;
        deleteButton.Dock = DockStyle.Fill;
        deleteButton.Enabled = false;
        addButton.Click += AddButton_Click;
        saveButton.Click += SaveButton_Click;
        deleteButton.Click += DeleteButton_Click;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1180, 580);
        MinimumSize = new Size(1000, 500);
        Controls.Add(layout);
        Text = "Õlleprojekt — õllede haldamine";
        StartPosition = FormStartPosition.CenterScreen;
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ((System.ComponentModel.ISupportInitialize)alcoholBox).EndInit();
        ResumeLayout(false);
    }
}

