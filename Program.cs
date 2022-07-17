using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Projekt_apk
{
    class Program
    {
        static void Main(string[] args)
        { //Rozliczanie domowych wydatków wraz z zapisem do pliku 

            WydatkiApp wydatkiApp = new WydatkiApp();

            wydatkiApp.Powitanie();
            wydatkiApp.AskForAction();
        }


        public class WydatkiApp
        {
            WydatekManager wydatekManager = new WydatekManager();

            public void Powitanie()
            {
                Console.WriteLine("Witam w aplikacji Twoje Wydatki");
            }
            public void AddWydatek()
            {

                Console.WriteLine("Podaj nazwę wydatku");
                var wydatekName = Console.ReadLine();

                Console.WriteLine("Podaj kwotę wydatku");
                var wydatekAmount = Console.ReadLine();

                var amountInDecimal = default(decimal);

                while (!decimal.TryParse(wydatekAmount, out amountInDecimal))
                {
                    Console.WriteLine("Podano niepoprawną kwotę");

                    Console.WriteLine("Podaj kwotę");
                    wydatekAmount = Console.ReadLine();

                }
                wydatekManager.AddWydatek(wydatekName, amountInDecimal);

                Console.WriteLine("Pomyślnie dodano: " + wydatekName + " oraz kwotę: " + amountInDecimal + " zł");



            }

            public void DeleteWydatek()
            {



                Console.WriteLine("Podaj nazwę wydatku, którą chcesz usunąć");
                var wydatekName = Console.ReadLine();


                wydatekManager.DeleteWydatek(wydatekName);

                Console.WriteLine("Udało się usunąć wydatek");

            }

            public void SumaWydatek()
            {
                wydatekManager.SumaWydatkow();

            }

            public void ListaWydatkow()
            {

                Console.WriteLine("Oto lista twoich wydatków: ");
                foreach (var wydatek in wydatekManager.ListaWydatkow())
                {
                    Console.WriteLine(wydatek);
                }

            }



            public void AskForAction()
            {
                Console.WriteLine("Podaj czynność, którą chcesz wykonać: ");
                var userInput = default(string);

                while (userInput != "exit")
                {

                    Console.WriteLine("add - dodawanie wydatku ");
                    Console.WriteLine("del - usuwanie wydatku ");
                    Console.WriteLine("list - wypisywanie listy wydatków");
                    Console.WriteLine("sum - suma wydatków");
                    Console.WriteLine("exit - wyjście");


                    userInput = Console.ReadLine();
                    userInput = userInput.ToLower();

                    switch (userInput)
                    {
                        case "add":
                            AddWydatek();
                            break;
                        case "del":
                            DeleteWydatek();
                            break;
                        case "list":
                            ListaWydatkow();
                            break;
                        case "sum":
                            SumaWydatek();
                            break;
                        case "exit":
                            Console.WriteLine("Pomyślnie zamknięto aplikacje");
                            break;
                        default:
                            Console.WriteLine("Nie rozpoznano zadania");
                            break;

                    }
                }
            }
        }

        public class Wydatek
        {
            public string Name { get; set; }

            public decimal Amount { get; set; }

            public override string ToString()
            {
                return Name + ";" + Amount.ToString();
            }
        }


        public class WydatekManager
        {
            public List<Wydatek> Wydatki { get; set; }

            private string NazwaPliku { get; set; } = "wydatki.txt";
           

            public WydatekManager()
            {
                Wydatki = new List<Wydatek>();

                if (!File.Exists(NazwaPliku))
                {
                    return;
                }

                var nazwa = File.ReadAllLines(NazwaPliku);

                foreach (var linia in nazwa)
                {
                    var linia_pierwsza = linia.Split(';');
                    if (decimal.TryParse(linia_pierwsza[1], out var amountInDecimal))
                    {
                        AddWydatek(linia_pierwsza[0], amountInDecimal, false);

                    }
                 
                }
                
            }

            public void AddWydatek(string name, decimal amount, bool zapis_domyslny = true)
            {
                var wydatek = new Wydatek
                {
                    Name = name,
                    Amount = amount
                };
                Wydatki.Add(wydatek);

                if (zapis_domyslny)
                {
                    File.WriteAllLines(NazwaPliku, new List<string> { wydatek.ToString() });
                }
            }

            public void DeleteWydatek(string name, bool zapis_domyslny = true)
            {
                foreach (var wydatek in Wydatki)
                {
                    if (wydatek.Name == name)
                    {
                        Wydatki.Remove(wydatek);
                        break;
                    }
                }

                if (zapis_domyslny)
                {
                    var lista_wydatkow_zapis = new List<string>();

                    foreach (var wydatek in lista_wydatkow_zapis)
                    {
                        lista_wydatkow_zapis.Add(wydatek.ToString());
                    }
                    File.Delete(NazwaPliku);
                    File.WriteAllLines(NazwaPliku, lista_wydatkow_zapis);
                }

            }
            public void SumaWydatkow()
            {
                var suma = Wydatki.Sum(sum => sum.Amount);
                Console.WriteLine("Suma wydatków: " + suma + " zł");

            }
  

        public List<string> ListaWydatkow()
        {
            var wydatkiStrings = new List<string>();
            var indexer = 1;

            foreach (var wydatek in Wydatki)
            {
                var wydatekString = indexer + ". " + wydatek.Name + " - " + wydatek.Amount + " zł";
                indexer++;

                wydatkiStrings.Add(wydatekString);
            }

            return wydatkiStrings;
        }


    }





    }
}
