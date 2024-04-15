namespace WordSearchApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWord.Text) || string.IsNullOrWhiteSpace(txtDirectory.Text))
            {
                MessageBox.Show("Please enter a word and directory path.");
                return;
            }

            string word = txtWord.Text.ToLower();
            string directoryPath = txtDirectory.Text;

            try
            {
                await SearchDirectoryAsync(word, directoryPath);
                MessageBox.Show("Search completed. See the report for results.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async Task SearchDirectoryAsync(string word, string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                int count = await SearchWordInFileAsync(word, file);
                if (count > 0)
                {
                    string fileName = Path.GetFileName(file);
                    string filePath = file;
                    string result = $"File Name: {fileName}{Environment.NewLine}" +
                                    $"File Path: {filePath}{Environment.NewLine}" +
                                    $"Occurrences of '{word}': {count}{Environment.NewLine}{Environment.NewLine}";
                    txtReport.AppendText(result);
                }
            }
        }

        private async Task<int> SearchWordInFileAsync(string word, string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                int count = 0;
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    string[] words = line.ToLower().Split(new char[] { ' ', '\t', '\n', '\r', ',', '.', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string w in words)
                    {
                        if (w == word)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
