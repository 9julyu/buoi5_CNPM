using System;
using System.Drawing;
using System.Windows.Forms;

namespace buoi5
{
    public partial class FontDialogForm : Form
    {
        private string currentFilePath = null; // Biến lưu đường dẫn tập tin hiện tại

        public FontDialogForm()
        {
            InitializeComponent();
            InitializeComboBoxes();
            ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;

        }

        private void InitializeComboBoxes()
        {
            // Thêm kích thước chữ vào ComboBox
            cmbFontSize.Items.AddRange(new object[] { 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            cmbFontSize.SelectedItem = 14; // Giá trị mặc định

            // Thêm danh sách phông chữ hệ thống vào ComboBox
            foreach (FontFamily font in FontFamily.Families)
            {
                cmbFontStyle.Items.Add(font.Name);
            }
            cmbFontStyle.SelectedItem = "Tahoma"; // Giá trị mặc định

            // Kết nối sự kiện
            cmbFontSize.SelectedIndexChanged += cmbFontSize_SelectedIndexChanged;
            cmbFontStyle.SelectedIndexChanged += cmbFontStyle_SelectedIndexChanged;
        }

        private void cmbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFontSize.SelectedItem != null && richTextBox.SelectionFont != null)
            {
                float fontSize = Convert.ToSingle(cmbFontSize.SelectedItem);
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont.FontFamily, fontSize, richTextBox.SelectionFont.Style);
            }
        }

        private void cmbFontStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFontStyle.SelectedItem != null && richTextBox.SelectionFont != null)
            {
                string selectedFont = cmbFontStyle.SelectedItem.ToString();
                float fontSize = richTextBox.SelectionFont.Size;
                richTextBox.SelectionFont = new Font(new FontFamily(selectedFont), fontSize, richTextBox.SelectionFont.Style);
            }
        }

        private void createNewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Xóa nội dung và đặt giá trị mặc định
            richTextBox.Clear();
            richTextBox.Font = new Font("Tahoma", 14);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileExtension = System.IO.Path.GetExtension(openFileDialog.FileName).ToLower();
                if (fileExtension == ".txt")
                {
                    richTextBox.Text = System.IO.File.ReadAllText(openFileDialog.FileName);
                }
                else if (fileExtension == ".rtf")
                {
                    richTextBox.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                }
                else
                {
                    MessageBox.Show("Định dạng không được hỗ trợ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentFilePath == null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = saveFileDialog.FileName;
                        richTextBox.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                        MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    richTextBox.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void địnhDạngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.ShowColor = true;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox.SelectionFont = fontDialog.Font;
                    richTextBox.SelectionColor = fontDialog.Color;
                }
            }
        }

        private void toolStripBold_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont != null)
            {
                FontStyle style = richTextBox.SelectionFont.Style ^ FontStyle.Bold;
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, style);
            }
        }

        private void toolStripItalic_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont != null)
            {
                FontStyle style = richTextBox.SelectionFont.Style ^ FontStyle.Italic;
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, style);
            }
        }

        private void toolStripUnderline_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont != null)
            {
                FontStyle style = richTextBox.SelectionFont.Style ^ FontStyle.Underline;
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, style);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận thoát
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes, thoát ứng dụng
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

    }
}
