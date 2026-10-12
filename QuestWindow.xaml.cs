using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace NazwaProjektu
{
    public partial class QuestWindow : Window
    {
        Quest quest;

        // konstruktor okna, przyjmuje obiekt quest jako parametr i ustawia pola tekstowe na wartosci z obiektu quest
        public QuestWindow(Quest quest)
        {
            InitializeComponent();
            this.quest = quest;
            NameBox.Text = quest.Name;
            DescBox.Text = quest.Desc;
        }


        // po wcisnieciu przycisku "Save", sprawdzamy czy nazwa questu nie jest pusta, jesli jest pusta wyswietlamy komunikat o bledzie
        // jesli nie jest pusta, zapisujemy dane do obiektu quest 
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Quest name cannot be empty.");
                return;
            }
            quest.Name = NameBox.Text;
            quest.Desc = DescBox.Text;

            DialogResult = true;
        }


        // po wcisnieciu przycisku "Cancel", zamykamy okno bez zapisywania zmian
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
