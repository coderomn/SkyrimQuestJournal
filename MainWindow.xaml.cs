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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // ObservableCollection<Quest> jest używana do przechowywania listy questów,
        // ponieważ automatycznie powiadamia interfejs użytkownika o zmianach w kolekcji
        // (dodawanie, usuwanie, edytowanie).
        ObservableCollection<Quest> quests = new ObservableCollection<Quest>();

        // Konstruktor okna głównego, inicjalizuje komponenty, ustawia źródło danych dla listy questów i ładuje questy z pliku
        public MainWindow()
        {
            InitializeComponent();
            QuestList.ItemsSource = quests;
            LoadQuests();
        }

        // Metoda obsługująca kliknięcie przycisku "Add",
        // tworzy nowy obiekt Quest, otwiera okno QuestWindow do edycji i dodaje quest do listy jeśli użytkownik zatwierdzi zmiany
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Quest quest = new Quest();
            QuestWindow window = new QuestWindow(quest);
            if (window.ShowDialog() == true)
            {
                quests.Add(quest);
            }
            
        }

        // Metoda obsługująca kliknięcie przycisku "Edit",
        // sprawdza czy wybrano quest, otwiera okno QuestWindow do edycji i odświeża listę questów po zatwierdzeniu zmian
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (QuestList.SelectedItem == null)
            {
                MessageBox.Show("Please select a quest to edit.");
                return;
            }

            Quest quest = (Quest)QuestList.SelectedItem;
            QuestWindow window = new QuestWindow(quest);
            window.ShowDialog();
            QuestList.Items.Refresh();
        }

        // Metoda obsługująca kliknięcie przycisku "Delete",
        // sprawdza czy wybrano quest, pyta użytkownika o potwierdzenie i usuwa quest z listy jeśli użytkownik potwierdzi
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (QuestList.SelectedItem == null)
            {
                MessageBox.Show("Please select a quest to delete.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this quest?", "Yes or no: ", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                quests.Remove((Quest)QuestList.SelectedItem);
            }
        }

        // Metoda obsługująca kliknięcie przycisku "Save",
        // serializuje listę questów do formatu JSON i zapisuje ją do pliku "quests.json"
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string json = JsonSerializer.Serialize(quests);
                File.WriteAllText("quests.json", json);
                MessageBox.Show("Quests saved successfully.");
            }
            catch
            {
                MessageBox.Show("Error saving quests to file.");
            }
        }

        // Metoda obsługująca zmianę tekstu w polu wyszukiwania,
        // filtruje listę questów na podstawie wprowadzonego tekstu i aktualizuje źródło danych listy
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Search.Text.ToLower();

            QuestList.ItemsSource = quests.Where(q => q.Name.ToLower().Contains(text)).ToList();
        }

        // Metoda obsługująca kliknięcie przycisku "Sort",
        // sortuje listę questów alfabetycznie według nazwy i aktualizuje źródło danych listy
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            QuestList.ItemsSource = quests.OrderBy(q => q.Name).ToList();
        }

        // Metoda obsługująca kliknięcie przycisku "About",
        // otwiera okno AboutWindow z informacjami o aplikacji
        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }

        // Metoda ładująca questy z pliku "quests.json",
        // deserializuje dane JSON do kolekcji ObservableCollection<Quest> i dodaje je do listy questów
        private void LoadQuests()
        {
            try
            {
             if (File.Exists("quests.json"))
                {
                    string json = File.ReadAllText("quests.json");

                    var loadedQuests = JsonSerializer.Deserialize<ObservableCollection<Quest>>(json);
                    if (loadedQuests != null)
                    {
                        foreach (Quest quest in loadedQuests)
                        {
                            quests.Add(quest);
                        }
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Error loading quests from file.");
            }
        }

    }

    // Klasa Quest reprezentuje pojedynczy quest z właściwościami Name (nazwa) i Desc (opis).
    public class Quest
    {
        public string Name { get; set; } = "";
        public string Desc { get; set; } = "";
    }
}