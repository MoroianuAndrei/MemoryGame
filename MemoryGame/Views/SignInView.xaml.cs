using System.Windows.Controls;

namespace MemoryGame.Views
{
    public partial class SignInView : UserControl
    {
        public SignInView()
        {
            InitializeComponent();
            // DataContext este deja setat în XAML, dar se poate seta și aici dacă se dorește:
            // this.DataContext = new SignInViewModel();
        }
    }
}
