using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kviz_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<RadioButton> gombok = new List<RadioButton>();
        public Dictionary<string, Tantargy> tantargyNyilvantarto = new Dictionary<string, Tantargy>();
        public List<Tantargy> targyak = new List<Tantargy>();
        Tantargy selectedTantargy = null;
        Temakor selectedTemakor = null;
        public List<Kerdesek> betoltottKerdesek = new List<Kerdesek>();
        int oldalIndex = 0;
        Kerdesek currentKerdes = null;

        List<Button> oldalValtoGombok = new List<Button>();
        bool kiertekelt = false;
        public MainWindow()
        {
            InitializeComponent();
            oldalValtoGombok.Add(oldalJelzo_1);
            oldalValtoGombok.Add(oldalJelzo_2);
            oldalValtoGombok.Add(oldalJelzo_3);
            oldalValtoGombok.Add(oldalJelzo_4);
            oldalValtoGombok.Add(oldalJelzo_5);
            oldalValtoGombok.Add(oldalJelzo_6);
            oldalValtoGombok.Add(oldalJelzo_7);
            oldalValtoGombok.Add(oldalJelzo_8);
            oldalValtoGombok.Add(oldalJelzo_9);
            oldalValtoGombok.Add(oldalJelzo_10);
            
            
            gombok.Add(valasz1);
            gombok.Add(valasz2);
            gombok.Add(valasz3);
            gombok.Add(valasz4);

            temakorComboBox.IsEnabled = false;

            Tantargy fizika = new Tantargy("fizika-adatbazis.txt");
            tantargyNyilvantarto.Add(fizika.temakornev, fizika);
            tantargyComboBox.Items.Add(fizika.temakornev);

            Tantargy physics = new Tantargy("physics-adatbazis.txt");
            tantargyNyilvantarto.Add(physics.temakornev, physics);
            tantargyComboBox.Items.Add(physics.temakornev);
        }

        private void tantargyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            temakorComboBox.Items.Clear();
            temakorComboBox.Items.Clear();
            selectedTantargy = null;
            selectedTemakor = null;

            Tantargy targy = null;
            string selectedTantargyString = tantargyComboBox.SelectedItem.ToString();
            tantargyNyilvantarto.TryGetValue(selectedTantargyString, out targy);

            temakorComboBox.IsEnabled = false;

            if (targy == null) return;

            temakorComboBox.IsEnabled = true;

            selectedTantargy = targy;
            foreach (Temakor tema in targy.temakorok)
            {
                temakorComboBox.Items.Add(tema.temakornev);
            }

        }

        private void temakorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (temakorComboBox.SelectedItem == null || temakorComboBox.Items.Count <= 0) { return; }
            Temakor tema = null;
            string selectedTemakorString = temakorComboBox.SelectedItem.ToString();
            selectedTantargy.temakorNyilvantarto.TryGetValue(selectedTemakorString, out tema);

            if (tema == null) return;

            selectedTemakor = tema;
        }

        private void FeladatGeneraloGomb_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTemakor == null & selectedTantargy == null)
            {
                MessageBox.Show("Kérlek válassz tantárgyat és témakört!");
                return;
            }
            else if (selectedTemakor == null)
            {
                MessageBox.Show("Kérlek válassz egy témakört!");
                return;
            }
            else if (selectedTantargy == null)
            {
                MessageBox.Show("Kérlek válassz egy tantárgyat!");
                return;
            }
            kvizOldal.Visibility = Visibility.Visible;
            foOldal.Visibility = Visibility.Hidden;

            tantargy.Content = selectedTantargy.temakornev;
            temakor.Content = selectedTemakor.temakornev;

            KerdessorGeneral();

            foreach (RadioButton button in gombok)
            {
                if (button.Content.ToString() == currentKerdes.helyesValasz)
                {
                    button.Foreground = Brushes.Black;
                }
                else
                {
                    button.Foreground = Brushes.Black;
                }
                button.IsEnabled = true;
            }
            kiertekelt = false;
        }

        private void KerdessorGeneral()
        {
            Random random = new Random();
            List<Kerdesek> osszesKerdes = selectedTemakor.kerdesek;
            List<Kerdesek> tempKerdesek = selectedTemakor.kerdesek;
            for (int i = 0; i < 10; i++)
            {
                int randomIndex = random.Next(0, tempKerdesek.Count);
                betoltottKerdesek.Add(tempKerdesek[randomIndex]);
                tempKerdesek.RemoveAt(randomIndex);
            }
            KerdesekBetolt(betoltottKerdesek[0]);
            oldalIndex = 0;
        }

        private void KerdesekBetolt(Kerdesek ujKerdes)
        {
            Random random = new Random();
            List<string> tempValasz = new List<string>();
            currentKerdes = ujKerdes;
            foreach (var item in ujKerdes.valaszok)
            {
                tempValasz.Add(item);
            }
            List<RadioButton> tempButton = new List<RadioButton>();
            tempButton = gombok;

            if (ujKerdes.sorrend.Count <= 0)
            {
                for (int i = 0; i < 4; i++)
                {

                    int randomIndex = random.Next(0, tempValasz.Count);

                    gombok[i].Content = tempValasz[randomIndex];
                    ujKerdes.sorrend.Add(tempValasz[randomIndex]);
                    tempValasz.RemoveAt(randomIndex);



                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    gombok[i].Content = ujKerdes.sorrend[i];
                }
            }
            foreach (RadioButton but in gombok)
            {
                //if (ujKerdes.kivalasztott != null && but.Content.ToString() == ujKerdes.kivalasztott);
                if (but.Content.ToString() == ujKerdes.kivalasztott && ujKerdes.kivalasztott != null)
                {
                    but.IsChecked = true;
                }
            }

            kerdesLabel.Content = ujKerdes.kerdes;
            oldalIndex = betoltottKerdesek.IndexOf(ujKerdes);
            haladasJelzo.Content = $"{oldalIndex + 1}/10";
            foreach (Button button in oldalValtoGombok)
            {
                button.Background = Brushes.White;
            }


            if (kiertekelt)
            {
                foreach (RadioButton button in gombok)
                {
                    if (button.Content.ToString() == ujKerdes.helyesValasz)
                    {
                        button.Foreground = Brushes.Green;
                    }
                    else
                    {
                        button.Foreground = Brushes.Red;
                    }
                    button.IsEnabled = false;

                }
            }
        }

        private void Kovetkezo_Oldal(object sender, RoutedEventArgs e)
        {
            if (oldalIndex + 1 >= betoltottKerdesek.Count) return;
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[oldalIndex + 1]);

        }

        private void elozoLap_Betolt(object sender, RoutedEventArgs e)
        {
            if (oldalIndex <= 0) return;
            KerdesekBetolt(betoltottKerdesek[oldalIndex - 1]);
        }
        private void Valasz1_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.kivalasztott = valasz1.Content.ToString();
            valasz1.Background = Brushes.LightGray;
            valasz2.Background = Brushes.Transparent;
            valasz3.Background = Brushes.Transparent;
            valasz4.Background = Brushes.Transparent;

        }

        private void Valasz2_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.kivalasztott = valasz2.Content.ToString();
            valasz2.Background = Brushes.LightGray;
            valasz1.Background = Brushes.Transparent;
            valasz3.Background = Brushes.Transparent;
            valasz4.Background = Brushes.Transparent;

        }

        private void Valasz3_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.kivalasztott = valasz3.Content.ToString();
            valasz3.Background = Brushes.LightGray;
            valasz2.Background = Brushes.Transparent;
            valasz1.Background = Brushes.Transparent;
            valasz4.Background = Brushes.Transparent;
        }

        private void Valasz4_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.kivalasztott = valasz4.Content.ToString();
            valasz4.Background = Brushes.LightGray;
            valasz2.Background = Brushes.Transparent;
            valasz3.Background = Brushes.Transparent;
            valasz1.Background = Brushes.Transparent;

        }

        private void Nullazas()
        {
            valasz1.IsChecked = false;
            valasz2.IsChecked = false;
            valasz3.IsChecked = false;
            valasz4.IsChecked = false;
            valasz1.Background = Brushes.Transparent;
            valasz2.Background = Brushes.Transparent;
            valasz3.Background = Brushes.Transparent;
            valasz4.Background = Brushes.Transparent;
        }
        private void oldalJelzo_1_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[0]);
        }

        private void oldalJelzo_2_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[1]);
        }
        private void oldalJelzo_3_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[2]);
        }
        private void oldalJelzo_4_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[3]);
        }
        private void oldalJelzo_5_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[4]);
        }
        private void oldalJelzo_6_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[5]);
        }
        private void oldalJelzo_7_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[6]);
        }
        private void oldalJelzo_8_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[7]);
        }
        private void oldalJelzo_9_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[8]);
        }
        private void oldalJelzo_10_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            KerdesekBetolt(betoltottKerdesek[9]);
        }

        private void Vegkiertekeles(object sender, RoutedEventArgs e)
        {
            kiertekelt = true;

            foreach (RadioButton button in gombok)
            {
                if (button.Content.ToString() == currentKerdes.helyesValasz)
                {
                    button.Foreground = Brushes.Green;
                }
                else
                {
                    button.Foreground = Brushes.Red;
                }
                button.IsEnabled = false;
            }

            int helyesValaszok = 0;
            for (int i = 0; i < betoltottKerdesek.Count; i++)
            {
                if (betoltottKerdesek[i].kivalasztott == betoltottKerdesek[i].helyesValasz)
                {
                    helyesValaszok++;
                    oldalValtoGombok[i].Foreground = Brushes.Green;
                    oldalValtoGombok[i].Foreground = Brushes.Green;
                }
                else
                {
                    oldalValtoGombok[i].Foreground = Brushes.Red;
                    oldalValtoGombok[i].Foreground = Brushes.Red;
                }
            }
            MessageBox.Show($"{helyesValaszok}/10 pontod lett. Gratulálok!");

            kilepesGomb.Visibility = Visibility.Visible;
        }

        private void kilepes(object sender, RoutedEventArgs e)
        {
            Nullazas();
            betoltottKerdesek.Clear();
            oldalIndex = 0;

            kvizOldal.Visibility = Visibility.Hidden;
            foOldal.Visibility = Visibility.Visible;

            kilepesGomb.Visibility = Visibility.Hidden;
            selectedTemakor = null;
            selectedTantargy = null;
            Close();
        }
        
        public class Tantargy
        {
            public Dictionary<string, Temakor> temakorNyilvantarto = new Dictionary<string, Temakor>();
            public List<Temakor> temakorok = new List<Temakor>();
            public string temakornev;

            public Tantargy(string eleresUtja)
            {
                string[] fajl = File.ReadAllLines(eleresUtja);

                temakornev = fajl[0].Split(';')[0];

                List<string> temaKorok = new List<string>();
                foreach (string sor in fajl)
                {
                    string temaKorNev = sor.Split(';')[1];
                    if (!temaKorok.Contains(temaKorNev))
                    {
                        temaKorok.Add(temaKorNev);
                        Temakor ujTema = new Temakor(temaKorNev, fajl);
                        temakorNyilvantarto.Add(ujTema.temakornev, ujTema);
                        temakorok.Add(ujTema);
                    }
                }
            }
        }

        public class Temakor
        {
            public List<Kerdesek> kerdesek = new List<Kerdesek>();
            public string temakornev;

            public Temakor(string temakornev, string[] fajl)
            {
                this.temakornev = temakornev;
                foreach (string sor in fajl)
                {
                    if (sor.Split(';')[1] == temakornev)
                    {
                        Kerdesek kerdes = new Kerdesek(sor);
                        kerdesek.Add(kerdes);
                    }
                }
            }
        }

        public class Kerdesek
        {
            public string kerdes;
            public string helyesValasz;
            public string valasz2;
            public string valasz3;
            public string valasz4;

            public string kivalasztott = null;

            public List<string> sorrend = new List<string>();
            public List<string> valaszok = new List<string>();

            public Kerdesek(string sor)
            {
                string[] splits = sor.Split(';');
                kerdes = splits[2];
                helyesValasz = splits[3];
                valasz2 = splits[4];
                valasz3 = splits[5];
                valasz4 = splits[6];

                ValaszokFeltölt();
            }

            private void ValaszokFeltölt()
            {
                valaszok.Add(helyesValasz);
                valaszok.Add(valasz2);
                valaszok.Add(valasz3);
                valaszok.Add(valasz4);
            }
        }
    }
}
