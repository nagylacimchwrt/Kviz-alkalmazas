﻿using System;
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
        Tantargy selectedTargy = null;
        Temakor selectedTema = null;
        public List<Kerdesek> betoltottKerdesek = new List<Kerdesek>();
        int oldalIndex = 0;
        Kerdesek currentKerdes = null;

        List<Button> oldalValtoGombok = new List<Button>();
        bool kiertekelt = false;
        public MainWindow()
        {
            InitializeComponent();
        }
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
        public string elsoValasz;
        public string masodikValasz;
        public string harmadikValasz;
        public string negyedikValasz;

        public string kivalasztott = null;

        public List<string> sorrend = new List<string>();
        public List<string> valaszok = new List<string>();

        public Kerdesek(string sor)
        {
            string[] splits = sor.Split(';');
            kerdes = splits[2];
            helyesValasz = splits[3];
            masodikValasz = splits[4];
            harmadikValasz = splits[5];
            negyedikValasz = splits[6];

            ValaszokFeltölt();
        }

        private void ValaszokFeltölt()
        {
            valaszok.Add(helyesValasz);
            valaszok.Add(elsoValasz);
            valaszok.Add(masodikValasz);
            valaszok.Add(harmadikValasz);
            valaszok.Add(negyedikValasz);
        }
    }
}
