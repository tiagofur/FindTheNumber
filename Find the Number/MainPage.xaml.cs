using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace Find_the_Number
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page

    {
        private Colecao _vm;

        public MainPage()
        {
            this.InitializeComponent();

            _vm = new Colecao();
            _vm.Items = new ObservableCollection<Item>();
            _vm.IsEasyChecked = true;
            this.DataContext = _vm;  // setando o contexto dos dados.
        }


        public class Colecao : ViewModelBase
        {
            private int _maxNumber;
            private int _smaller;
            private int _bigger;
            private int _resposta;
            private bool _concluido;

            public Colecao()
            {
                StartComando = new DelegateCommand<object>(Executar_StartComando);
            }

            public DelegateCommand<object> StartComando { get; set; }
            private void Executar_StartComando(object obj)
            {
                Plays = 0;
                _smaller = 1;
                _bigger = _maxNumber-1;
                CreateNumbers();
                RandonNumber();
                _concluido = false;
            }

            private string _information;
            public string Information
            {
                get { return this._information; }
                set
                {
                    if (value != this._information)
                    {
                        this._information = value;
                        this.NotifyPropertyChanged();
                    }
                }
            }

            private int _plays;
            public int Plays
            {
                get { return this._plays; }
                set
                {
                    if (value != this._plays)
                    {
                        this._plays = value;
                        this.NotifyPropertyChanged();
                    }
                }
            }

            private bool _isEasyChecked;
            public bool IsEasyChecked
            {
                get { return this._isEasyChecked; }
                set
                {
                    if (value != this._isEasyChecked)
                    {
                        this._isEasyChecked = value;
                        this.NotifyPropertyChanged();

                        if (value == true)
                        {
                            _maxNumber = 31;
                        }
                    }
                }
            }

            private bool _isMediumChecked;
            public bool IsMediumChecked
            {
                get { return this._isMediumChecked; }
                set
                {
                    if (value != this._isMediumChecked)
                    {
                        this._isMediumChecked = value;
                        this.NotifyPropertyChanged();

                        if (value == true)
                        {
                            _maxNumber = 51;
                        }
                    }
                }
            }

            private bool _isHardChecked;
            public bool IsHardChecked
            {
                get { return this._isHardChecked; }
                set
                {
                    if (value != this._isHardChecked)
                    {
                        this._isHardChecked = value;
                        this.NotifyPropertyChanged();

                        if (value == true)
                        {
                            _maxNumber = 101;
                        }
                    }
                }
            }

            public ObservableCollection<Item> Items { get; set; }

            public void CreateNumbers()
            {
                Items.Clear();
                for (int i = 1; i < this._maxNumber; i++)
                {
                    Items.Add(new Item(this) { Numero = i });
                }
            }

            public void RandonNumber()
            {
                Random random = new Random();
                _resposta = random.Next(1, _maxNumber-1);
                Information = "Find the number generated randomly!";
            }

            public VerifyNumberResult VerifyNumbersCloser(int selectedNumber)
            {
                if (selectedNumber < _resposta)
                {
                    _smaller = Math.Max(selectedNumber, _smaller);
                    return VerifyNumberResult.Menor;
                }
                else
                {
                    _bigger = Math.Min(selectedNumber, _bigger);
                    return VerifyNumberResult.Maior;
                }
            }

            public void IncrementPlays()
            {
                Plays++;
            }

            public VerifyNumberResult VerifySelectedNumber(int selectedNumber)
            {
                if (_concluido)
                {
                    return VerifyNumberResult.Concluido;
                }

                IncrementPlays();

                VerifyNumberResult result;
                if (selectedNumber == _resposta)
                {
                    result = VerifyNumberResult.Correto;
                    Information = "Congratulations! You found the ramdon number!";
                    _concluido = true;
                }
                else
                {
                    result = VerifyNumbersCloser(selectedNumber);
                    Information = "Numbers closer:  smaller " + _smaller + " and bigger " + _bigger;
                }
                return result;
            }
        }


        public enum VerifyNumberResult
        {
            Concluido,
            Correto,
            Maior,
            Menor,
        }


        public class Item : ViewModelBase
        {
            public Item(IServiceProvider serviceProvicer)
            {
                this.ServiceProvider = serviceProvicer;
                TrocaCorComando = new DelegateCommand<object>(Executar_TrocaCorComando);
            }

            public DelegateCommand<object> TrocaCorComando { get; set; }
            private void Executar_TrocaCorComando(object obj)
            {
                var result = this.GetService<Colecao>().VerifySelectedNumber(Numero);
                switch (result)
                {
                    case VerifyNumberResult.Concluido:
                        // apenas ignora e não muda mais nada
                        break;
                    case VerifyNumberResult.Correto:
                        Cor = new SolidColorBrush(Windows.UI.Colors.LightGreen);
                        break;
                    case VerifyNumberResult.Maior:
                        Cor = new SolidColorBrush(Windows.UI.Colors.LightYellow);
                        break;
                    case VerifyNumberResult.Menor:
                        Cor = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                        break;
                }
            }


            private int _numero;
            public int Numero
            {
                get { return this._numero; }
                set
                {
                    if (value != this._numero)
                    {
                        this._numero = value;
                        this.NotifyPropertyChanged();
                    }
                }
            }

            private Brush _cor = new SolidColorBrush(Windows.UI.Colors.Transparent);
            public Brush Cor
            {
                get { return this._cor; }
                set
                {
                    if (value != this._cor)
                    {
                        this._cor = value;
                        this.NotifyPropertyChanged();
                    }
                }
            }
        }
    }
}
