using System.Windows;
using System.Windows.Controls;

namespace MemoryGame.Views
{
    public partial class CustomBoardDialog : Window
    {
        public int SelectedRows { get; private set; }
        public int SelectedColumns { get; private set; }

        public CustomBoardDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedRows = int.Parse(((ComboBoxItem)RowsComboBox.SelectedItem).Content.ToString());
            SelectedColumns = int.Parse(((ComboBoxItem)ColumnsComboBox.SelectedItem).Content.ToString());

            // Verificăm dacă numărul total de celule este par
            if ((SelectedRows * SelectedColumns) % 2 != 0)
            {
                MessageBox.Show("The total number of cells must be even!", "Invalid Configuration",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}